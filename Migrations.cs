using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lombiq.Antispam.Models;
using Orchard.Data.Migration;
using Orchard.ContentManagement.MetaData;
using Orchard.Core.Contents.Extensions;

namespace Lombiq.Antispam
{
    public class Migrations : DataMigrationImpl
    {
        public int Create()
        {
            ContentDefinitionManager.AlterPartDefinition(typeof(JavaScriptAntispamPart).Name,
                builder => builder
                    .Attachable()
                    .WithDescription("Prevents spambots to post the editor by requiring JavaScript support."));

            return 1;
        }
    }
}