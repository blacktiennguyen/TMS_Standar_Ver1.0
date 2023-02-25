using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMS.WebAPP.Framework.Mvc
{
    public class ActionConfirmationModel
    {
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public string WindowId { get; set; }
    }
}