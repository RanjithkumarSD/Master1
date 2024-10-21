using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using Moq;
using NUnit.Framework;
using System.Net;
using Xunit;
using System.Threading;
using System.Threading.Tasks;
using Moq.Protected;

namespace BupaCodeAssesment.Controllers
{
    public class HomeController : Controller
    {
        private readonly string bupaRequestURLApiEndpoint = "https://digitalcodingtest.bupa.com.au/api/v1/bookowners";
        public ActionResult Index()
        {
            var bookCategories = new Dictionary<string, List<string>>();

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage apiResponse = client.GetAsync(bupaRequestURLApiEndpoint).Result;
                if (apiResponse.IsSuccessStatusCode)
                {
                    var apiResponseData = apiResponse.Content.ReadAsStringAsync().Result;
                    JArray responseDataJsonArray = JArray.Parse(apiResponseData);

                    foreach (var person in responseDataJsonArray)
                    {
                        string category = (int)person["age"] >= 18 ? "Adults" : "Children";
                        if (!bookCategories.ContainsKey(category))
                        {
                            bookCategories[category] = new List<string>();
                        }

                        foreach (var book in person["books"])
                        {
                            bookCategories[category].Add((string)book["name"]);
                        }
                    }

                    foreach (var category in bookCategories.Keys.ToList())
                    {
                        bookCategories[category] = bookCategories[category].Distinct().OrderBy(b => b).ToList();
                    }
                }
            }

            return View(bookCategories);


            //key = "Adults" , value = {
            //                          "Great Expectations",
            //                           "Gulliver's Travels",
            //                           "Hamlet",
            //                           "Jane Eyre",
            //                           "React: The Ultimate Guide",
            //                           "Wuthering Heights"
            //                         };

            //key = "Children" , value = {
            //                            "Great Expectations",
            //                            "Hamlet",
            //                            "Little Red Riding Hood",
            //                            "The Hobbit"
            //                            };
        }

    } 
}

