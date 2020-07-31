using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;

namespace SharesAPI.Currency
{
    public class CurrencyAPI
    {
        public double conversion {get; set;}
        public static async System.Threading.Tasks.Task<double?> convert(string Currency, double price) {
            string url = $"http://localhost:8000/currency_converter/{Currency}?format=json&base=USD";
            using (HttpClient client = new HttpClient())
            {
                try{
                    client.Timeout = TimeSpan.FromSeconds(4);
                    HttpResponseMessage response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode();
                    if (response.IsSuccessStatusCode)
                    {
                        CurrencyAPI content = response.Content.ReadAsAsync<CurrencyAPI>().Result;
                        return content.conversion * price;
                    }
                    return null;
                }
                catch {
                    return null;
                }
            }
        }
    }
}
