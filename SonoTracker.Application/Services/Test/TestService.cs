using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Threading.Tasks;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using SonoTracker.Application.Services.Base;
using SonoTracker.Common.Core;
using SonoTracker.Common.DTO.Base;
using SonoTracker.Common.DTO.Test;
using SonoTracker.Common.DTO.Test.Parameters;
using SonoTracker.Common.Helpers.HttpClient;
using SonoTracker.Domain;
using SonoTracker.Integration.FileRepository;

namespace SonoTracker.Application.Services.Test
{
    public class TestService : BaseService<Entities.Test, AddTestDto, EditTestDto, TestDto, Guid, Guid?>, ITestService
    {
        private readonly IFileRepository _fileRepository;
        private readonly MicroServicesUrls _urls;
        public TestService(IServiceBaseParameter<Entities.Test> parameters, IFileRepository fileRepository, MicroServicesUrls urls) : base(parameters)
        {
            _fileRepository = fileRepository;
            _urls = urls;
        }

        public override async Task<IFinalResult> GetByIdForEditAsync(object id)
        {
            var entity = await UnitOfWork.Repository.FirstOrDefaultAsync(x => x.Id == Guid.Parse(id.ToString()),
                include: src => src.Include(t => t.TestAttachments));
            var mapped = Mapper.Map<Entities.Test, EditTestDto>(entity);
            await MapAttachmentInternalAsync(mapped);
            return ResponseResult.PostResult(mapped, HttpStatusCode.OK);
        }


        public async Task<PagingResult> GetAllPagedAsync(BaseParam<TestFilter> filter)
        {
            var limit = filter.PageSize;

            var offset = --filter.PageNumber * filter.PageSize;

            var query = await UnitOfWork.Repository.FindPagedAsync(predicate: PredicateBuilderFunction(filter.Filter), pageNumber: offset, pageSize: limit, filter.OrderByValue);
            
            var data = Mapper.Map<IEnumerable<Entities.Test>, IEnumerable<TestDto>>(query.Result);
            
            return new PagingResult(filter.PageNumber, filter.PageSize, query.Count, data, status: HttpStatusCode.OK, MessagesConstants.Success);
        }



        static Expression<Func<Entities.Test, bool>> PredicateBuilderFunction(TestFilter filter)
        {
            var predicate = PredicateBuilder.New<Entities.Test>(x => x.IsDeleted == filter.IsDeleted);
            //if (!string.IsNullOrWhiteSpace(filter?.NameAr))
            //{
            //    predicate = predicate.And(b => b.CreatedByEmployeeAr.ToLower().Contains(filter.NameAr.ToLower()));
            //}
            return predicate;
        }


        private async Task MapAttachmentInternalAsync(EditTestDto mapped)
        {

            if (mapped != null)
            {
                var tokens = await _fileRepository.GetTokens(mapped.TestAttachments.Select(x => x.FileId).ToList());
                mapped.TestAttachments.ForEach(e =>
                {
                    var token = tokens.First(x => x.Id == e.FileId);
                    e.Url = _urls.DownloadFileWithAppCode + "/" + e.FileId + "?token=" + token.Token;
                });
            }
        }

    }
}
