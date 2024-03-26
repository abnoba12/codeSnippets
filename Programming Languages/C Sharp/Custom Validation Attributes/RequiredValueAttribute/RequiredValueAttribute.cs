using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace StonebridgeJCP.Validation
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = false)]
    public class RequiredValueAttribute : ValidationAttribute, IClientValidatable
    {
        public object expectedValue = null;
        
        /// <summary>
        /// Checkes to see if the current fields value matches the one passed
        /// </summary>
        /// <param name="expectedValue">The the field is expected to be</param>
        public RequiredValueAttribute(object expectedValue)            
        {
            this.expectedValue = expectedValue;
        }

        public override bool IsValid(object value)
        {
            if (value == null || !value.Equals(this.expectedValue))
            {
                return false;
            }
            return true;
        }

        public virtual IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule
            {
                ErrorMessage = FormatErrorMessage(metadata.GetDisplayName()),
                ValidationType = "requiredvalue",
            };

            // find the value on the control we depend on;
            // if it's a bool, format it javascript style
            // (the default is True or False!)
            string targetValue = (expectedValue ?? "").ToString();
            if (expectedValue is bool)
                targetValue = targetValue.ToLower();

            rule.ValidationParameters.Add("targetvalue", targetValue);

            yield return rule;
        }
    }
}