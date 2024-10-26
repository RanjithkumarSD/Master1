using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Threading.Tasks;
using BupaCodeAssesment.Services;
using System;
using System.Net.Http;

namespace BupaCodeAssesment.Controllers
{
    public class HomeController : Controller
    {
        private readonly string bupaRequestURLApiEndpoint = "https://digitalcodingtest.bupa.com.au/api/v1/bookowners"; // API endpoint
        private readonly HttpClientService _httpClientService;

        public HomeController()
        {
            _httpClientService = new HttpClientService(new HttpClient());
        }

        public ActionResult Index()
        {
            var bookCategories = new Dictionary<string, List<string>>();

            try
            {
                var bookOwners = await _httpClientService.GetBookOwnersAsync(bupaRequestURLApiEndpoint);

                if (bookOwners != null)
                {
                    foreach (var bookOwner in bookOwners)
                    {
                        string category = bookOwner.Age >= 18 ? "Adults" : "Children"; // Determine category
                        if (!bookCategories.ContainsKey(category))
                        {
                            bookCategories[category] = new List<string>();
                        }

                        foreach (var book in bookOwner.Books)
                        {
                            bookCategories[category].Add(book.Name); // Add book names
                        }
                    }

                    foreach (var category in bookCategories.Keys.ToList())
                    {
                        bookCategories[category] = bookCategories[category].Distinct().OrderBy(booknames => booknames).ToList(); // Sort book names
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
            }

            return View(bookCategories); // Return the results to the view
        }

        //    //Expected result:

        //    //key = "Adults" , value = {
        //    //                          "Great Expectations",
        //    //                           "Gulliver's Travels",
        //    //                           "Hamlet",
        //    //                           "Jane Eyre",
        //    //                           "The Ultimate Guide",
        //    //                           "Wuthering Heights"
        //    //                         };

        //    //key = "Children" , value = {
        //    //                            "Great Expectations",
        //    //                            "Hamlet",
        //    //                            "Little Red Riding Hood",
        //    //                            "The Hobbit"
        //    //                            };
        //}

    }
}


