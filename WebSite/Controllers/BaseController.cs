using System.Web.Mvc;

namespace WebSite.Controllers
{
    public class BaseController : Controller
    {
        protected AuctionEntities DbContext { get; }



        public BaseController()
        {
            this.DbContext = new AuctionEntities();
        }
    }
}