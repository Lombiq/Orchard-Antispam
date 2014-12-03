using Lombiq.Antispam.Constants;
using Orchard.ContentManagement;
using Orchard.Environment.Extensions;
using Orchard.Localization;
using Orchard.Mvc.Filters;
using Orchard.Users.Events;
using System.Web.Mvc;

namespace Lombiq.Antispam.Filters
{
    [OrchardFeature("Lombiq.Antispam.Registration")]
    public class RegisterFilter : FilterProvider, IResultFilter, IUserEventHandler
    {
        private readonly IContentManager _contentManager;
        private bool _modelStateIsValid;


        public RegisterFilter(IContentManager contentManager)
        {
            _contentManager = contentManager;

            _modelStateIsValid = true;
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
                        _modelStateIsValid = false;
                    }
                }

                filterContext.Controller.ViewData[ContentTypes.RegistrationSpamProtector] = _contentManager.BuildEditor(_contentManager.New(ContentTypes.RegistrationSpamProtector));
            }
        }

        public void Creating(UserContext context)
        {
            if (!_modelStateIsValid)
            {
                context.Cancel = true;
            }
        }

        public void Created(UserContext context)
        {

        }

        public void LoggedIn(Orchard.Security.IUser user)
        {

        }

        public void LoggedOut(Orchard.Security.IUser user)
        {

        }

        public void AccessDenied(Orchard.Security.IUser user)
        {

        }

        public void ChangedPassword(Orchard.Security.IUser user)
        {

        }

        public void SentChallengeEmail(Orchard.Security.IUser user)
        {

        }

        public void ConfirmedEmail(Orchard.Security.IUser user)
        {

        }

        public void Approved(Orchard.Security.IUser user)
        {

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