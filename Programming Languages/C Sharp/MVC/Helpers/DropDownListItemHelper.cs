using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OMS40.Helpers
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
}