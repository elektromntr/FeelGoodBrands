using Logic.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class LanguageController : Controller
    {
        public void SetSessionLanguage(string language) => 
            HttpContext.Session.SetString(LanguageExtension.SessionLanguageKey(),
                LanguageExtension.GetLanguage(language).ToString());
    }
}
