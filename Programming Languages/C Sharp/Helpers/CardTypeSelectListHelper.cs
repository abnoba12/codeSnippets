using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StonebridgeJCP.Helpers
{
    public class CardTypeSelectListHelper
    {
        public static SelectList CardTypeList
        {
            get
            {
                List<DropDownListItemHelper> tempList = new List<DropDownListItemHelper>();
                tempList.Add(new DropDownListItemHelper { value = "", text = "Select One:" });
                tempList.Add(new DropDownListItemHelper { value = "06", text = "jcpenney rewards credit card" });
                tempList.Add(new DropDownListItemHelper { value = "22", text = "American Express" });
                tempList.Add(new DropDownListItemHelper { value = "17", text = "Discover" });
                tempList.Add(new DropDownListItemHelper { value = "05", text = "Master Card" });
                tempList.Add(new DropDownListItemHelper { value = "04", text = "Visa" });
                return new SelectList(tempList, "value", "text", 1);
            }
        }
    }
}