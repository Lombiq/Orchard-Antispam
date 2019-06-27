using Orchard.Environment.Extensions;
using System;

namespace Lombiq.Antispam.Services
{
    [OrchardFeature("Lombiq.Antispam.ReCaptcha")]
    public class ReCaptchaService : IReCaptchaService
    {
        public bool VerifyToken(string token, string action)
        {
            return false;
        }
    }
}