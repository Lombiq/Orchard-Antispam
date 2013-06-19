using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.ContentManagement;

namespace Lombiq.Antispam.Models
{
    public class JavaScriptAntispamPart : ContentPart
    {
        public bool IAmHuman { get; set; }
    }
}