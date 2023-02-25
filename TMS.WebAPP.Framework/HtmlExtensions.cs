using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using TMS.WebAPP.Framework.Mvc;

namespace TMS.WebAPP.Framework
{
    public static class HtmlExtensions
    {
        public static MvcHtmlString DeleteConfirmation<T>(this HtmlHelper<T> helper, string buttonsSelector) where T : BaseTMSEntityModel
        {
            return DeleteConfirmation(helper, "", buttonsSelector);
        }

        public static MvcHtmlString DeleteConfirmation<T>(this HtmlHelper<T> helper, string actionName,
            string buttonsSelector) where T : BaseTMSEntityModel
        {
            if (String.IsNullOrEmpty(actionName))
                actionName = "Delete";

            var modalId = MvcHtmlString.Create(helper.ViewData.ModelMetadata.ModelType.Name.ToLower() + "-delete-confirmation")
                .ToHtmlString();

            var deleteConfirmationModel = new DeleteConfirmationModel
            {
                Id = helper.ViewData.Model.Id,
                ControllerName = helper.ViewContext.RouteData.GetRequiredString("controller"),
                ActionName = actionName,
                WindowId = modalId
            };

            var window = new StringBuilder();
            window.AppendLine(string.Format("<div id='{0}' class=\"modal fade\"  tabindex=\"-1\" role=\"dialog\" aria-labelledby=\"{0}-title\">", modalId));
            window.AppendLine(helper.Partial("Delete", deleteConfirmationModel).ToHtmlString());
            window.AppendLine("</div>");

            window.AppendLine("<script>");
            window.AppendLine("$(document).ready(function() {");
            window.AppendLine(string.Format("$('#{0}').attr(\"data-toggle\", \"modal\").attr(\"data-target\", \"#{1}\")", buttonsSelector, modalId));
            window.AppendLine("});");
            window.AppendLine("</script>");

            return MvcHtmlString.Create(window.ToString());
        }

        public static MvcHtmlString ActionConfirmation(this HtmlHelper helper, string buttonId, string actionName = "")
        {
            if (string.IsNullOrEmpty(actionName))
                actionName = helper.ViewContext.RouteData.GetRequiredString("action");

            var modalId = MvcHtmlString.Create(buttonId + "-action-confirmation").ToHtmlString();

            var actionConfirmationModel = new ActionConfirmationModel()
            {
                ControllerName = helper.ViewContext.RouteData.GetRequiredString("controller"),
                ActionName = actionName,
                WindowId = modalId
            };

            var window = new StringBuilder();
            window.AppendLine(string.Format("<div id='{0}' class=\"modal fade\"  tabindex=\"-1\" role=\"dialog\" aria-labelledby=\"{0}-title\">", modalId));
            window.AppendLine(helper.Partial("Confirm", actionConfirmationModel).ToHtmlString());
            window.AppendLine("</div>");

            window.AppendLine("<script>");
            window.AppendLine("$(document).ready(function() {");
            window.AppendLine(string.Format("$('#{0}').attr(\"data-toggle\", \"modal\").attr(\"data-target\", \"#{1}\");", buttonId, modalId));
            window.AppendLine(string.Format("$('#{0}-submit-button').attr(\"name\", $(\"#{1}\").attr(\"name\"));", modalId, buttonId));
            window.AppendLine(string.Format("$(\"#{0}\").attr(\"name\", \"\")", buttonId));
            window.AppendLine(string.Format("if($(\"#{0}\").attr(\"type\") == \"submit\")$(\"#{0}\").attr(\"type\", \"button\")", buttonId));
            window.AppendLine("});");
            window.AppendLine("</script>");

            return MvcHtmlString.Create(window.ToString());
        }
    }
}