using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.ActionFilters
{


    /**
     * 
     * [ActionFilter] -> Bir action'ın çalışmasını engelleyebilir, bir action'ın çalışmasından önce veya sonra bir işlem yapabiliriz.
     * 
     * -> IActionFilter
     * -> IAsyncActionFilter
     * -> ActionFilterAttribute
     * 
     * 
     */
    public class ValidationFilterAttribute : ActionFilterAttribute
    {

        // Motot çalışmadan hemen önce çalışır 
        public override void OnActionExecuting(ActionExecutingContext context)
        {

            var controller = context.RouteData.Values["controller"];
            var action = context.RouteData.Values["action"];

            // Parametlerin içerisinde Dto olan parametreyi bul
            var param = context.ActionArguments.SingleOrDefault(p => p.Value.ToString().Contains("Dto")).Value;

            if (param is null)
            {
                context.Result = new BadRequestObjectResult($"Object is null. Controller: {controller}, action: {action}");
                return;
            }
            if (!context.ModelState.IsValid)
            {
                context.Result = new UnprocessableEntityObjectResult(context.ModelState); 
            }

        }
    }
}
