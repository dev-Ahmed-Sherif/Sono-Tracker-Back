using LinqKit;
using Microsoft.EntityFrameworkCore;
using SonoTracker.Application.Services.Base;
using SonoTracker.Common.Core;
using SonoTracker.Common.DTO.Base;
using SonoTracker.Common.DTO.Tracker.InspectionClause;
using SonoTracker.Common.DTO.Tracker.InspectionClause.Parameters;
using SonoTracker.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SonoTracker.Application.Services.Tracker.InspectionClause
{
    public class InspectionClauseService : BaseService<Domain.Entities.Tracker.InspectionClause, AddInspectionClauseDto, EditInspectionClauseDto, InspectionClauseDto, string, string>, IInspectionClauseService
    {
        public InspectionClauseService(IServiceBaseParameter<Domain.Entities.Tracker.InspectionClause> businessBaseParameter)
            : base(businessBaseParameter)
        {
        }

        public override async Task<IFinalResult> GetByIdAsync(object id, CancellationToken cancellationToken = default)
        {
            var idStr = id?.ToString();
            var entity = await UnitOfWork.Repository.FirstOrDefaultAsync(x => x.Id == idStr,
                include: src => src
                    .Include(c => c.InspectionType)
                    .Include(c => c.Parent));
            var mapped = Mapper.Map<Domain.Entities.Tracker.InspectionClause, InspectionClauseDto>(entity);
            return ResponseResult.PostResult(mapped, HttpStatusCode.OK);
        }

        public override async Task<IFinalResult> GetByIdForEditAsync(object id, CancellationToken cancellationToken = default)
        {
            var idStr = id?.ToString();
            var entity = await UnitOfWork.Repository.FirstOrDefaultAsync(x => x.Id == idStr,
                include: src => src
                    .Include(c => c.InspectionType)
                    .Include(c => c.Parent));
            var mapped = Mapper.Map<Domain.Entities.Tracker.InspectionClause, EditInspectionClauseDto>(entity);
            return ResponseResult.PostResult(mapped, HttpStatusCode.OK);
        }

        public override async Task<IFinalResult> GetAllAsync(bool disableTracking = false, Expression<Func<Domain.Entities.Tracker.InspectionClause, bool>> predicate = null, CancellationToken cancellationToken = default)
        {
            var entities = await UnitOfWork.Repository.GetAllAsync(
                include: src => src
                    .Include(c => c.InspectionType)
                    .Include(c => c.Parent));
            var mapped = Mapper.Map<IEnumerable<Domain.Entities.Tracker.InspectionClause>, IEnumerable<InspectionClauseDto>>(entities);
            return ResponseResult.PostResult(mapped, status: HttpStatusCode.OK, message: HttpStatusCode.OK.ToString());
        }

        public async Task<PagingResult> GetAllPagedAsync(BaseParam<InspectionClauseFilter> filter, CancellationToken cancellationToken = default)
        {
            var clauseFilter = filter?.Filter ?? new InspectionClauseFilter();
            var limit = filter.PageSize;
            var offset = --filter.PageNumber * filter.PageSize;

            var query = await UnitOfWork.Repository.FindPagedAsync(
                predicate: PredicateBuilderFunction(clauseFilter),
                pageNumber: offset,
                pageSize: limit,
                filter.OrderByValue,
                include: src => src
                    .Include(c => c.InspectionType)
                    .Include(c => c.Parent),
                cancellationToken: cancellationToken);

            var data = Mapper.Map<IEnumerable<Domain.Entities.Tracker.InspectionClause>, IEnumerable<InspectionClauseDto>>(query.Item2);
            return new PagingResult(filter.PageNumber, filter.PageSize, query.Item1, data, status: HttpStatusCode.OK, MessagesConstants.Success);
        }

        public override async Task<IFinalResult> AddAsync(AddInspectionClauseDto dto, CancellationToken cancellationToken = default)
        {
            var mapped = Mapper.Map<Domain.Entities.Tracker.InspectionClause>(dto);
            UnitOfWork.Repository.Add(mapped);
            await UnitOfWork.SaveChangesAsync(cancellationToken);
            return ResponseResult.PostResult(mapped, status: HttpStatusCode.Created, message: HttpStatusCode.Created.ToString());
        }

        public override async Task<IFinalResult> UpdateAsync(AddInspectionClauseDto dto, CancellationToken cancellationToken = default)
        {
            var entityToUpdate = await UnitOfWork.Repository.GetAsync(dto.Id);
            var newEntity = Mapper.Map(dto, entityToUpdate);
            UnitOfWork.Repository.Update(entityToUpdate, newEntity);
            var affectedRows = await UnitOfWork.SaveChangesAsync(cancellationToken);
            if (affectedRows > 0)
                Result = ResponseResult.PostResult(result: true, status: HttpStatusCode.Accepted, message: MessagesConstants.UpdateSuccess);
            return Result;
        }

        public override async Task<IFinalResult> DeleteAsync(object id, CancellationToken cancellationToken = default)
        {
            var entityToDelete = await UnitOfWork.Repository.GetAsync(id);
            UnitOfWork.Repository.Remove(entityToDelete);
            var affectedRows = await UnitOfWork.SaveChangesAsync(cancellationToken);
            if (affectedRows > 0)
                Result = ResponseResult.PostResult(result: true, status: HttpStatusCode.Accepted, message: MessagesConstants.DeleteSuccess);
            return Result;
        }

        public async Task<IFinalResult> DeleteRangeAsync(IEnumerable<string> ids, CancellationToken cancellationToken = default)
        {
            var idsList = ids.ToList();
            var entitiesToDelete = await UnitOfWork.Repository.FindAsync(d => idsList.Contains(d.Id), cancellationToken: cancellationToken);
            UnitOfWork.Repository.RemoveRange(entitiesToDelete);
            var rows = await UnitOfWork.SaveChangesAsync(cancellationToken);
            return ResponseResult.PostResult(result: rows, status: HttpStatusCode.NoContent, message: MessagesConstants.DeleteSuccess);
        }

        static Expression<Func<Domain.Entities.Tracker.InspectionClause, bool>> PredicateBuilderFunction(InspectionClauseFilter filter)
        {
            var predicate = PredicateBuilder.New<Domain.Entities.Tracker.InspectionClause>(true);

            if (!string.IsNullOrWhiteSpace(filter.Code))
                predicate = predicate.And(x => x.Code.Contains(filter.Code));

            if (!string.IsNullOrWhiteSpace(filter.Name))
                predicate = predicate.And(x => x.Name.Contains(filter.Name));

            if (!string.IsNullOrEmpty(filter.ParentId))
                predicate = predicate.And(x => x.ParentId == filter.ParentId);

            if (!string.IsNullOrEmpty(filter.InspectionTypeId))
                predicate = predicate.And(x => x.InspectionTypeId == filter.InspectionTypeId);

            return predicate;
        }
    }
}
