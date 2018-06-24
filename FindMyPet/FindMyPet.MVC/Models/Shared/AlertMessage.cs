using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FindMyPet.MVC.Models.Shared
{
    public enum AlertMessageTypeEnum
    {
        Success = 1,
        Error = 2,
        Info = 3
    }

    public class AlertMessage
    {
        public AlertMessageTypeEnum Type { get; set; }
        public string Message { get; set; }
    }
}