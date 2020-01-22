using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;

namespace Claims.AuthorizationAttributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class YearsWorkedAttribute : TypeFilterAttribute
    {
        public YearsWorkedAttribute(int years) : base(typeof(YearsWorkedFilter))
        {
            Arguments = new object[] { years };
        }
    }

    public class YearsWorkedFilter : IAuthorizationFilter
    {
        public YearsWorkedFilter(int years) => Years = years;

        public int Years { get; set; }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            // If the user is not authenticated, create a new UnauthorizedResult
            if (!context.HttpContext.User.Identity.IsAuthenticated)
                context.Result = new UnauthorizedResult();

            try
            {
                var started = context.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "DateStarted").Value;
                var dateStarted = DateTime.Parse(started);

                // If the user hasn't been around for at least X amount of years, create a new UnauthorizedResult
                if (DateTime.Now.Subtract(dateStarted).TotalDays < 365 * Years)
                    context.Result = new UnauthorizedResult();
            }
            catch (Exception ex)
            {
                context.Result = new UnauthorizedResult();
            }
            
        }
    }
}
