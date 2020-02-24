using System;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp;
using PseudocodeProcessor;

namespace NeaWebApplication.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        { // this seems so wrong
            var path = Path.Combine(Environment.CurrentDirectory, "wwwroot", "index.html");
            return PhysicalFile(path, "text/html");
        }

        [HttpGet]
        public JsonResult Translate(string code, int languageVersion)
        {
            if (!Enum.IsDefined(typeof(LanguageVersion), languageVersion))
                languageVersion = 0;
            
            if (string.IsNullOrWhiteSpace(code))
                return new JsonResult(new {code = string.Empty, failed = true, messages = new[] {"Submitted code argument was null or empty"}});
            
            var translationProcessor = new PseudocodeProcessor.CSharpProcessor.CSharpProcessor(code, (LanguageVersion) languageVersion);
            IPseudocode pseudocode;
            
            try
            {
                pseudocode = translationProcessor.TranslateCode();
            }
            catch (Exception)
            {
                return new JsonResult(new {code = string.Empty, failed = true, messages = new[] {"Fatal translation exception occured"}});
            }
            
            return new JsonResult(new {code = pseudocode.TranslatedCode, failed = pseudocode.HasFailed, messages = pseudocode.GetMessages});
        }
    }
}