using ClosedXML.Excel;
using FuzzySharp;
using Microsoft.AspNetCore.Http;
using SonoTracker.Domain.Enum;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SonoTracker.Common.Helpers
{
    public sealed class TripExcelPersonRow
    {
        public required string Name { get; init; }
        public required string Identity { get; init; }
        public required string NationalityId { get; init; }
        public required string NationalityRaw { get; init; }
        public required Gender Gender { get; init; }
        public IDType IDType { get; init; } = IDType.Passport;
        public string Job { get; init; } = "—";
        public string Mobile { get; init; } = "00000000000";
        public string Email { get; init; } = "noreply@local";
        public int ExcelRowNumber { get; init; }
    }

    public sealed class TripExcelImportResult
    {
        public bool Success { get; init; }
        public string? Error { get; init; }
        public IReadOnlyList<TripExcelPersonRow> Rows { get; init; } = [];

        public static TripExcelImportResult Ok(IReadOnlyList<TripExcelPersonRow> rows) =>
            new() { Success = true, Rows = rows };

        public static TripExcelImportResult Fail(string error) =>
            new() { Success = false, Error = error };
    }

    /// <summary>
    /// Reads TripPassenger / TripStaff Excel templates under wwwroot/Templates.
    /// Nationality fuzzy match uses TokenSetRatio with threshold <see cref="NationalityMatchThreshold"/>.
    /// </summary>
    public static class TripPersonExcelImporter
    {
        /// <summary>Minimum FuzzySharp TokenSetRatio score to accept a nationality match (NameAr/NameEn).</summary>
        public const int NationalityMatchThreshold = 65;

        public const string GenderHeader = "الجنس";

        /// <summary>Minimum FuzzySharp Ratio score to accept a gender alias match.</summary>
        public const int GenderMatchThreshold = 85;

        public static readonly string[] PassengerRequiredHeaders =
        [
            "اسم السائح",
            "الجنسية",
            "رقم الباسبور",
            GenderHeader
        ];

        public static readonly string[] StaffRequiredHeaders =
        [
            "الاسم",
            "الجنسية",
            "رقم الباسبور/الرقم القومى",
            GenderHeader
        ];

        public static TripExcelImportResult ImportPassengers(
            IFormFile file,
            IEnumerable<(string Id, string? NameAr, string? NameEn)> nationalities)
        {
            if (file == null || file.Length == 0)
                return TripExcelImportResult.Fail("مرفق الركاب: الملف مطلوب.");

            using var stream = BufferFormFile(file);
            return ImportPassengers(stream, file.FileName, nationalities);
        }

        public static TripExcelImportResult ImportPassengers(
            Stream stream,
            string fileName,
            IEnumerable<(string Id, string? NameAr, string? NameEn)> nationalities)
        {
            return Import(
                stream,
                fileName,
                "مرفق الركاب",
                PassengerRequiredHeaders,
                nameHeader: "اسم السائح",
                nationalityHeader: "الجنسية",
                identityHeader: "رقم الباسبور",
                defaultJob: "سائح",
                defaultIdType: IDType.Passport,
                nationalities);
        }

        public static TripExcelImportResult ImportStaff(
            IFormFile file,
            IEnumerable<(string Id, string? NameAr, string? NameEn)> nationalities)
        {
            if (file == null || file.Length == 0)
                return TripExcelImportResult.Fail("مرفق الطاقم: الملف مطلوب.");

            using var stream = BufferFormFile(file);
            return ImportStaff(stream, file.FileName, nationalities);
        }

        public static TripExcelImportResult ImportStaff(
            Stream stream,
            string fileName,
            IEnumerable<(string Id, string? NameAr, string? NameEn)> nationalities)
        {
            return Import(
                stream,
                fileName,
                "مرفق الطاقم",
                StaffRequiredHeaders,
                nameHeader: "الاسم",
                nationalityHeader: "الجنسية",
                identityHeader: "رقم الباسبور/الرقم القومى",
                defaultJob: "طاقم",
                defaultIdType: null,
                nationalities);
        }

        /// <summary>
        /// Buffers the form file so Excel parsing does not consume the upload stream.
        /// </summary>
        public static MemoryStream BufferFormFile(IFormFile file)
        {
            var ms = new MemoryStream();
            file.CopyTo(ms);
            ms.Position = 0;
            return ms;
        }

        static TripExcelImportResult Import(
            Stream stream,
            string fileName,
            string fileLabel,
            string[] requiredHeaders,
            string nameHeader,
            string nationalityHeader,
            string identityHeader,
            string defaultJob,
            IDType? defaultIdType,
            IEnumerable<(string Id, string? NameAr, string? NameEn)> nationalities)
        {
            if (stream == null || stream.Length == 0)
                return TripExcelImportResult.Fail($"{fileLabel}: الملف مطلوب.");

            var extension = Path.GetExtension(fileName)?.ToLowerInvariant();
            if (extension is not ".xlsx" and not ".xlsm")
                return TripExcelImportResult.Fail($"{fileLabel}: يجب رفع ملف Excel بصيغة .xlsx (القوالب المتوفرة في Templates).");

            var nationalityList = nationalities?.ToList() ?? [];
            if (nationalityList.Count == 0)
                return TripExcelImportResult.Fail("لا توجد جنسيات مسجلة في قاعدة البيانات لمطابقتها.");

            try
            {
                // Copy so ClosedXML can own/dispose its stream without affecting the caller buffer used for upload.
                using var excelStream = new MemoryStream();
                if (stream.CanSeek)
                    stream.Position = 0;
                stream.CopyTo(excelStream);
                excelStream.Position = 0;

                using var workbook = new XLWorkbook(excelStream);
                var worksheet = workbook.Worksheets.FirstOrDefault();
                if (worksheet == null)
                    return TripExcelImportResult.Fail($"{fileLabel}: الملف لا يحتوي على ورقة عمل.");

                var usedRange = worksheet.RangeUsed();
                if (usedRange == null)
                    return TripExcelImportResult.Fail($"{fileLabel}: لا يوجد بيانات مضافضة");

                var headerRow = usedRange.FirstRow();
                var headerMap = BuildHeaderMap(headerRow);

                var headerValidation = ValidateHeaders(fileLabel, requiredHeaders, headerMap.Keys);
                if (headerValidation != null)
                    return TripExcelImportResult.Fail(headerValidation);

                var nameCol = headerMap[nameHeader];
                var nationalityCol = headerMap[nationalityHeader];
                var identityCol = headerMap[identityHeader];
                var genderCol = headerMap[GenderHeader];

                var rows = new List<TripExcelPersonRow>();
                var lastRow = usedRange.LastRow().RowNumber();

                for (var rowNumber = headerRow.RowNumber() + 1; rowNumber <= lastRow; rowNumber++)
                {
                    var name = GetCellText(worksheet, rowNumber, nameCol);
                    var nationalityRaw = GetCellText(worksheet, rowNumber, nationalityCol);
                    var identity = GetCellText(worksheet, rowNumber, identityCol);
                    var genderRaw = GetCellText(worksheet, rowNumber, genderCol);

                    if (string.IsNullOrWhiteSpace(name) &&
                        string.IsNullOrWhiteSpace(nationalityRaw) &&
                        string.IsNullOrWhiteSpace(identity) &&
                        string.IsNullOrWhiteSpace(genderRaw))
                        continue;

                    if (string.IsNullOrWhiteSpace(name))
                        return TripExcelImportResult.Fail($"{fileLabel}: الصف {rowNumber} — الاسم مطلوب.");

                    if (string.IsNullOrWhiteSpace(identity))
                        return TripExcelImportResult.Fail($"{fileLabel}: الصف {rowNumber} — رقم الهوية/الجواز مطلوب.");

                    if (string.IsNullOrWhiteSpace(nationalityRaw))
                        return TripExcelImportResult.Fail($"{fileLabel}: الصف {rowNumber} — الجنسية مطلوبة.");

                    if (string.IsNullOrWhiteSpace(genderRaw))
                        return TripExcelImportResult.Fail($"{fileLabel}: الصف {rowNumber} — الجنس مطلوب.");

                    var match = MatchNationality(nationalityRaw, nationalityList);
                    if (match == null)
                        return TripExcelImportResult.Fail(
                            $"{fileLabel}: الصف {rowNumber} — تعذر مطابقة الجنسية '{nationalityRaw}' (الحد الأدنى للتشابه {NationalityMatchThreshold}%).");

                    var gender = TryParseGender(genderRaw);
                    if (gender == null)
                        return TripExcelImportResult.Fail(
                            $"{fileLabel}: الصف {rowNumber} — تعذر مطابقة الجنس '{genderRaw}'. القيم المقبولة: ذكر، أنثى، Male، Female.");

                    var idType = defaultIdType ?? InferIdType(identity);

                    rows.Add(new TripExcelPersonRow
                    {
                        Name = name.Trim(),
                        Identity = identity.Trim(),
                        NationalityId = match.Value.Id,
                        NationalityRaw = nationalityRaw.Trim(),
                        Gender = gender.Value,
                        Job = defaultJob,
                        IDType = idType,
                        ExcelRowNumber = rowNumber
                    });
                }

                if (rows.Count == 0)
                    return TripExcelImportResult.Fail($"{fileLabel}: لا يوجد بيانات مضافضة");

                return TripExcelImportResult.Ok(rows);
            }
            catch (Exception ex)
            {
                return TripExcelImportResult.Fail($"{fileLabel}: فشل قراءة ملف Excel — {ex.Message}");
            }
        }

        static Dictionary<string, int> BuildHeaderMap(IXLRangeRow headerRow)
        {
            var map = new Dictionary<string, int>(StringComparer.Ordinal);
            foreach (var cell in headerRow.CellsUsed())
            {
                var header = cell.GetString()?.Trim();
                if (string.IsNullOrWhiteSpace(header))
                    continue;

                // First occurrence wins if duplicates appear.
                if (!map.ContainsKey(header))
                    map[header] = cell.Address.ColumnNumber;
            }

            return map;
        }

        /// <summary>
        /// Requires every expected template column name to be present (case-sensitive Arabic).
        /// Extra columns are allowed; renamed/missing required headers are rejected before reading data.
        /// </summary>
        static string? ValidateHeaders(string fileLabel, string[] requiredHeaders, IEnumerable<string> actualHeaders)
        {
            var actual = actualHeaders.ToHashSet(StringComparer.Ordinal);
            var missing = requiredHeaders.Where(h => !actual.Contains(h)).ToList();
            if (missing.Count == 0)
                return null;

            return $"{fileLabel}: أعمدة القالب غير مطابقة. المفقودة/المُعاد تسميتها: {string.Join("، ", missing)}. " +
                   $"الأعمدة المتوقعة: {string.Join("، ", requiredHeaders)}.";
        }

        static string GetCellText(IXLWorksheet worksheet, int rowNumber, int columnNumber)
        {
            var cell = worksheet.Cell(rowNumber, columnNumber);
            if (cell.IsEmpty())
                return string.Empty;

            return cell.GetFormattedString()?.Trim() ?? cell.GetString()?.Trim() ?? string.Empty;
        }

        static (string Id, int Score)? MatchNationality(
            string excelValue,
            IReadOnlyList<(string Id, string? NameAr, string? NameEn)> nationalities)
        {
            var normalizedInput = LookupDuplicateGuard.NormalizeText(excelValue);
            if (string.IsNullOrWhiteSpace(normalizedInput))
                return null;

            string? bestId = null;
            var bestScore = -1;

            foreach (var (id, nameAr, nameEn) in nationalities)
            {
                var scoreAr = string.IsNullOrWhiteSpace(nameAr)
                    ? 0
                    : Fuzz.TokenSetRatio(normalizedInput, LookupDuplicateGuard.NormalizeText(nameAr));

                var scoreEn = string.IsNullOrWhiteSpace(nameEn)
                    ? 0
                    : Fuzz.TokenSetRatio(normalizedInput, LookupDuplicateGuard.NormalizeText(nameEn));

                var score = Math.Max(scoreAr, scoreEn);
                if (score > bestScore)
                {
                    bestScore = score;
                    bestId = id;
                }
            }

            if (bestId == null || bestScore < NationalityMatchThreshold)
                return null;

            return (bestId, bestScore);
        }

        /// <summary>
        /// Maps Excel الجنس values to <see cref="Gender"/> using exact aliases then light FuzzySharp Ratio.
        /// Accepts: ذكر/أنثى, Male/Female, م/ف (and common variants).
        /// </summary>
        static Gender? TryParseGender(string excelValue)
        {
            var normalizedInput = NormalizeGenderToken(excelValue);
            if (string.IsNullOrWhiteSpace(normalizedInput))
                return null;

            // Exact / short aliases (after normalization).
            if (IsMaleAlias(normalizedInput))
                return Gender.Male;
            if (IsFemaleAlias(normalizedInput))
                return Gender.Female;

            Gender? best = null;
            var bestScore = -1;

            foreach (var (gender, alias) in GenderAliases)
            {
                var score = Fuzz.Ratio(normalizedInput, NormalizeGenderToken(alias));
                if (score > bestScore)
                {
                    bestScore = score;
                    best = gender;
                }
            }

            if (best == null || bestScore < GenderMatchThreshold)
                return null;

            return best;
        }

        static readonly (Gender Gender, string Alias)[] GenderAliases =
        [
            (Gender.Male, "ذكر"),
            (Gender.Male, "male"),
            (Gender.Male, "m"),
            (Gender.Male, "م"),
            (Gender.Female, "أنثى"),
            (Gender.Female, "انثى"),
            (Gender.Female, "انثي"),
            (Gender.Female, "female"),
            (Gender.Female, "f"),
            (Gender.Female, "ف")
        ];

        static bool IsMaleAlias(string normalized) =>
            normalized is "ذكر" or "male" or "m" or "م";

        static bool IsFemaleAlias(string normalized) =>
            // NormalizeGenderToken maps أنثى/انثى → انثه and ى → ي
            normalized is "انثه" or "انثي" or "female" or "f" or "ف";

        /// <summary>Normalize gender cell text: trim, collapse spaces, lowercase, unify common Arabic letter variants.</summary>
        static string NormalizeGenderToken(string value)
        {
            var normalized = LookupDuplicateGuard.NormalizeText(value);
            if (string.IsNullOrWhiteSpace(normalized))
                return string.Empty;

            // Unify alef / yaa variants so أنثى and انثي match reliably.
            return normalized
                .Replace('أ', 'ا')
                .Replace('إ', 'ا')
                .Replace('آ', 'ا')
                .Replace('ى', 'ي')
                .Replace('ة', 'ه');
        }

        static IDType InferIdType(string identity)
        {
            var digits = new string(identity.Where(char.IsDigit).ToArray());
            // Egyptian national ID is typically 14 digits.
            return digits.Length == 14 ? IDType.IDCard : IDType.Passport;
        }
    }
}
