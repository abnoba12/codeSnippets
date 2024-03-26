using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.Collections;

namespace CorporateAdminV2.Validation.RequiredIfAttribute
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public class RequiredIfAttribute : ValidationAttribute, IClientValidatable
    {
        protected RequiredAttribute _innerAttribute;
        private MvcApplication MvcApplication = new MvcApplication();

        public object DependentProperty { get; set; }
        public String[] DependentPropertyArray = null;
        public object TargetValue { get; set; }
        public object[] TargetValueArray = null;
        public object expectedValueOfField { get; set; }

        //To avoid multiple rules with same name
        public static Dictionary<string, int> countPerField = null;
        //Required if you want to use this attribute multiple times
        private object _typeId = new object();
        public override object TypeId
        {
            get { return _typeId; }
        }

        public bool AllowEmptyStrings
        {
            get
            {
                return _innerAttribute.AllowEmptyStrings;
            }
            set
            {
                _innerAttribute.AllowEmptyStrings = value;
            }
        }

        /// <summary>
        /// This takes the name of another field in the model and that fields expected value. 
        /// If the dependentProperty equals the targetValue then return valid.
        /// </summary>
        /// <param name="dependentProperty">The name of the field to check</param>
        /// <param name="targetValue">The expected value of the passed in field</param>
        public RequiredIfAttribute(object dependentProperty, object targetValue)
        {
            _innerAttribute = new RequiredAttribute();
            this.DependentProperty = dependentProperty;
            this.TargetValue = targetValue;
            //If we have just a string then add it to an array. Otherwise if our object is already
            //an array then cast the object as an array            
            if (DependentProperty.GetType().IsArray)
            {
                DependentPropertyArray = ((IEnumerable)DependentProperty).Cast<object>().Select(x => x.ToString()).ToArray();
            }
            else if (DependentProperty.GetType().ToString() == "System.String")
            {
                String DependentPropertyString = (String)DependentProperty;
                DependentPropertyArray = new String[1] { DependentPropertyString };
            }
            if (TargetValue.GetType().IsArray)
            {
                TargetValueArray = ((IEnumerable)TargetValue).Cast<object>().Select(x => x.ToString()).ToArray();
            }
            else
            {
                TargetValueArray = new object[1] { TargetValue };
            }
        }

        /// <summary>
        /// This takes the name of another field in the model and that fields expected value. 
        /// You can also pass in what the expected value of the current field should be.
        /// If the dependentProperty equals the targetValue and the expected value equals the fields value
        /// then return true.
        /// </summary>
        /// <param name="dependentProperty">The name of the field to check</param>
        /// <param name="targetValue">The expected value of the passed in field</param>
        /// <param name="expectedValueOfCurrentField">The expected value of the current field. Not the target field</param>
        public RequiredIfAttribute(object dependentProperty, object targetValue, object expectedValueOfCurrentField) : this(dependentProperty, targetValue)
        {
            this.expectedValueOfField = expectedValueOfCurrentField;
        }

        protected override ValidationResult IsValid(object valueOfField, ValidationContext validationContext)
        {
            //Make sure we have the Dependent Property Array and the Target Value Array sizes match
            //DependentPropertyArray != null && TargetValueArray != null && 
            if (DependentPropertyArray.Length > 0 && DependentPropertyArray.Length == TargetValueArray.Length)
            {
                //Check each Dependent Property and it's target value to see if this field is required
                bool required = true;
                for (int x = 0; x < DependentPropertyArray.Length; x++ )
                {                    
                    bool negateTargetValue = false;
                    var TargetValue = TargetValueArray[x];

                    // get a reference to the property this validation depends upon
                    var containerType = validationContext.ObjectInstance.GetType();
                    var field = containerType.GetProperty(DependentPropertyArray[x]);

                    if (field != null)
                    {
                        // get the value of the dependent property
                        var dependentValue = field.GetValue(validationContext.ObjectInstance, null);
                        // trim spaces of dependent value
                        if (dependentValue != null)
                        {
                            dependentValue = dependentValue.ToString().Trim();

                            if (!AllowEmptyStrings && (dependentValue as string).Length == 0)
                            {
                                dependentValue = null;
                            }
                        }                        

                        //Allow for a 'NOT' condition
                        if (TargetValue.GetType() == typeof(String))
                        {
                            string TargetValueString = (string)TargetValue;
                            if(String.IsNullOrEmpty(TargetValueString))
                                TargetValue = null;

                            if (!String.IsNullOrEmpty(TargetValueString) && TargetValueString[0] == '!')
                            {
                                negateTargetValue = true;
                                TargetValue = TargetValueString.Substring(1);
                            }
                        }

                        //Don't validate unless this field is currently required
                        if ((dependentValue == null && TargetValue == null) ||
                                (dependentValue != null &&
                                    ((TargetValue != null && TargetValue.Equals("*")) ||
                                        (!negateTargetValue && dependentValue.Equals(TargetValue)) ||
                                        (negateTargetValue && !dependentValue.Equals(TargetValue))
                                    )
                                )
                            )
                        {
                            //I didn't feel like trying to invert the if statement above so I added an else statement insted.
                        }
                        else
                        {
                            required = false;
                        }

                    }
                    else
                    {
                        required = false;
                    }
                }


                //Don't validate unless this field is currently required
                if (required)
                {
                    //If we are not using expectedValueOfCurrentField then do normal validation
                    if (expectedValueOfField == null)
                    {
                        if (!_innerAttribute.IsValid(valueOfField))
                            // validation failed - return an error
                            return new ValidationResult(FormatErrorMessage(validationContext.DisplayName), new[] { validationContext.MemberName });
                    }
                    else
                    {
                        if (!valueOfField.Equals(expectedValueOfField))
                            // validation failed - return an error
                            return new ValidationResult(FormatErrorMessage(validationContext.DisplayName), new[] { validationContext.MemberName });
                    }
                }
            }
            return ValidationResult.Success;
        }

        public virtual IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            int count = 0;
            string Key = metadata.ContainerType.FullName + "." + metadata.GetDisplayName();

            //This is used to make unique requiredIf attribute names for when we use multiple
            //requiredIf statements on the same field.
            if (countPerField == null)
                countPerField = new Dictionary<string, int>();

            if (countPerField.ContainsKey(Key))
            {
                count = ++countPerField[Key];
            }
            else
                countPerField.Add(Key, count);

            //Reset once we reach the end of the alphabet
            if (count == 26)
            {
                countPerField = null;
            }

            string tmp = count == 0 ? "" : Char.ConvertFromUtf32(96 + count);

            var rule = new ModelClientValidationRule
            {
                ErrorMessage = FormatErrorMessage(metadata.GetDisplayName()),
                ValidationType = "requiredif"+tmp,
            };

            for (int x = 0; x < DependentPropertyArray.Length; x++)
            {
                string depProp = BuildDependentPropertyId(metadata, context as ViewContext, DependentPropertyArray[x]);

                // find the value on the control we depend on;
                // if it's a bool, format it javascript style
                // (the default is True or False!)
                string targetValue = (TargetValueArray[x] ?? "").ToString();
                if (TargetValueArray[x] is bool)
                    targetValue = targetValue.ToLower();

                if (x == 0)
                {
                    rule.ValidationParameters.Add("dependentproperty" + tmp, depProp);
                    rule.ValidationParameters.Add("targetvalue" + tmp, targetValue);
                }
                else
                {
                    rule.ValidationParameters.Add("dependentproperty" + tmp + x, depProp);
                    rule.ValidationParameters.Add("targetvalue" + tmp + x, targetValue);
                }
            }

            yield return rule;
        }

        private string BuildDependentPropertyId(ModelMetadata metadata, ViewContext viewContext, String depPropVal)
        {
            // build the ID of the property            
            string depProp = viewContext.ViewData.TemplateInfo.GetFullHtmlFieldId(depPropVal);
            
            // unfortunately this will have the name of the current field appended to the beginning,
            // because the TemplateInfo's context has had this fieldname appended to it. Instead, we
            // want to get the context as though it was one level higher (i.e. outside the current property,
            // which is the containing object, and hence the same level as the dependent property.
            var thisField = metadata.PropertyName + "_";
            if (depProp.StartsWith(thisField))
                // strip it off again
                depProp = depProp.Substring(thisField.Length);
            return depProp;
        }
    }
}