using Orchard.ContentManagement;
using Orchard.Environment.Extensions;
using Orchard.Mvc.Filters;
using System.Web.Mvc;

namespace Lombiq.Antispam.Filters
{
    [OrchardFeature("Lombiq.Antispam.Registration")]
    public class RegisterFilter : FilterProvider, IResultFilter
    {
        private readonly IContentManager _contentManager;


        public RegisterFilter(IContentManager contentManager)
        {
            _contentManager = contentManager;
        }


        public void OnResultExecuted(ResultExecutedContext filterContext) { }

        public void OnResultExecuting(ResultExecutingContext filterContext)
        {
            if (filterContext.RouteData.Values["action"].ToString() == "Register")
            {
                filterContext.Controller.ViewData["RegistrationSpamProtector"] = _contentManager.BuildEditor(_contentManager.New("RegistrationSpamProtector"));
            }
        }
    }
}