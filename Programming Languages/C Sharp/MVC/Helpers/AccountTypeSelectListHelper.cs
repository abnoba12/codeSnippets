using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StonebridgeJCP.Helpers
{
    public class AccountTypeSelectListHelper
    {
        public static SelectList AccountTypeList
        {
            get
            {
                List<DropDownListItemHelper> tempList = new List<DropDownListItemHelper>();
                tempList.Add(new DropDownListItemHelper { value = "", text = "Select One:" });
                tempList.Add(new DropDownListItemHelper { value = "Checking", text = "Checking" });
                tempList.Add(new DropDownListItemHelper { value = "Savings", text = "Savings" });
                return new SelectList(tempList, "value", "text", 1);
            }
        }
    }
}