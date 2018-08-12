using FindMyPet.DTO.Admin;
using FindMyPet.MVC.Models.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FindMyPet.MVC.Mappers
{
    public interface IAdminMapper
    {
        CommentToApproveViewModel AdminFoundAlertToViewModel(AdminFoundAlert adminFoundAlert);
        AlertToApproveViewModel AdminReportedAlertToViewModel(AdminReportedAlert adminReportedAlert);
    }

    public class AdminMapper : IAdminMapper
    {
        public CommentToApproveViewModel AdminFoundAlertToViewModel(AdminFoundAlert adminFoundAlert)
        {
            return new CommentToApproveViewModel
            {
                Id = adminFoundAlert.Id,
                Code = adminFoundAlert.Code.ToString(),
                OwnerName = HttpUtility.HtmlDecode(adminFoundAlert.OwnerFullName),
                ownerProfileImageUrl = adminFoundAlert.OwnerProfileImageUrl,
                PetName = HttpUtility.HtmlDecode(adminFoundAlert.PetName),
                PetProfileImageUrl = adminFoundAlert.PetProfileImageUrl,
                FoundComment = HttpUtility.HtmlDecode(adminFoundAlert.FoundComment),
                FoundDateTime = adminFoundAlert.FoundDateTime.ToString("dd/MMM/yyyy hh:mm:ss tt")
            };
        }

        public AlertToApproveViewModel AdminReportedAlertToViewModel(AdminReportedAlert adminReportedAlert)
        {
            return new AlertToApproveViewModel
            {
                Id = adminReportedAlert.Id,
                Code = adminReportedAlert.Code.ToString(),
                OwnerName = adminReportedAlert.OwnerFullName,
                ownerProfileImageUrl = adminReportedAlert.OwnerProfileImageUrl,
                PetName = adminReportedAlert.PetName,
                PetProfileImageUrl = adminReportedAlert.PetProfileImageUrl,
                Comment = adminReportedAlert.Comment,
                CreateOn = adminReportedAlert.CreateOn.ToString("dd/MMM/yyyy hh:mm:ss tt"),
                Images = adminReportedAlert.Images
            };
        }
    }
}