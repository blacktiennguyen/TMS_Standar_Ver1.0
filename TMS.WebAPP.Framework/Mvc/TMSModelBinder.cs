using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace TMS.WebAPP.Framework.Mvc
{
    public class TMSModelBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var model = base.BindModel(controllerContext, bindingContext);
            if (model is BaseTMSModel)
            {
                ((BaseTMSModel)model).BindModel(controllerContext, bindingContext);
            }
            return model;
        }

        protected override void SetProperty(ControllerContext controllerContext, ModelBindingContext bindingContext,
            PropertyDescriptor propertyDescriptor, object value)
        {
            //check if data type of value is System.String
            if (propertyDescriptor.PropertyType == typeof(string))
            {
                //developers can mark properties to be excluded from trimming with [NoTrim] attribute
                if (propertyDescriptor.Attributes.Cast<object>().All(a => a.GetType() != typeof(NoTrimAttribute)))
                {
                    var stringValue = (string)value;
                    value = string.IsNullOrEmpty(stringValue) ? stringValue : stringValue.Trim();
                }
            }

            base.SetProperty(controllerContext, bindingContext, propertyDescriptor, value);
        }
    }
}