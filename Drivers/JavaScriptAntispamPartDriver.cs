using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lombiq.Antispam.Models;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.Localization;

namespace Lombiq.Antispam.Drivers
{
    public class JavaScriptAntispamPartDriver : ContentPartDriver<JavaScriptAntispamPart>
    {
        protected override string Prefix
        {
            get { return "Lombiq.Antispam.JavaScriptAntispamPart"; }
        }

        public Localizer T { get; set; }


        public JavaScriptAntispamPartDriver()
        {
            T = NullLocalizer.Instance;
        }


        protected override DriverResult Editor(JavaScriptAntispamPart part, dynamic shapeHelper)
        {
            return ContentShape("Parts_JavaScriptAntispam_Edit",
                () => shapeHelper.EditorTemplate(
                    TemplateName: "Parts.JavaScriptAntispam",
                    Model: part,
                    Prefix: Prefix));
        }

        protected override DriverResult Editor(JavaScriptAntispamPart part, IUpdateModel updater, dynamic shapeHelper)
        {
            updater.TryUpdateModel(part, Prefix, null, null);

            if (!part.IAmHuman)
            {
                updater.AddModelError("NotHuman", T("You are either a bot (we only serve humans, sorry) or have JavaScript turned off. If the latter, please turn on JavaScript to post this form."));
            }

            return Editor(part, shapeHelper);
        }
    }
}