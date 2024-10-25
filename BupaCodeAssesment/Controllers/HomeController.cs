using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Mvc;
using System.Threading.Tasks;

namespace BupaCodeAssesment.Controllers
{
    public class HomeController : Controller
    {
        private readonly string bupaRequestURLApiEndpoint = "https://digitalcodingtest.bupa.com.au/api/v1/bookowners";//Api for Bupa
        
        public async Task <ActionResult> Index()
        {
            var bookCategories = new Dictionary<string, List<string>>();
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage apiResponse = await client.GetAsync(bupaRequestURLApiEndpoint);//Requesting to the Api 
                    if (apiResponse.IsSuccessStatusCode)
                    {
                        var apiResponseData = await apiResponse.Content.ReadAsStringAsync();//Getting the data
                        JArray bookOwners = JArray.Parse(apiResponseData);

                        foreach (var bookOwner in bookOwners)
                        {
                            string category = (int)bookOwner["age"] >= 18 ? "Adults" : "Children"; // Check the Conditions for Adults and Children
                            if (!bookCategories.ContainsKey(category))
                            {
                                bookCategories[category] = new List<string>();
                            }

                            foreach (var book in bookOwner["books"])
                            {
                                bookCategories[category].Add((string)book["name"]);// Add the name
                            }
                        }

                        foreach (var category in bookCategories.Keys.ToList())
                        {
                            bookCategories[category] = bookCategories[category].Distinct().OrderBy(book => book).ToList();//Sorting the book name 
                        }
                    }
                }
            return View(bookCategories); // Expected result showing here --bookCategories

            //Expected result:

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

