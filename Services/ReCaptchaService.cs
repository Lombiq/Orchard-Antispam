using Lombiq.Antispam.Constants;
using Lombiq.Antispam.Models;
using Orchard.AntiSpam.Models;
using Orchard.ContentManagement;
using Orchard.Environment.Extensions;
using Orchard.Exceptions;
using Orchard.Logging;
using Orchard.Mvc;
using Orchard.Settings;
using RestEase;
using System;
using System.Linq;

namespace Lombiq.Antispam.Services
{
    [OrchardFeature(FeatureNames.Lombiq_Antispam_ReCaptcha)]
    public class ReCaptchaService : IReCaptchaService
    {
        private const string ReCaptchaResponseTokenVerificationUrl = "https://www.google.com/recaptcha/api/siteverify";
        private const double ScoreThreshold = 0.5;

        private readonly ISiteService _siteService;
        private readonly IHttpContextAccessor _hca;


        public ILogger Logger { get; set; }


        public ReCaptchaService(
            ISiteService siteService, 
            IHttpContextAccessor hca)
        {
            _siteService = siteService;
            _hca = hca;

            Logger = NullLogger.Instance;
        }


        public ReCaptchaTokenVerificationResult VerifyResponseToken(string token, string action)
        {
            try
            {
                var client = RestClient.For<IReCaptchaResponseTokenVerificationApiClient>(
                    ReCaptchaResponseTokenVerificationUrl);

                var response = client.VerifyTokenAsync(new ReCaptchaTokenVerificationRequest
                {
                    SecretKey = _siteService.GetSiteSettings().As<ReCaptchaSettingsPart>().PrivateKey,
                    ResponseToken = token,
                    RemoteIp = _hca.Current().Request.ServerVariables["REMOTE_ADDR"]
                }).Result;

                if (response.ErrorCodes?.Any() ?? false)
                {
                    Logger.Error($"Failed to verify reCAPTCHA v3 response token. API returned the following error codes: {string.Join(", ", response.ErrorCodes)}");
                }

                return new ReCaptchaTokenVerificationResult
                {
                    IsVerified = response.Success &&
                        response.Action == action &&
                        response.Score >= ScoreThreshold,
                    IsFailed = !response.Success,
                    Response = response
                };
            }
            catch (Exception ex)
            {
                if (ex.IsFatal()) throw;

                Logger.Error(ex, "Failed to verify reCAPTCHA v3 response token.");

                return new ReCaptchaTokenVerificationResult
                {
                    IsFailed = true
                };
            }
        }
    }
}