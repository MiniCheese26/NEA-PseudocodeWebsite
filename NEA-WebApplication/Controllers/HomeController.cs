using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp;
using PseudocodeProcessor;

namespace NeaWebApplication.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public ContentResult Index()
        {
            return new ContentResult 
            {
                ContentType = "text/html",
                Content = "<div>Hello World</div>"
            };
        }

        [HttpPost]
        public JsonResult Translate(string code, int languageVersion)
        {
            if (!Enum.IsDefined(typeof(LanguageVersion), languageVersion))
                languageVersion = 0;
            
            if (string.IsNullOrWhiteSpace(code))
                return new JsonResult(new {code = string.Empty, failed = true, messages = new[] {"Submitted code argument was null or empty"}});
            
            var translationProcessor = new PseudocodeProcessor.CSharpProcessor.CSharpProcessor(code, (LanguageVersion) languageVersion);
            IPseudocode pseudocode = translationProcessor.TranslateCode();
            
            return new JsonResult(new {code = pseudocode.TranslatedCode, failed = pseudocode.HasFailed, messages = pseudocode.GetMessages});
        }
    }
}