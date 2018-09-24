using RedfinProject.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace RedfinProject
{
    class FoodTruckClient
    {
        /// <summary>
        /// Food Truck API Base URL
        /// </summary>
        public string BaseUrl { get; set; }

        /// <summary>
        /// HttpClient used to make GET requests
        /// </summary>
        public HttpClient HttpClient { get; set; }

        /// <summary>
        /// Current date when running application
        /// </summary>
        public DateTime CurrentDate { get; set; }

        /// <summary>
        /// Maximum number of results per page
        /// </summary>
        public int PageLimit { get; set; }

        public FoodTruckClient()
        {
            this.BaseUrl = "https://data.sfgov.org/resource/bbb8-hzi6.json";
            this.HttpClient = new HttpClient();
            /*Required to make HTTPS requests*/
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

            /*Convert current datetime to PST, if user is in a different time zone*/
            var pacificZone = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");
            this.CurrentDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, pacificZone);
            this.PageLimit = 10;
        }

        /// <summary>
        /// Returns the query string which the API will use to filter 
        /// food trucks based on the current time and day of the week. 
        /// Query will page results using the pageNumber param and the 
        /// PageLimit propery and will order results by food truck name. 
        /// </summary>
        /// <param name="pageNum">Page number used to calculate offset in result set</param>
        /// <returns>URL query to make API request</returns>
        private string GetUrlQuery(int pageNum)
        {
            var currTime = this.CurrentDate.ToString("HH:mm"); // (24 hour clock)
            var currDayOfWeek = this.CurrentDate.DayOfWeek;
            string url = this.BaseUrl;

            /*Only retrieve food truck that are currently open, based on the current time and day of the week*/
            url += $"?$where=start24<='{currTime}'and%20end24>='{currTime}'and%20dayofweekstr='{currDayOfWeek}'";
            url += $"&$limit={this.PageLimit}&$offset={pageNum * this.PageLimit}";
            url += "&$order=applicant";
            return url;
        }

        /// <summary>
        /// Retrieves result set from food truck API with specified page number
        /// and converts result set to a list of FoodTruck objects />
        /// </summary>
        /// <param name="pageNum">Page number used to calculate offset in result set</param>
        /// <returns></returns>
        public async Task<List<FoodTruck>> GetFoodTruckListAsync(int pageNum)
        {
            List<FoodTruck> foodTruckList = null;
            try
            {
                var response = await this.HttpClient.GetAsync(this.GetUrlQuery(pageNum));
                if (response.IsSuccessStatusCode)
                {
                    foodTruckList = await response.Content.ReadAsAsync<List<FoodTruck>>();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occured while retrieving food truck list: " + e.Message);
            }
            
            return foodTruckList;
        }
    }
}