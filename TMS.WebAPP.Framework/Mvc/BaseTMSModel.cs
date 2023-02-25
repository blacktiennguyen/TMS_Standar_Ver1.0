using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace TMS.WebAPP.Framework.Mvc
{
    /// <summary>
    /// Base TMS model
    /// </summary>
    [ModelBinder(typeof(TMSModelBinder))]
    public partial class BaseTMSModel
    {
        public BaseTMSModel()
        {
            this.CustomProperties = new Dictionary<string, object>();
            PostInitialize();
        }

        public virtual void BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
        }

        /// <summary>
        /// Developers can override this method in custom partial classes
        /// in order to add some custom initialization code to constructors
        /// </summary>
        protected virtual void PostInitialize()
        {
        }

        /// <summary>
        /// Use this property to store any custom value for your models.
        /// </summary>
        public Dictionary<string, object> CustomProperties { get; set; }
    }

    /// <summary>
    /// Base TMS entity model
    /// </summary>
    public partial class BaseTMSEntityModel : BaseTMSModel
    {
        public virtual int Id { get; set; }
    }
}