using BusinessDatabase.DataObj;
using BusinessDatabase.Dto.Base;
using System.Web.Mvc;


namespace CMS.Filters
{
    public class CustomActionFilter : ActionFilterAttribute, IActionFilter
    {
        void IActionFilter.OnActionExecuting(ActionExecutingContext filterContext)
        {
            var a = (UserDto)filterContext.HttpContext.Session["Account"];
           
          
            this.OnActionExecuting(filterContext);
        }
    }
}