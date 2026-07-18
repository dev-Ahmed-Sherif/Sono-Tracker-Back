using System;
using System.Diagnostics.CodeAnalysis;
using SonoTracker.Common.DTO.Tracker.TripAttachment;
using SonoTracker.Domain.Entities.Tracker;

namespace SonoTracker.Application.Mapping
{
    public partial class MappingService
    {
        [ExcludeFromCodeCoverage]
        public void MapTripAttachment()
        {
            CreateMap<TripAttachment, TripAttachmentDto>()
                .ForMember(des => des.FileName, opt => opt.MapFrom(src => src.Attachment.FileName))
                .ForMember(des => des.Extension, opt => opt.MapFrom(src => src.Attachment.Extension))
                .ForMember(des => des.Size, opt => opt.MapFrom(src => src.Attachment.Size))
                .ForMember(des => des.Url, opt => opt.MapFrom(src => src.Attachment.Url))
                .ForMember(des => des.IsPublic, opt => opt.MapFrom(src => src.Attachment.IsPublic))
                .ForMember(des => des.TripInformationCode, opt => opt.MapFrom(src => src.TripInformation.Code))
                .ReverseMap();

            CreateMap<TripAttachment, EditTripAttachmentDto>()
                .ForMember(des => des.FileName, opt => opt.MapFrom(src => src.Attachment.FileName))
                .ForMember(des => des.Extension, opt => opt.MapFrom(src => src.Attachment.Extension))
                .ForMember(des => des.Size, opt => opt.MapFrom(src => src.Attachment.Size))
                .ForMember(des => des.Url, opt => opt.MapFrom(src => src.Attachment.Url))
                .ForMember(des => des.IsPublic, opt => opt.MapFrom(src => src.Attachment.IsPublic))
                .ForMember(des => des.TripInformationCode, opt => opt.MapFrom(src => src.TripInformation.Code))
                .ReverseMap();

            CreateMap<AddTripAttachmentDto, TripAttachment>()
                .ForMember(des => des.Id, opt => opt.Ignore())
                .ReverseMap();
        }
    }
}
