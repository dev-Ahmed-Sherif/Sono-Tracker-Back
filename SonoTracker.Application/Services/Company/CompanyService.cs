using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net;
using System.Threading.Tasks;
using LinqKit;
using SonoTracker.Application.Services.Base;
using SonoTracker.Application.Services.Test;
using SonoTracker.Common.Core;
using SonoTracker.Common.DTO.Base;
using SonoTracker.Common.DTO.Company;
using SonoTracker.Common.DTO.Company.Parameters;
using SonoTracker.Common.Infrastructure.Repository.Company;
using SonoTracker.Domain;

namespace SonoTracker.Application.Services.Company
{
    public class CompanyService : BaseService<Entities.Company, AddCompanyDto, EditCompanyDto, CompanyDto, Guid, Guid?>, ICompanyService
    {
        private readonly ICompanyCustomRepository _companyCustomRepository;
        public CompanyService(IServiceBaseParameter<Entities.Company> parameters , ICompanyCustomRepository companyCustomRepository) : base(parameters)
        {
            _companyCustomRepository = companyCustomRepository;
        }


        public override async Task<IFinalResult> AddAsync(AddCompanyDto model)
        {
            var entity = Mapper.Map<AddCompanyDto, Entities.Company>(model);

            var result = await _companyCustomRepository.AddCompanyAsync(entity);

            var affectedRows = await UnitOfWork.SaveChangesAsync();

            if(affectedRows <= 0 )  return ResponseResult.PostResult(false , HttpStatusCode.BadRequest , null , MessagesConstants.AddError);

            return ResponseResult.PostResult(true, HttpStatusCode.Created, null, MessagesConstants.AddSuccess);
        }

        public async Task<PagingResult> GetAllPagedAsync(BaseParam<CompanyFilter> filter)
        {
            var limit = filter.PageSize;
            
            var offset = --filter.PageNumber * filter.PageSize;

            var query = await UnitOfWork.Repository.FindPagedAsync(predicate: PredicateBuilderFunction(filter.Filter), pageNumber: offset, pageSize: limit, filter.OrderByValue);

            var data = Mapper.Map<IEnumerable<Entities.Company>, IEnumerable<CompanyDto>>(query.Result);

            return new PagingResult(filter.PageNumber, filter.PageSize, query.Count, data, status: HttpStatusCode.OK, MessagesConstants.Success);
        }



        static Expression<Func<Entities.Company, bool>> PredicateBuilderFunction(CompanyFilter filter)
        {
            var predicate = PredicateBuilder.New<Entities.Company>(x => x.IsDeleted == filter.IsDeleted);
            //if (!string.IsNullOrWhiteSpace(filter?.NameAr))
            //{
            //    predicate = predicate.And(b => b.CreatedByEmployeeAr.ToLower().Contains(filter.NameAr.ToLower()));
            //}
            return predicate;
        }



    }
}
