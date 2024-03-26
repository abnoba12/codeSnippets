using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StonebridgeJCP.Helpers
{
    public class StateSelectListHelper
    {
        public static SelectList StateList
        {
            get
            {
                List<DropDownListItemHelper> tempList = new List<DropDownListItemHelper>();
                tempList.Add(new DropDownListItemHelper { value = "", text = "Select One:" });
                tempList.Add(new DropDownListItemHelper { value = "AL", text = "Alabama" });
                tempList.Add(new DropDownListItemHelper { value = "AK", text = "Alaska" });
                tempList.Add(new DropDownListItemHelper { value = "AZ", text = "Arizona" });
                tempList.Add(new DropDownListItemHelper { value = "AR", text = "Arkansas" });
                tempList.Add(new DropDownListItemHelper { value = "CA", text = "California" });
                tempList.Add(new DropDownListItemHelper { value = "CO", text = "Colorado" });
                tempList.Add(new DropDownListItemHelper { value = "CT", text = "Connecticut" });
                tempList.Add(new DropDownListItemHelper { value = "DE", text = "Delaware" });
                tempList.Add(new DropDownListItemHelper { value = "DC", text = "District of Columbia" });
                tempList.Add(new DropDownListItemHelper { value = "FL", text = "Florida" });
                tempList.Add(new DropDownListItemHelper { value = "GA", text = "Georgia" });
                tempList.Add(new DropDownListItemHelper { value = "HI", text = "Hawaii" });
                tempList.Add(new DropDownListItemHelper { value = "ID", text = "Idaho" });
                tempList.Add(new DropDownListItemHelper { value = "IL", text = "Illinois" });
                tempList.Add(new DropDownListItemHelper { value = "IN", text = "Indiana" });
                tempList.Add(new DropDownListItemHelper { value = "IA", text = "Iowa" });
                tempList.Add(new DropDownListItemHelper { value = "KS", text = "Kansas" });
                tempList.Add(new DropDownListItemHelper { value = "KY", text = "Kentucky" });
                tempList.Add(new DropDownListItemHelper { value = "LA", text = "Louisiana" });
                tempList.Add(new DropDownListItemHelper { value = "ME", text = "Maine" });
                tempList.Add(new DropDownListItemHelper { value = "MT", text = "Montana" });
                tempList.Add(new DropDownListItemHelper { value = "NE", text = "Nebraska" });
                tempList.Add(new DropDownListItemHelper { value = "NV", text = "Nevada" });
                tempList.Add(new DropDownListItemHelper { value = "NH", text = "New Hampshire" });
                tempList.Add(new DropDownListItemHelper { value = "NJ", text = "New Jersey" });
                tempList.Add(new DropDownListItemHelper { value = "NM", text = "New Mexico" });
                tempList.Add(new DropDownListItemHelper { value = "NY", text = "New York" });
                tempList.Add(new DropDownListItemHelper { value = "NC", text = "North Carolina" });
                tempList.Add(new DropDownListItemHelper { value = "ND", text = "North Dakota" });
                tempList.Add(new DropDownListItemHelper { value = "OH", text = "Ohio" });
                tempList.Add(new DropDownListItemHelper { value = "OK", text = "Oklahoma" });
                tempList.Add(new DropDownListItemHelper { value = "OR", text = "Oregon" });
                tempList.Add(new DropDownListItemHelper { value = "MD", text = "Maryland" });
                tempList.Add(new DropDownListItemHelper { value = "MA", text = "Massachusetts" });
                tempList.Add(new DropDownListItemHelper { value = "MI", text = "Michigan" });
                tempList.Add(new DropDownListItemHelper { value = "MN", text = "Minnesota" });
                tempList.Add(new DropDownListItemHelper { value = "MS", text = "Mississippi" });
                tempList.Add(new DropDownListItemHelper { value = "MO", text = "Missouri" });
                tempList.Add(new DropDownListItemHelper { value = "PA", text = "Pennsylvania" });
                tempList.Add(new DropDownListItemHelper { value = "RI", text = "Rhode Island" });
                tempList.Add(new DropDownListItemHelper { value = "SC", text = "South Carolina" });
                tempList.Add(new DropDownListItemHelper { value = "SD", text = "South Dakota" });
                tempList.Add(new DropDownListItemHelper { value = "TN", text = "Tennessee" });
                tempList.Add(new DropDownListItemHelper { value = "TX", text = "Texas" });
                tempList.Add(new DropDownListItemHelper { value = "UT", text = "Utah" });
                tempList.Add(new DropDownListItemHelper { value = "VT", text = "Vermont" });
                tempList.Add(new DropDownListItemHelper { value = "VA", text = "Virginia" });
                tempList.Add(new DropDownListItemHelper { value = "WA", text = "Washington" });
                tempList.Add(new DropDownListItemHelper { value = "WV", text = "West Virginia" });
                tempList.Add(new DropDownListItemHelper { value = "WI", text = "Wisconsin" });
                tempList.Add(new DropDownListItemHelper { value = "WY", text = "Wyoming" });
                return new SelectList(tempList, "value", "text", 1);
            }
        }
    }
}