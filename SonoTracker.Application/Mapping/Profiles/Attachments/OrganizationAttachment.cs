using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SonoTracker.Common.DTO.Attachments.OrganizationAttachment;
using SonoTracker.Domain.Entities.Attachments;

namespace SonoTracker.Application.Mapping
{
    public partial class MappingService
    {
        public void MapOrganizationAttachment()
        {
            CreateMap<OrganizationAttachment, OrganizationAttachmentDto>().ReverseMap();
            CreateMap<OrganizationAttachment, EditOrganizationAttachmentDto>().ReverseMap();
            CreateMap<OrganizationAttachment, AddOrganizationAttachmentDto>().ReverseMap();
        }
    }
}
