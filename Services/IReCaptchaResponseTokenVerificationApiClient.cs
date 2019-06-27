using Lombiq.Antispam.Models;
using RestEase;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lombiq.Antispam.Services
{
    public interface IReCaptchaResponseTokenVerificationApiClient
    {
        [Post]
        Task<ReCaptchaTokenVerificationResponse> VerifyTokenAsync(
            [Body(BodySerializationMethod.UrlEncoded)] IDictionary<string, object> requestData);
    }


    public static class ReCaptchaResponseTokenVerificationApiClientExtensions
    {
        public static Task<ReCaptchaTokenVerificationResponse> VerifyTokenAsync(
            this IReCaptchaResponseTokenVerificationApiClient client,
            ReCaptchaTokenVerificationRequest request) =>
            client.VerifyTokenAsync(new Dictionary<string, object>
            {
                ["secret"] = request.SecretKey,
                ["response"] = request.ResponseToken,
                ["remoteip"] = request.RemoteIp
            });
    }
}