using Orchard;
using Orchard.Environment.Extensions;
using Orchard.Users.Events;

namespace Lombiq.Antispam.Handlers
{
    [OrchardFeature("Lombiq.Antispam.Registration")]
    public class RegistrationSpamProtectorHandler : IUserEventHandler
    {
        private readonly IWorkContextAccessor _wca;
        public RegistrationSpamProtectorHandler(IWorkContextAccessor wca)
        {
            _wca = wca;
        }
        public void Creating(UserContext context)
        {
            if (true)
            {
                context.Cancel = true;
            }
        }

        public void Created(UserContext context)
        {
           
        }

        public void LoggedIn(Orchard.Security.IUser user)
        {

        }

        public void LoggedOut(Orchard.Security.IUser user)
        {

        }

        public void AccessDenied(Orchard.Security.IUser user)
        {

        }

        public void ChangedPassword(Orchard.Security.IUser user)
        {

        }

        public void SentChallengeEmail(Orchard.Security.IUser user)
        {

        }

        public void ConfirmedEmail(Orchard.Security.IUser user)
        {

        }

        public void Approved(Orchard.Security.IUser user)
        {

        }
    }
}