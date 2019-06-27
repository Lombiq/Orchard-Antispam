namespace Lombiq.Antispam.Models
{
    public class ReCaptchaTokenVerificationRequest
    {
        public string SecretKey { get; set; }
        public string ResponseToken { get; set; }
        public string RemoteIp { get; set; }
    }
}