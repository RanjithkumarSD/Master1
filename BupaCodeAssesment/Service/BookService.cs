using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using BupaCodeAssesment.Models;

namespace BupaCodeAssesment.Services
{
    public class HttpClientService
    {
        private readonly HttpClient _httpClient;

        public HttpClientService(HttpClient httpClient)
        {
            _httpClient = new HttpClient();
        }

        public async Task<List<BookOwner>> GetBookOwnersAsync(string url)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode(); 

                var data = await response.Content.ReadAsStringAsync();
                var bookOwners = JArray.Parse(data).ToObject<List<BookOwner>>();
                return bookOwners;
            }
            catch (HttpRequestException httpRequestException)
            {
                throw new Exception("Error while fetching book owners: " + httpRequestException.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred: " + ex.Message);
            }
        }
    }
}
