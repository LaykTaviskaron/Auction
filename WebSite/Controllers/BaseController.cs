using System.Web.Mvc;
using WebSite.Models;
using Microsoft.AspNet.Identity.Owin;
using System.Web;

namespace WebSite.Controllers
{
    public class BaseController : Controller
    {
        protected ApplicationDbContext DbContext
        {
            get
            {
                return System.Web.HttpContext.Current.GetOwinContext().Get<ApplicationDbContext>();
            }
        }

        public BaseController()
        {
        }
    }
}