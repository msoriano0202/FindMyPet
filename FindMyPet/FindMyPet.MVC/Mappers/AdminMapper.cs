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
    }

    public class AdminMapper : IAdminMapper
    {
        public CommentToApproveViewModel AdminFoundAlertToViewModel(AdminFoundAlert adminFoundAlert)
        {
            return new CommentToApproveViewModel
            {
                Id = adminFoundAlert.Id,
                Code = adminFoundAlert.Code.ToString(),
                OwnerName = adminFoundAlert.OwnerFullName,
                ownerProfileImageUrl = adminFoundAlert.OwnerProfileImageUrl,
                PetName = adminFoundAlert.PetName,
                PetProfileImageUrl = adminFoundAlert.PetProfileImageUrl,
                FoundComment = adminFoundAlert.FoundComment,
                FoundDateTime = adminFoundAlert.FoundDateTime.ToString("dd/MMM/yyyy hh:mm:ss tt")
            };
        }
    }
}