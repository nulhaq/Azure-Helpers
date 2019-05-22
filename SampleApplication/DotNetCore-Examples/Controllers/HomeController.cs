using System.Diagnostics;
using ailogica.Azure.Helpers;
using DotNetCore.Models;
using Microsoft.AspNetCore.Mvc;

namespace DotNetCore.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AzureBlobStorage()
        {
            ViewData["Message"] = "Your application description page.";

            var azureHelper = new BlobHelpers("DefaultEndpointsProtocol=https;AccountName=dealbargainsresourceg232;AccountKey=5Cu3lyQfocuL59nhGOmMBeRvhMe3Mk77DyZw19J5icw70xP6w17tQi6umGgSke2cS/MS5+z0Cq9eTJFsd4Ja0w==;EndpointSuffix=core.windows.net");
            var uploadBlobs = azureHelper.UploadBlobs("product-images", "uk/naveed.jpg", @"C:\code\Examples\DotNetCore-Examples\naveed.jpg");
            var isExist = azureHelper.BlobExist("product-images", "uk/naveed.jpg", @"C:\code\Examples\DotNetCore-Examples\naveed.jpg");
            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
