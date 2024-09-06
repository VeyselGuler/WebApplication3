using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using RestSharp;
using WebApplication3.Models;

namespace WebApplication3.Controllers;

public class HomeController : Controller
{
   
    public IActionResult Index()
    {
        var client = new RestClient("https://dummyjson.com");
        var request = new RestRequest("products");
        var response = client.Execute<ProductListResponse>(request);
        var products = response.Data.Products;
        
        return View(products);
    }

    public IActionResult Details(int id)
    {
        var client = new RestClient("https://dummyjson.com");
        var request = new RestRequest($"products/{id}");
        var response = client.Execute<Product>(request);
        var product = response.Data;
        
        return View(product);
    }

    [HttpGet]
    [Route("/UpdateProduct/{id}")]
    public IActionResult UpdateProduct(int id)
    {
        var client = new RestClient("https://dummyjson.com");
        var request = new RestRequest($"products/{id}");
        var response = client.Execute<Product>(request);
        var product = response.Data;

        if (product == null)
        {
            return NotFound();
        }

        return View(product);
    }

    [HttpPost]
    [Route("/UpdateProduct/{id}")]
    public IActionResult UpdateProduct(int id, Product model, string captchaToken)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        if (!VerifyCaptcha(captchaToken))
        {
            ViewBag.CaptchaError = true;
            return View(model);
        }

        var client = new RestClient("https://dummyjson.com");
        var request = new RestRequest($"products/{id}", Method.Put);
        request.AddJsonBody(model);
        var response = client.Execute<Product>(request);

        if (response.IsSuccessful)
        {
            return RedirectToAction("Index");
        }

        return View(model);
    }

    [HttpGet]
    public IActionResult AddProduct()
    {
        return View();
    }

    [HttpPost]
    public IActionResult AddProduct(Product model, string captchaToken)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        if (!VerifyCaptcha(captchaToken))
        {
            ViewBag.CaptchaError = true;
            return View(model);
        }

        var client = new RestClient("https://dummyjson.com");
        var request = new RestRequest("products/add", Method.Post);
        request.AddJsonBody(model);
        var response = client.Execute<Product>(request);

        if (response.IsSuccessful)
        {
            return RedirectToAction("Index");
        }

        return View(model);
    }

    public bool VerifyCaptcha(string captchaToken)
    {
        var client = new RestClient("https://www.google.com/recaptcha");
        var request = new RestRequest("api/siteverify", Method.Post);
        request.AddParameter("secret", "");
        request.AddParameter("response", captchaToken);

        var response = client.Execute<CaptchaResponse>(request);
        return response.Data.Success && response.Data.Score > 0.6;
    }
}



