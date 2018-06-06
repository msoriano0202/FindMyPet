using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FindMyPet.MVC.Helpers
{
    public static class CustomHtmlHelper
    {
        //public static MvcHtmlString CustomToggleCheckbox(this HtmlHelper helper, string id, bool? isChecked = false, bool? isDisabled = false)
        //{
        //    var checkedStr = isChecked.HasValue && isChecked.Value ? "checked" : string.Empty;
        //    var disabledStr = isDisabled.HasValue && isDisabled.Value ? "disabled" : string.Empty;

        //    //var result = $"<input id = \"{id}\" name = \"{id}\" type = \"checkbox\" {checkedStr} {disabledStr}>";
        //    var result = string.Format("<input id = \"{0}\" name = \"{0}\" type = \"checkbox\" {1} {2} data-toggle=\"toggle\">", id, checkedStr, disabledStr);
        //    return new MvcHtmlString(result);
        //}
    }
}