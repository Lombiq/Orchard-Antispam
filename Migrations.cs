using Lombiq.Antispam.Constants;
using Orchard.ContentManagement.MetaData;
using Orchard.Data.Migration;
using Orchard.Environment.Extensions;

namespace Lombiq.Antispam
{
    [OrchardFeature("Lombiq.Antispam.Registration")]
    public class Migrations : DataMigrationImpl
    {
        public int Create()
        {
            ContentDefinitionManager.AlterTypeDefinition(ContentTypes.RegistrationSpamProtector,
                cfg => cfg
                    .DisplayedAs("Registration Spam Protector"));

            return 1;
        }
    }
}