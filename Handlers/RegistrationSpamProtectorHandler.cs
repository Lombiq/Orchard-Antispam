using Orchard.ContentManagement.Handlers;
using Orchard.Environment.Extensions;

namespace Lombiq.Antispam.Handlers
{
    [OrchardFeature("Lombiq.Antispam.Registration")]
    public class RegistrationSpamProtectorHandler : ContentHandler
    {
        protected override void Created(CreateContentContext context)
        {
            if (!context.ContentItem.ContentType.Equals("User")) return;
        }
    }
}