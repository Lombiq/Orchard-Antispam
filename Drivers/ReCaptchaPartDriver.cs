using Lombiq.Antispam.Constants;
using Lombiq.Antispam.Services;
using Orchard;
using Orchard.AntiSpam.Models;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.Environment.Extensions;
using Orchard.Localization;
using Orchard.Mvc;
using Orchard.Security;
using Orchard.Settings;
using Orchard.UI.Admin;
using Orchard.UI.Notify;
using System;

namespace Lombiq.Antispam.Drivers
{
    [OrchardSuppressDependency("Orchard.AntiSpam.Drivers.ReCaptchaPartDriver")]
    [OrchardFeature(FeatureNames.Lombiq_Antispam_ReCaptcha)]
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
                updater.AddModelError("", T("reCAPTCHA v3 response token is unaccessible."));

                return Editor(part, shapeHelper);
            }

            var verificationResult = _reCaptchaService.VerifyResponseToken(reCaptchaToken, part.ContentItem.ContentType);

            if (verificationResult.IsFailed)
            {
                DisplayErrorMessageAndAddModelError(
                    updater,
                    T("reCAPTCHA v3 verification has unexpectedly failed."));
            }
            else if (!verificationResult.IsVerified)
            {
                DisplayErrorMessageAndAddModelError(
                    updater, 
                    T("reCAPTCHA v3 verification has indicated suspicious activity."));
            }

            return Editor(part, shapeHelper);
        }


        private bool IsAdminFilterApplied() =>
            AdminFilter.IsApplied(_hca.Current().Request.RequestContext);

        private bool ShouldProtectFormWithReCaptcha() =>
            _currentUserLazy.Value == null || !_reCaptchaSettingsPartLazy.Value.TrustAuthenticatedUsers;

        private string GetFieldName(ReCaptchaPart part) =>
            $"{part.ContentItem.ContentType}.ReCaptchaResponseToken";

        private void DisplayErrorMessageAndAddModelError(IUpdateModel updater, LocalizedString text)
        {
            _notifier.Error(text);

            updater.AddModelError("", text);
        }
    }
}