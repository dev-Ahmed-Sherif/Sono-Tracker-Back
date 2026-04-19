using LinqKit;
using Microsoft.EntityFrameworkCore;
using SonoTracker.Application.Services.Base;
using SonoTracker.Common.Core;
using SonoTracker.Common.DTO.Base;
using SonoTracker.Common.DTO.Tracker.InspectionFloatingUnitClause;
using SonoTracker.Common.DTO.Tracker.InspectionFloatingUnitClause.Parameters;
using SonoTracker.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SonoTracker.Application.Services.Tracker.InspectionFloatingUnitClause
{
    public class InspectionFloatingUnitClauseService : BaseService<Domain.Entities.Tracker.InspectionFloatingUnitClause, AddInspectionFloatingUnitClauseDto, EditInspectionFloatingUnitClauseDto, InspectionFloatingUnitClauseDto, string, string>, IInspectionFloatingUnitClauseService
    {
        public InspectionFloatingUnitClauseService(IServiceBaseParameter<Domain.Entities.Tracker.InspectionFloatingUnitClause> businessBaseParameter)
            : base(businessBaseParameter)
        {
        }

        public override async Task<IFinalResult> GetByIdAsync(object id, CancellationToken cancellationToken = default)
        {
            var idStr = id?.ToString();
            var entity = await UnitOfWork.Repository.FirstOrDefaultAsync(x => x.Id == idStr,
                include: src => src
                    .Include(c => c.InspectionClause)
                    .Include(c => c.Inspection));
            var mapped = Mapper.Map<Domain.Entities.Tracker.InspectionFloatingUnitClause, InspectionFloatingUnitClauseDto>(entity);
            return ResponseResult.PostResult(mapped, HttpStatusCode.OK);
        }

        public override async Task<IFinalResult> GetByIdForEditAsync(object id, CancellationToken cancellationToken = default)
        {
            var idStr = id?.ToString();
            var entity = await UnitOfWork.Repository.FirstOrDefaultAsync(x => x.Id == idStr,
                include: src => src
                    .Include(c => c.InspectionClause)
                    .Include(c => c.Inspection));
            var mapped = Mapper.Map<Domain.Entities.Tracker.InspectionFloatingUnitClause, EditInspectionFloatingUnitClauseDto>(entity);
            return ResponseResult.PostResult(mapped, HttpStatusCode.OK);
        }

        public override async Task<IFinalResult> GetAllAsync(bool disableTracking = false, Expression<Func<Domain.Entities.Tracker.InspectionFloatingUnitClause, bool>> predicate = null, CancellationToken cancellationToken = default)
        {
            var entities = await UnitOfWork.Repository.GetAllAsync(
                include: src => src
                    .Include(c => c.InspectionClause)
                    .Include(c => c.Inspection));
            var mapped = Mapper.Map<IEnumerable<Domain.Entities.Tracker.InspectionFloatingUnitClause>, IEnumerable<InspectionFloatingUnitClauseDto>>(entities);
            return ResponseResult.PostResult(mapped, status: HttpStatusCode.OK, message: HttpStatusCode.OK.ToString());
        }

        public async Task<PagingResult> GetAllPagedAsync(BaseParam<InspectionFloatingUnitClauseFilter> filter, CancellationToken cancellationToken = default)
        {
            var clauseFilter = filter?.Filter ?? new InspectionFloatingUnitClauseFilter();
            var limit = filter.PageSize;
            var offset = --filter.PageNumber * filter.PageSize;

            var query = await UnitOfWork.Repository.FindPagedAsync(
                predicate: PredicateBuilderFunction(clauseFilter),
                pageNumber: offset,
                pageSize: limit,
                filter.OrderByValue,
                include: src => src
                    .Include(c => c.InspectionClause)
                    .Include(c => c.Inspection),
                cancellationToken: cancellationToken);

            var data = Mapper.Map<IEnumerable<Domain.Entities.Tracker.InspectionFloatingUnitClause>, IEnumerable<InspectionFloatingUnitClauseDto>>(query.Item2);
            return new PagingResult(filter.PageNumber, filter.PageSize, query.Item1, data, status: HttpStatusCode.OK, MessagesConstants.Success);
        }

        public override async Task<IFinalResult> AddAsync(AddInspectionFloatingUnitClauseDto dto, CancellationToken cancellationToken = default)
        {
            var mapped = Mapper.Map<Domain.Entities.Tracker.InspectionFloatingUnitClause>(dto);
            UnitOfWork.Repository.Add(mapped);
            await UnitOfWork.SaveChangesAsync(cancellationToken);
            return ResponseResult.PostResult(mapped, status: HttpStatusCode.Created, message: HttpStatusCode.Created.ToString());
        }

        public override async Task<IFinalResult> UpdateAsync(AddInspectionFloatingUnitClauseDto dto, CancellationToken cancellationToken = default)
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

        static Expression<Func<Domain.Entities.Tracker.InspectionFloatingUnitClause, bool>> PredicateBuilderFunction(InspectionFloatingUnitClauseFilter filter)
        {
            var predicate = PredicateBuilder.New<Domain.Entities.Tracker.InspectionFloatingUnitClause>(true);

            if (!string.IsNullOrEmpty(filter.InspectionId))
                predicate = predicate.And(x => x.InspectionId == filter.InspectionId);

            if (!string.IsNullOrEmpty(filter.InspectionClauseId))
                predicate = predicate.And(x => x.InspectionClauseId == filter.InspectionClauseId);

            if (filter.IsInspected.HasValue)
                predicate = predicate.And(x => x.IsInspected == filter.IsInspected.Value);

            return predicate;
        }
    }
}
