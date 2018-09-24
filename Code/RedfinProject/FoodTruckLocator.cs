using RedfinProject.Models;
using System;
using System.Collections.Generic;

namespace RedfinProject
{
    class FoodTruckLocator
    {
        static void Main(string[] args)
        {
            var foodTruckClient = new FoodTruckClient();
            int pageNum = 0;
            Console.WriteLine("Welcome to Food Truck Locator! The following food trucks are open:");
            string line = "";
            List<FoodTruck> results = null;
            while(line != "exit")
            {
                results = foodTruckClient.GetFoodTruckListAsync(pageNum).GetAwaiter().GetResult();
                if(results == null)
                {
                    Console.WriteLine("** Unable to retreive food truck list. **");
                    Console.ReadKey();
                    return;
                }
                if (results.Count == 0)
                {
                    Console.WriteLine("** No more results to display. **");
                    Console.ReadKey();
                    return;
                }
                Console.WriteLine($"Page {pageNum++ + 1}:");
                DisplayFoodTruckResults(results);
                line = Console.ReadLine();
            }        
        }

        /// <summary>
        /// Print each food truck's name and address in the list
        /// </summary>
        /// <param name="results">Food Truck list to print</param>
        private static void DisplayFoodTruckResults(List<FoodTruck> results)
        {
            const string col1Name = "FOOD TRUCK NAME";
            const string col2Name = "ADDRESS";
            const int colSize = 30;

            Console.WriteLine($"{col1Name, colSize} {col2Name,colSize}");
            foreach(var foodTruck in results)
            {
                /*shorten long food truck names and addresses*/
                var foodTruckName = foodTruck.Applicant.Length > colSize ? foodTruck.Applicant.Substring(0, colSize-3) + "..." : foodTruck.Applicant;
                var foodTruckLocation = foodTruck.Location.Length > colSize ? foodTruck.Location.Substring(0, colSize-3) + "..." : foodTruck.Location;
                Console.WriteLine($"{foodTruckName, colSize} {foodTruckLocation, colSize}");
            }
            Console.WriteLine("* Press enter to see more results or type 'exit' to quit. *");
        }
    }
}