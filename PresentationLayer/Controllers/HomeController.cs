using System.Diagnostics;
using BusinessLogicLayer.Models;
using BusinessLogicLayer.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PresentationLayer.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly ITranslationService _translationService;

    public HomeController(ITranslationService translationService)
    {
        _translationService = translationService;
    }

    [HttpPost]
    public async Task<IActionResult> Translate(string originalText)
    {
        if (string.IsNullOrEmpty(originalText))
        {
            ModelState.AddModelError("originalText", "Please enter the text to be translated.");
            return View("Index");
        }

        try
        {
            var translatedText = await _translationService.TranslateToLeetSpeakAsync(originalText);
            return View("Index", translatedText);
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, "An error occurred while translating: " + ex.Message);
            return View("Index");
        }
    }

    public IActionResult Index()
    {
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
