namespace Lombiq.Antispam.Models
{
    public class ReCaptchaTokenVerificationResult
    {
        public bool IsVerified { get; set; }
        public bool IsFailed { get; set; }
        public ReCaptchaTokenVerificationResponse Response { get; set; }
    }
}