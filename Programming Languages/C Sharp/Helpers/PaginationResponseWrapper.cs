using LCIInterfaces;
using System;
using System.Collections.Generic;

namespace LCIWebAPI.Business.Wrappers
{
    /// <summary>
    /// If your object uses server side pagination then use this wrapper 
    /// </summary>
    public class PaginationResponseWrapper<T> : IPaginationResponseWrapper<T>
    {
        public IEnumerable<T> Items { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }
        public int TotalCount { get; set; }
        public int PageCount
        {
            get
            {
                if (TotalCount != 0 && Take != 0)
                {
                    return (int)Math.Ceiling((double)TotalCount / Take);
                }
                return 0;
            }
        }
        public int CurrentPage
        {
            get
            {
                if (Take != 0)
                {
                    return (int)Math.Ceiling((double)Skip / Take) + 1;
                }
                return 0;
            }
        }
        public string NextPageUri { get; set; }
        public string PrevPageUri { get; set; }
    }
}