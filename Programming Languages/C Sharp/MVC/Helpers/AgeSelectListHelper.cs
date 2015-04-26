using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StonebridgeJCP.Helpers
{
    public class AgeSelectListHelper
    {
        public static SelectList AgeList
        {
            get
            {
                List<DropDownListItemHelper> tempList = new List<DropDownListItemHelper>();
                for (int x = 8; x <= 110; x++)
                {
                    DropDownListItemHelper tempDropDownListItemHelper = new DropDownListItemHelper();
                    tempDropDownListItemHelper.value = x.ToString();
                    tempDropDownListItemHelper.text = x.ToString();

                    tempList.Add(tempDropDownListItemHelper);
                }
                return new SelectList(tempList, "value", "text");
            }
        }
    }
}