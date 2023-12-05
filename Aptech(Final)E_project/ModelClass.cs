using Aptech_Final_E_project.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Aptech_Final_E_project
{
    public class ModelClass
    {
        public IEnumerable<Recipes> Receipes { get; set; }
        public IEnumerable<Announcement> Announcements { get; set; }
        public bool UserHasActivesubscription { get; set; }
        public IEnumerable<FAQ>FAQs  { get; set; }
        public IEnumerable<Tips> tips  { get; set; }
    }
}
