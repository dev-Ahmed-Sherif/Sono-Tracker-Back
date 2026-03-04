using SonoTracker.Common.DTO.Tracker.Accident;
using SonoTracker.Domain.Entities.Tracker;
using System;

namespace SonoTracker.Application.Mapping
{
    public partial class MappingService
    {
        public void MapAccident()
        {
            CreateMap<Accident, AccidentDto>()
                .ForMember(des => des.Number, opt => opt.MapFrom(src => src.Code))
                .ForMember(des => des.AccidentDate, opt => opt.MapFrom(src => src.AccidentDate.ToDateTime(TimeOnly.MinValue)))
                .ForMember(des => des.ResponseDate, opt => opt.MapFrom(src => src.ResponseDate.HasValue ? src.ResponseDate.Value.ToDateTime(TimeOnly.MinValue) : default))
                .ForMember(des => des.AccidentType, opt => opt.MapFrom(src => src.AccidentType.NameAr))
                .ForMember(des => des.FloatingUnit, opt => opt.MapFrom(src => src.FloatingUnit.NameAr))
                .ForMember(des => des.Organization, opt => opt.MapFrom(src => src.Organization.NameAr))
                .ForMember(des => des.Town, opt => opt.MapFrom(src => src.Town.NameAr))
                .ForMember(des => des.CaseId, opt => opt.MapFrom(src => src.Case))
                .ForMember(des => des.Case, opt => opt.MapFrom(src => src.Case.GetName()));

            CreateMap<Accident, EditAccidentDto>()
                .ForMember(des => des.Number, opt => opt.MapFrom(src => src.Code))
                .ForMember(des => des.AccidentDate, opt => opt.MapFrom(src => src.AccidentDate.ToDateTime(TimeOnly.MinValue)))
                .ForMember(des => des.ResponseDate, opt => opt.MapFrom(src => src.ResponseDate.HasValue ? src.ResponseDate.Value.ToDateTime(TimeOnly.MinValue) : default))
                .ForMember(des => des.AccidentType, opt => opt.MapFrom(src => src.AccidentType.NameAr))
                .ForMember(des => des.FloatingUnit, opt => opt.MapFrom(src => src.FloatingUnit.NameAr))
                .ForMember(des => des.Organization, opt => opt.MapFrom(src => src.Organization.NameAr))
                .ForMember(des => des.Town, opt => opt.MapFrom(src => src.Town.NameAr))
                .ForMember(des => des.CaseId, opt => opt.MapFrom(src => src.Case))
                .ForMember(des => des.Case, opt => opt.MapFrom(src => src.Case.GetName()));

            CreateMap<Accident, AddAccidentDto>()
                .ForMember(des => des.Number, opt => opt.MapFrom(src => src.Code))
                .ForMember(des => des.AccidentDate, opt => opt.MapFrom(src => src.AccidentDate.ToDateTime(TimeOnly.MinValue)))
                .ForMember(des => des.ResponseDate, opt => opt.MapFrom(src => src.ResponseDate.HasValue ? src.ResponseDate.Value.ToDateTime(TimeOnly.MinValue) : (DateTime?)null))
                .ForMember(des => des.Attach, opt => opt.Ignore());

            CreateMap<AddAccidentDto, Accident>()
                .ForMember(des => des.Code, opt => opt.MapFrom(src => src.Number))
                .ForMember(des => des.AccidentDate, opt => opt.MapFrom(src => DateOnly.FromDateTime(src.AccidentDate)))
                .ForMember(des => des.ResponseDate, opt => opt.MapFrom(src => src.ResponseDate.HasValue ? DateOnly.FromDateTime(src.ResponseDate.Value) : (DateOnly?)null))
                .ForMember(des => des.Case, opt => opt.MapFrom(src => src.CaseId))
                .ForMember(des => des.Attach, opt => opt.Ignore());
        }
    }
}
