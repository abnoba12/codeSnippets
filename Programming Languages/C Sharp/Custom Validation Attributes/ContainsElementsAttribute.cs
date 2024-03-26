using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SmartWeb.Web.Helpers
{
    /// <summary>
    /// Validator to ensure the list has at least X number of elements in it
    /// EX: [ContainsMinElements(1, ErrorMessage = "At least one test window is required")]
    /// </summary>
    public class ContainsMinElementsAttribute : ValidationAttribute
    {
        private readonly int _minElements;
        public ContainsMinElementsAttribute(int minElements)
        {
            _minElements = minElements;
        }

        public override bool IsValid(object value)
        {
            var list = value as IList;
            if (list != null)
            {
                return list.Count >= _minElements;
            }
            return false;
        }
    }
}