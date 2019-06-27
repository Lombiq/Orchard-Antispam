using Lombiq.Antispam.Models;
using Orchard;

namespace Lombiq.Antispam.Services
{
    public interface IReCaptchaService : IDependency
    {
        ReCaptchaTokenVerificationResult VerifyResponseToken(string token, string action);
    }
}