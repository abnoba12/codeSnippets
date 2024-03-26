using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.Text.RegularExpressions;

namespace StonebridgeJCP.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = false)]
    public class FutureDateAttribute : ValidationAttribute
    {
        //Get a list of the accepted card types for this validation
        public string year;
        public string month;
        public string day;

        /// <summary>
        /// Is the value of this field in the future?
        /// </summary>
        public FutureDateAttribute()
        {
        }

        /// <summary>
        /// Pass in the name of the year and month fields to validate against
        /// </summary>
        /// <param name="year">Field containing the year value</param>
        /// <param name="month">Field containing the month value</param>
        public FutureDateAttribute(string year, string month)
        {
            this.year = year;
            this.month = month;
        }

        /// <summary>
        /// Pass in the name of the year, month, and day fields to validate againt
        /// </summary>
        /// <param name="year">Field containing the year value</param>
        /// <param name="month">Field containing the month value</param>
        /// <param name="day">Field containing the day value</param>
        public FutureDateAttribute(string year, string month, string day)
        {
            this.year = year;
            this.month = month;
            this.day = day;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            DateTime passedDate = new DateTime();

            if (value != null)
            {
                string submittedDate = (string)value;
                passedDate = DateTime.Parse(submittedDate);
            }
            else
            {
                // get a reference to the property this validation depends upon
                var containerType = validationContext.ObjectInstance.GetType();

                //Pull the values by the field names we were passed
                var yearField = containerType.GetProperty(this.year);
                var yearFieldValue = yearField.GetValue(validationContext.ObjectInstance, null);
                var monthField = containerType.GetProperty(this.month);
                var monthFieldValue = monthField.GetValue(validationContext.ObjectInstance, null);
                object dayFieldValue = null;
                if (!String.IsNullOrEmpty(this.day))
                {
                    var dayField = containerType.GetProperty(this.day);
                    dayFieldValue = dayField.GetValue(validationContext.ObjectInstance, null);
                }

                //Do have a valid Year and month
                if (yearFieldValue != null && monthFieldValue != null && yearFieldValue is string && monthFieldValue is string)
                {
                    //Do we also have a valid day?
                    if (dayFieldValue != null && dayFieldValue is string)
                    {
                        passedDate = DateTime.Parse(yearFieldValue + "-" + monthFieldValue + "-" + dayFieldValue);
                    }
                    else
                    {
                        passedDate = DateTime.Parse(monthFieldValue + "/" + yearFieldValue);
                        //Get the end of the current month instead of the first
                        passedDate = passedDate.AddMonths(1);
                        passedDate = passedDate.Subtract(TimeSpan.FromDays(1));
                    }
                }
            }

            if (DateTime.Compare(passedDate, System.DateTime.Now) < 0 && (passedDate.Year != 1 || passedDate.Month != 1 || passedDate.Day != 1))
            {
                // validation failed - return an error
                return new ValidationResult(FormatErrorMessage(validationContext.DisplayName), new[] { validationContext.MemberName });
            }
            return ValidationResult.Success;
        }
    }
}