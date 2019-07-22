using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Lombiq.Antispam.Models
{
    public class ReCaptchaTokenVerificationResponse
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("score")]
        public double Score { get; set; }

        [JsonProperty("action")]
        public string Action { get; set; }

        [JsonProperty("challenge_ts")]
        public DateTime ChallengeTime { get; set; }

        [JsonProperty("hostname")]
        public string Hostname { get; set; }

        [JsonProperty("error-codes")]
        public IEnumerable<string> ErrorCodes { get; set; }
    }
}