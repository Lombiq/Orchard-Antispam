using Lombiq.Antispam.Services;
using Orchard;
using Orchard.AntiSpam.Models;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.Environment.Extensions;
using Orchard.Localization;
using Orchard.Logging;
using Orchard.Mvc;
using Orchard.Security;
using Orchard.Settings;
using Orchard.UI.Admin;
using Orchard.UI.Notify;
using Orchard.Utility.Extensions;
using System;

namespace Lombiq.Antispam.Drivers
{
    [OrchardSuppressDependency("Orchard.AntiSpam.Drivers.ReCaptchaPartDriver")]
    [OrchardFeature("Lombiq.Antispam.ReCaptcha")]
    public class ReCaptchaPartDriver : ContentPartDriver<ReCaptchaPart>
    {
        private readonly INotifier _notifier;
        private readonly IWorkContextAccessor _wca;
        private readonly ISiteService _siteService;
        private readonly IHttpContextAccessor _hca;
        private readonly IReCaptchaService _reCaptchaService;
        private readonly Lazy<IUser> _currentUserLazy;
        private readonly Lazy<ReCaptchaSettingsPart> _reCaptchaSettingsPartLazy;


        public Localizer T { get; set; }
        public ILogger Logger { get; set; }


        public ReCaptchaPartDriver(
            INotifier notifier,
            IWorkContextAccessor wca,
            ISiteService siteService,
            IHttpContextAccessor hca,
            IReCaptchaService reCaptchaService)
        {
            _notifier = notifier;
            _wca = wca;
            _siteService = siteService;
            _hca = hca;
            _reCaptchaService = reCaptchaService;

            _currentUserLazy = new Lazy<IUser>(() => _wca.GetContext().CurrentUser);
            _reCaptchaSettingsPartLazy = new Lazy<ReCaptchaSettingsPart>(() =>
                _siteService.GetSiteSettings().As<ReCaptchaSettingsPart>());

            T = NullLocalizer.Instance;
            Logger = NullLogger.Instance;
        }


        protected override DriverResult Editor(ReCaptchaPart part, dynamic shapeHelper)
        {
            if (IsAdminFilterApplied()) return null;

            return ContentShape("Parts_ReCaptcha_Edit", () =>
            {
                if (!ShouldProtectFormWithReCaptcha()) return null;

                return shapeHelper.EditorTemplate(
                    TemplateName: "Parts.ReCaptcha.Fields",
                    Model: shapeHelper.ViewModel(
                        SiteKey: _reCaptchaSettingsPartLazy.Value.PublicKey,
                        Action: part.ContentItem.ContentType,
                        FieldName: GetFieldName(part)),
                    Prefix: Prefix);
            });
        }

        protected override DriverResult Editor(ReCaptchaPart part, IUpdateModel updater, dynamic shapeHelper)
        {
            if (IsAdminFilterApplied() || !ShouldProtectFormWithReCaptcha()) return null;

            var reCaptchaToken = _hca.Current().Request[GetFieldName(part)];

            if (string.IsNullOrEmpty(reCaptchaToken))
            {
                Logger.Error("reCAPTCHA v3 token is empty.");

                updater.AddModelError("", T("An error occurred while validating reCaptcha."));

                return Editor(part, shapeHelper);
            }
            
            if (!_reCaptchaService.VerifyToken(reCaptchaToken, part.ContentItem.ContentType))
            {
                var suspiciousResultText = T("reCAPTCHA verification indicated suspicious activity.");

                _notifier.Error(suspiciousResultText);

                updater.AddModelError("", suspiciousResultText);
            }

            return Editor(part, shapeHelper);
        }


        private bool IsAdminFilterApplied() =>
            AdminFilter.IsApplied(_hca.Current().Request.RequestContext);

        private bool ShouldProtectFormWithReCaptcha() =>
            _currentUserLazy.Value == null || !_reCaptchaSettingsPartLazy.Value.TrustAuthenticatedUsers;

        private string GetFieldName(ReCaptchaPart part) =>
            $"ReCaptchaToken-{part.ContentItem.ContentType}".HtmlClassify();
    }
}