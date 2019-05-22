using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using ailogica.Azure.Helpers;
using ailogica.Azure.Helpers.Services;
using DotNetCore.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DotNetCore.Controllers
{
    public class HomeController : Controller
    {
        BlobService imageService = new BlobService();
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AzureBlobStorage()
        {
            
            return View();
        }
        // GET: Image
        public ActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Upload(IFormFile photo)
        {
            var imageUrl = string.Empty;

            var azureHelper = new BlobHelpers("connection-string");
                      
            var filePath = Path.GetTempFileName();
            if (photo.Length > 0)
            {
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await photo.CopyToAsync(stream);
                    var result = await azureHelper.UploadBlob("sampleimage", photo.FileName, stream);
                    imageUrl = result.FileUrl;
                }
            }

            TempData["LatestImage"] = imageUrl.ToString();
            return RedirectToAction("LatestImage");
        }

        public ActionResult LatestImage()
        {
            var latestImage = string.Empty;
            if (TempData["LatestImage"] != null)
            {
                ViewBag.LatestImage = Convert.ToString(TempData["LatestImage"]);
            }

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
