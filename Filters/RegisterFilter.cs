using Lombiq.Antispam.Constants;
using Orchard.ContentManagement;
using Orchard.Environment.Extensions;
using Orchard.Localization;
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
            if (filterContext.RouteData.Values["action"].ToString() == "Register" && filterContext.RouteData.Values["controller"].ToString() == "Account" && filterContext.RouteData.Values["area"].ToString() == "Orchard.Users")
            {
                if (filterContext.HttpContext.Request.HttpMethod.ToString().Equals("POST"))
                {
                    var filterContextController = filterContext.Controller;
                    var updaterController = new UpdaterController();
                    updaterController.ControllerContext = filterContextController.ControllerContext;
                    updaterController.ValueProvider = filterContextController.ValueProvider;

                    var antispampart = _contentManager.New(ContentTypes.RegistrationSpamProtector);

                    _contentManager.UpdateEditor(antispampart, updaterController);

                    if (!updaterController.ModelState.IsValid)
                    {

                    }
                }

                filterContext.Controller.ViewData[ContentTypes.RegistrationSpamProtector] = _contentManager.BuildEditor(_contentManager.New(ContentTypes.RegistrationSpamProtector));
            }
        }
    }


    [OrchardFeature("Lombiq.Antispam.Registration")]
    public class UpdaterController : Controller, IUpdateModel
    {
        #region IUpdateModel Members

        bool IUpdateModel.TryUpdateModel<TModel>(TModel model, string prefix, string[] includeProperties, string[] excludeProperties)
        {
            return TryUpdateModel(model, prefix, includeProperties, excludeProperties);
        }

        void IUpdateModel.AddModelError(string key, LocalizedString errorMessage)
        {
            ModelState.AddModelError(key, errorMessage.ToString());
        }

        #endregion
    }
}