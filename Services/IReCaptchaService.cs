using Orchard;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lombiq.Antispam.Services
{
    public interface IReCaptchaService : IDependency
    {
        bool VerifyToken(string token, string action);
    }
}