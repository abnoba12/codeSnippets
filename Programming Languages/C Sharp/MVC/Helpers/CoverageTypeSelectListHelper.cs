using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StonebridgeJCP.Helpers
{
    public class CoverageTypeSelectListHelper
    {
        public static SelectList CoverageTypeList
        {
            get
            {
                //TODO: -JWeigand- Get the real list of coverage types
                List<DropDownListItemHelper> tempList = new List<DropDownListItemHelper>();
                tempList.Add(new DropDownListItemHelper { value = "FGBL", text = "Modified Whole Life" });
                tempList.Add(new DropDownListItemHelper { value = "GBL", text = "Graded Benefit Life" });
                tempList.Add(new DropDownListItemHelper { value = "SUWL", text = "Underwritten Whole Life" });
                tempList.Add(new DropDownListItemHelper { value = "T69", text = "Term to 69" });
                tempList.Add(new DropDownListItemHelper { value = "T80", text = "Term to 80" });
                tempList.Add(new DropDownListItemHelper { value = "AD", text = "Accidental Death" });
                tempList.Add(new DropDownListItemHelper { value = "AD&D", text = "Accidental Death and Dismemberment" });
                return new SelectList(tempList, "value", "text", 1);
            }
        }
    }

    public class ProductLookup
    {
        public static Dictionary<string, productLocation> productLookup
        {
            get
            {
                //TODO: -JWeigand- Finish filling out the list of products
                Dictionary<string, productLocation> productLookupTemp = new Dictionary<string, productLocation>();
                productLookupTemp.Add("FGBL", new productLocation("", ""));
                productLookupTemp.Add("GBL", new productLocation("", ""));
                productLookupTemp.Add("SUWL", new productLocation("", ""));
                productLookupTemp.Add("T69", new productLocation("", ""));
                productLookupTemp.Add("T80", new productLocation("Term80", "Term80Enroll"));
                productLookupTemp.Add("AD", new productLocation("AD", "CustomerInformation"));
                productLookupTemp.Add("AD&D", new productLocation("", ""));
                return productLookupTemp;
            }
        }
    }

    public struct productLocation{
        public string controller, action;

        public productLocation(string controller, string action){
            this.controller = controller;
            this.action = action;
        }
    }
}