using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using SonoTracker.Application.Services.Base;
using SonoTracker.Common.Core;
using SonoTracker.Common.DTO.Base;
using SonoTracker.Common.DTO.Tracker.TripAttachment;
using SonoTracker.Common.DTO.Tracker.TripAttachment.Parameters;
using SonoTracker.Domain;

namespace SonoTracker.Application.Services.Tracker.TripAttachment
{
    public class TripAttachmentService(IServiceBaseParameter<Entities.Tracker.TripAttachment> businessBaseParameter)
        : BaseService<Entities.Tracker.TripAttachment, AddTripAttachmentDto, EditTripAttachmentDto, TripAttachmentDto, string, string>(businessBaseParameter), ITripAttachmentService
    {
        public override async Task<IFinalResult> GetByIdForEditAsync(object id, CancellationToken cancellationToken = default)
        {
            var idStr = id?.ToString();
            var entity = await UnitOfWork.Repository.FirstOrDefaultAsync(x => x.Id == idStr,
                include: src => src.Include(t => t.Attachment)
                    .Include(t => t.TripInformation),
                cancellationToken: cancellationToken);
            var mapped = Mapper.Map<Entities.Tracker.TripAttachment, EditTripAttachmentDto>(entity);
            return ResponseResult.PostResult(mapped, HttpStatusCode.OK);
        }

        public override async Task<IFinalResult> GetByIdAsync(object id, CancellationToken cancellationToken = default)
        {
            var idStr = id?.ToString();
            var entity = await UnitOfWork.Repository.FirstOrDefaultAsync(x => x.Id == idStr,
                include: src => src.Include(t => t.Attachment)
                    .Include(t => t.TripInformation),
                cancellationToken: cancellationToken);

            var mapped = Mapper.Map<Entities.Tracker.TripAttachment, TripAttachmentDto>(entity);

            return ResponseResult.PostResult(mapped, HttpStatusCode.OK);
        }

        public override async Task<IFinalResult> GetAllAsync(bool disableTracking = false, Expression<Func<Entities.Tracker.TripAttachment, bool>> predicate = null, CancellationToken cancellationToken = default)
        {
            var entity = await UnitOfWork.Repository.GetAllAsync(
                include: src => src.Include(t => t.Attachment)
                    .Include(t => t.TripInformation),
                cancellationToken: cancellationToken);

            var filteredEntities = IsSuperAdmin()
                ? entity
                : entity.Where(e => e.IsDeleted != true);

            var mapped = Mapper.Map<IEnumerable<Entities.Tracker.TripAttachment>, IEnumerable<TripAttachmentDto>>(filteredEntities);
            return ResponseResult.PostResult(mapped, status: HttpStatusCode.OK,
                message: HttpStatusCode.OK.ToString());
        }

        public async Task<PagingResult> GetAllPagedAsync(BaseParam<TripAttachmentFilter> filter, CancellationToken cancellationToken = default)
        {
            var isSuperAdmin = IsSuperAdmin();
            var tripAttachmentFilter = filter?.Filter ?? new TripAttachmentFilter();
            if (!isSuperAdmin)
                tripAttachmentFilter.IsDeleted = false;

            var limit = filter.PageSize;
            var offset = --filter.PageNumber * filter.PageSize;

            var query = await UnitOfWork.Repository.FindPagedAsync(
                predicate: PredicateBuilderFunction(tripAttachmentFilter),
                pageNumber: offset,
                pageSize: limit,
                filter.OrderByValue,
                include: src => src.Include(t => t.Attachment)
                    .Include(t => t.TripInformation),
                cancellationToken: cancellationToken);

            var items = isSuperAdmin ? query.Item2 : query.Item2.Where(x => x.IsDeleted != true);
            var data = Mapper.Map<IEnumerable<Entities.Tracker.TripAttachment>, IEnumerable<TripAttachmentDto>>(items);

            return new PagingResult(filter.PageNumber, filter.PageSize, query.Item1, data, status: HttpStatusCode.OK, MessagesConstants.Success);
        }

        public async Task<PagingResult> GetDropDownAsync(BaseParam<SearchCriteriaFilter> filter, CancellationToken cancellationToken = default)
        {
            var isSuperAdmin = IsSuperAdmin();
            var limit = filter.PageSize;
            var offset = --filter.PageNumber * filter.PageSize;

            var predicate = DropDownPredicateBuilderFunction(filter.Filter);

            var query = await UnitOfWork.Repository.FindPagedAsync(
                predicate: predicate,
                pageNumber: offset,
                pageSize: limit,
                include: src => src.Include(t => t.Attachment)
                    .Include(t => t.TripInformation),
                cancellationToken: cancellationToken);

            var items = isSuperAdmin ? query.Item2 : query.Item2.Where(x => x.IsDeleted != true);
            var data = Mapper.Map<IEnumerable<Entities.Tracker.TripAttachment>, IEnumerable<TripAttachmentDto>>(items);

            return new PagingResult(filter.PageNumber, filter.PageSize, query.Item1, data, status: HttpStatusCode.OK, MessagesConstants.Success);
        }

        public async Task<IFinalResult> GetAllFilterAsync(TripAttachmentFilter filter, CancellationToken cancellationToken = default)
        {
            var entities = await UnitOfWork.Repository.FindAsync(
                predicate: PredicateBuilderFunction(filter),
                cancellationToken: cancellationToken,
                include: src => src.Include(t => t.Attachment)
                    .Include(t => t.TripInformation));

            var data = Mapper.Map<IEnumerable<Entities.Tracker.TripAttachment>, IEnumerable<TripAttachmentDto>>(entities.Where(x => x.IsDeleted != true));

            return ResponseResult.PostResult(data, status: HttpStatusCode.OK,
                message: HttpStatusCode.OK.ToString());
        }

        static Expression<Func<Entities.Tracker.TripAttachment, bool>> PredicateBuilderFunction(TripAttachmentFilter filter)
        {
            var predicate = PredicateBuilder.New<Entities.Tracker.TripAttachment>(x => x.IsDeleted == filter.IsDeleted);

            if (!string.IsNullOrEmpty(filter.TripInformationId))
            {
                predicate = predicate.And(x => x.TripInformationId == filter.TripInformationId);
            }

            if (!string.IsNullOrEmpty(filter.AttachmentId))
            {
                predicate = predicate.And(x => x.AttachmentId == filter.AttachmentId);
            }

            return predicate;
        }

        static Expression<Func<Entities.Tracker.TripAttachment, bool>> DropDownPredicateBuilderFunction(SearchCriteriaFilter filter)
        {
            var predicate = PredicateBuilder.New<Entities.Tracker.TripAttachment>(x => !x.IsDeleted);
            if (!string.IsNullOrWhiteSpace(filter?.SearchCriteria))
            {
                predicate = predicate.And(b => b.Attachment.FileName.Contains(filter.SearchCriteria));
            }
            return predicate;
        }

        public async Task<IFinalResult> DeleteRangeAsync(IEnumerable<string> ids, CancellationToken cancellationToken = default)
        {
            var idsList = ids.ToList();
            var entitiesToDelete = await UnitOfWork.Repository.FindAsync(d => idsList.Contains(d.Id), cancellationToken: cancellationToken);

            UnitOfWork.Repository.RemoveRange(entitiesToDelete);

            var rows = await UnitOfWork.SaveChangesAsync(cancellationToken);

            return ResponseResult.PostResult(result: rows, status: HttpStatusCode.NoContent, message: MessagesConstants.DeleteSuccess);
        }

        public async Task<IFinalResult> DeleteRangeWithAttachIdRangeAsync(IEnumerable<string> ids, CancellationToken cancellationToken = default)
        {
            var entitiesToDelete = await UnitOfWork.Repository.FindAsync(d => ids.Contains(d.AttachmentId), cancellationToken: cancellationToken);

            UnitOfWork.Repository.RemoveRange(entitiesToDelete);

            await UnitOfWork.SaveChangesAsync(cancellationToken);

            return entitiesToDelete == null
                ? ResponseResult.PostResult(false, status: HttpStatusCode.BadRequest, message: MessagesConstants.DeleteError)
                : ResponseResult.PostResult(true, status: HttpStatusCode.OK, message: MessagesConstants.DeleteSuccess);
        }
    }
}
