using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using HtmlAgilityPack;

namespace Zohan.Funcs
{
    public static class Scrape
    {
        [FunctionName("ProductAvailability")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            var url = Environment.GetEnvironmentVariable("ProductUrl");
            var web = new HtmlWeb();
            var html = await web.LoadFromWebAsync(url);
 
		    var searchString = "Temporarily Out of Stock";
            var node = html.DocumentNode.SelectSingleNode($"//text()[contains(., '{searchString}')]/..");            
            var availabilityResponse = new { available = node == null, productUrl = url};

            return new OkObjectResult(availabilityResponse);
            
        }
    }
}
