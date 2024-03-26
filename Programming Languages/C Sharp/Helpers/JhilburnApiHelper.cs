/* Usage:

string URL = string.Format("/partner/{0}", partnerId);
JArray ja = Helpers.JhilburnApiHelper.apiGetRequest(URL);

if (ja != null && ja.First != null)
{
	JObject jo = ja.First.Value<JObject>();
	DateTime join_date = Convert.ToDateTime(jo["Join_Date"].ToString());
	ViewBag.end_date = join_date.AddDays(65).ToString("M/d/yy, htt");
}
-----------------------
string URL = string.Format("/partner/newclientcount/{0}", partnerId);
JArray ja = Helpers.JhilburnApiHelper.apiGetRequest(URL);
			
if (ja != null)
{
	foreach (JObject ClientAcquisition in ja)
	{
		//buncball level stuff
		var l = f.Find(a => a.PartnerId == ClientAcquisition["PartnerId"].ToString());

		bool OptIn = bool.Parse(ClientAcquisition["OptIn"].ToString());

		DateTime JoinDate = new DateTime();
		DateTime.TryParse(ClientAcquisition["Partner"]["Join_Date"].ToString(), out JoinDate);
		string phoneNumber = Regex.Replace(ClientAcquisition["Partner"]["Phone_1"].ToString(), "[^.0-9]", "");

		StringBuilder partnerInfo = new StringBuilder();
		
	}
}
-----------------------------------------
	string apiURL1 = string.Format("/partner/{0}", partnerId);
	List<OMS40.Models.Partner> stylistJa1 = Helpers.JhilburnApiHelper.apiGetRequest<OMS40.Models.Partner>(apiURL1);
*/


using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.Script.Serialization;

namespace OMS40.Helpers
{
    public class JhilburnApiHelper
    {
        /// <summary>
        /// Make a request to the jhilburn API for the JSON data
        /// </summary>
        /// <param name="URL">URL relative to the api eg: "stylistpromos/fastfive/2756"</param>
        /// <returns>returns an array of assoicative arrays</returns>
        public static JArray apiGetRequest(string URL)
        {
            URL = string.Format(WebConfigurationManager.AppSettings["JHApiUrl"].ToString() + "{0}", URL);
            var request = System.Net.HttpWebRequest.Create(URL) as HttpWebRequest;
            request.ContentType = "application/x-www-form-urlencoded";
            request.Method = "GET";
            request.Accept = "application/json";
            UTF8Encoding encoding = new UTF8Encoding();

            try
            {
                using (WebResponse response = request.GetResponse())
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        //read the API data and put into a JSON object
                        StreamReader rdr = new StreamReader(stream);
                        string json = rdr.ReadToEnd();
                        JArray ja = new JArray();

                        //If this is a JSON object then turn it into an array
                        if (json[0] == '[')
                        {
                            ja = JArray.Parse(json);                            
                        }
                        else
                        {
                            JObject jo = JObject.Parse(json);
                            ja.Add(jo);
                        }
                        return ja;
                    }
                }
            }
            catch (WebException ex)
            {
                HttpWebResponse errorResponse = ex.Response as HttpWebResponse;

                //We want to swallow 404 errors
                if (errorResponse != null && errorResponse.StatusCode != HttpStatusCode.NotFound)
                {
                    Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                    return null;
                }
                return null;
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                return null;
            }
        }

        /// <summary>
        /// Make a request to the jhilburn API for the JSON data.
        /// Usage:
        /// string apiURL = string.Format("/partner/{0}", partnerId);
        /// List<OMS40.Models.Partner> partnerList = Helpers.JhilburnApiHelper.apiGetRequest<OMS40.Models.Partner>(apiURL);
        /// </summary>
        /// <typeparam name="T">The Model to hydrate</typeparam>
        /// <param name="URL">URL relative to the api eg: "stylistpromos/fastfive/2756"</param>
        /// <returns>Will return a list of T objects</returns>
        public static List<T> apiGetRequest<T>(string URL) where T : new()
        {
            URL = string.Format(WebConfigurationManager.AppSettings["JHApiUrl"].ToString() + "{0}", URL);
            var request = System.Net.HttpWebRequest.Create(URL) as HttpWebRequest;
            request.ContentType = "application/x-www-form-urlencoded";
            request.Method = "GET";
            request.Accept = "application/json";
            UTF8Encoding encoding = new UTF8Encoding();
            List<T> result = new List<T>();

            try
            {
                using (WebResponse response = request.GetResponse())
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        //read the API data and put into a JSON object
                        StreamReader rdr = new StreamReader(stream);
                        string json = rdr.ReadToEnd();
                        JArray ja = new JArray();

                        //If this is a JSON object then turn it into an array
                        if (json[0] == '[')
                        {
                            ja = JArray.Parse(json);
                            if (ja != null)
                            {
                                foreach (JObject item in ja)
                                {
                                    JObject jo = item.Value<JObject>();
                                    result.Add(jo.ToObject<T>());
                                }
                            }
                        }
                        else
                        {
                            JObject jo = JObject.Parse(json);
                            result.Add(jo.ToObject<T>());
                        }
                        return result;
                    }
                }
            }
            catch (WebException ex)
            {
                HttpWebResponse errorResponse = ex.Response as HttpWebResponse;

                //We want to swallow 404 errors
                if (errorResponse != null && errorResponse.StatusCode != HttpStatusCode.NotFound)
                {
                    Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                    return null;
                }
                return null;
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                return null;
            }
        }
    }
}