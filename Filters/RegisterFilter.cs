using Lombiq.Antispam.Constants;
using Orchard.ContentManagement;
using Orchard.Environment.Extensions;
using Orchard.Localization;
using Orchard.Mvc.Filters;
using Orchard.Security;
using Orchard.Users.Events;
using System.Web.Mvc;

namespace Lombiq.Antispam.Filters
{
    [OrchardFeature("Lombiq.Antispam.Registration")]
    public class RegisterFilter : FilterProvider, IActionFilter, IUserEventHandler
    {
        private readonly IContentManager _contentManager;
        private bool _modelIsValid;


        public RegisterFilter(IContentManager contentManager)
        {
            _contentManager = contentManager;

            _modelIsValid = true;
        }


        public void OnActionExecuted(ActionExecutedContext filterContext) { }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!(filterContext.RouteData.Values["action"].ToString() == "Register" && filterContext.RouteData.Values["controller"].ToString() == "Account" && filterContext.RouteData.Values["area"].ToString() == "Orchard.Users")) return;

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
                    _modelIsValid = false;
                }
            }

            filterContext.Controller.ViewData[ContentTypes.RegistrationSpamProtector] = _contentManager.BuildEditor(_contentManager.New(ContentTypes.RegistrationSpamProtector));
        }

        public void Creating(UserContext context)
        {
            if (!_modelIsValid)
            {
                context.Cancel = true;
            }
        }

        public void Created(UserContext context) { }

        public void LoggedIn(IUser user) { }

        public void LoggedOut(IUser user) { }

        public void AccessDenied(IUser user) { }

        public void ChangedPassword(IUser user) { }

        public void SentChallengeEmail(IUser user) { }

        public void ConfirmedEmail(IUser user) { }

        public void Approved(IUser user) { }
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