using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OMSReturns.Helpers
{
    public class DropDownListItemHelper
    {        
        public string value { get; set; }
        public string text { get; set; }

        public DropDownListItemHelper(string value, string text)
        {
            this.value = value;
            this.text = text;
        }

        public DropDownListItemHelper(){}
    }

    public class DropDownHelper
    {
        /// <summary>
        /// Used to build a Select list from any object
        /// </summary>
        /// <typeparam name="T">The object type that the drop down will be built from</typeparam>
        /// <param name="list">A list of objects to build the drop down from</param>
        /// <param name="value">Name of the field to put into the values fields of the dropdown</param>
        /// <param name="text">Name of the field to put into the text fields of the dropdown</param>
        /// <returns>A SelectList</returns>
        public static SelectList BuildDropDown<T>(List<T> list, string value, string text)
        {
            //This is what will hold our data to turn into a select list
            List<DropDownListItemHelper> tempList = new List<DropDownListItemHelper>();

            //get the items inside of the list
            //Type typeParameterType = typeof(T);
            var properties = typeof(T).GetProperties(); 
            foreach (T item in list)
            {
                //loop through all the properies in the object to find what we want to use for our text and value
                string SelectListItemText = "";
                string SelectListItemValue = "";
                foreach (var property in properties)
                {
                    string propertyName = property.Name;
                    if (propertyName == text)
                    {
                        SelectListItemText = property.GetValue(item, null).ToString();
                    }
                    if (propertyName == value)
                    {
                        SelectListItemValue = property.GetValue(item, null).ToString();
                    }
                }

                //If we have a SelectListItemText and SelectListItemValue then add it to the select list
                if (!String.IsNullOrEmpty(SelectListItemValue) && !String.IsNullOrEmpty(SelectListItemText))
                {
                    tempList.Add(new DropDownListItemHelper(SelectListItemValue, SelectListItemText)); 
                }
            }
            return new SelectList(tempList, "value", "text");
        }
    }
}