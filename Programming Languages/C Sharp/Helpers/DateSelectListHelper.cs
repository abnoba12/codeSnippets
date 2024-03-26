using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace OMS40.Helpers
{
    public class DateSelectListHelper
    {
        public static SelectList Day
        {
            get
            {
                List<DropDownListItemHelper> tempList = new List<DropDownListItemHelper>();
                tempList.Add(new DropDownListItemHelper("", "Select One:"));
                for (int x = 1; x <= 31; x++)
                {
                    DropDownListItemHelper tempDropDownListItemHelper = new DropDownListItemHelper();
                    tempDropDownListItemHelper.value = x.ToString();
                    tempDropDownListItemHelper.text = x.ToString();

                    tempList.Add(tempDropDownListItemHelper);
                }
                return new SelectList(tempList, "value", "text");
            }
        }

        //Listing of days that exist in every month
        public static SelectList CommonDay
        {
            get
            {
                List<DropDownListItemHelper> tempList = new List<DropDownListItemHelper>();
                tempList.Add(new DropDownListItemHelper("", "Select One:"));
                for (int x = 1; x <= 28; x++)
                {
                    DropDownListItemHelper tempDropDownListItemHelper = new DropDownListItemHelper();
                    tempDropDownListItemHelper.value = x.ToString();
                    tempDropDownListItemHelper.text = x.ToString();

                    tempList.Add(tempDropDownListItemHelper);
                }
                return new SelectList(tempList, "value", "text");
            }
        }

        /// <summary>
        /// Select list for the twelve months
        /// </summary>
        public static SelectList Month
        {
            get
            {
                List<DropDownListItemHelper> tempList = new List<DropDownListItemHelper>();
                tempList.Add(new DropDownListItemHelper("", "Month"));
                for (int x = 1; x <= 12; x++)
                {
                    DropDownListItemHelper tempDropDownListItemHelper = new DropDownListItemHelper();
                    tempDropDownListItemHelper.value = x.ToString();
                    DateTime month = new DateTime(2000, x, 1);
                    tempDropDownListItemHelper.text = String.Format("{0:MMMM}", month);

                    tempList.Add(tempDropDownListItemHelper);
                }
                return new SelectList(tempList, "value", "text");
            }
        }

        /// <summary>
        /// Years starting from 80 years ago till 18 years ago
        /// </summary>
        public static SelectList YearEightTeenToEighty
        {
            get
            {
                List<DropDownListItemHelper> tempList = new List<DropDownListItemHelper>();
                tempList.Add(new DropDownListItemHelper("", "Select One:"));
                int eightTeenPlusStart = DateTime.Now.AddYears(-18).Year;
                int eightTeenPlusEnd = DateTime.Now.AddYears(-80).Year;
                for (int x = eightTeenPlusEnd; x <= eightTeenPlusStart; x++)
                {
                    DropDownListItemHelper tempDropDownListItemHelper = new DropDownListItemHelper();
                    tempDropDownListItemHelper.value = x.ToString();
                    tempDropDownListItemHelper.text = x.ToString();

                    tempList.Add(tempDropDownListItemHelper);
                }
                return new SelectList(tempList, "value", "text");
            }
        }

        /// <summary>
        /// Years starting from 80 years ago till now
        /// </summary>
        public static SelectList YearToEighty
        {
            get
            {
                List<DropDownListItemHelper> tempList = new List<DropDownListItemHelper>();
                tempList.Add(new DropDownListItemHelper("", "Select One:"));
                int Start = DateTime.Now.Year;
                int End = DateTime.Now.AddYears(-80).Year;
                for (int x = End; x <= Start; x++)
                {
                    DropDownListItemHelper tempDropDownListItemHelper = new DropDownListItemHelper();
                    tempDropDownListItemHelper.value = x.ToString();
                    tempDropDownListItemHelper.text = x.ToString();

                    tempList.Add(tempDropDownListItemHelper);
                }
                return new SelectList(tempList, "value", "text");
            }
        }

        /// <summary>
        /// Years starting from now to 15 years into the future
        /// </summary>
        public static SelectList YearFutureTofifteen
        {
            get
            {
                List<DropDownListItemHelper> tempList = new List<DropDownListItemHelper>();
                tempList.Add(new DropDownListItemHelper("", "Year"));
                for (int x = System.DateTime.Now.Year; x <= (System.DateTime.Now.Year + 15); x++)
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