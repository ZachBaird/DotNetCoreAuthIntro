using Microsoft.AspNetCore.Authorization;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Claims.PolicyHandlers
{
    public class MinimumYearsWorkedRequirement : IAuthorizationRequirement
    {
        public int Years { get; }

        public MinimumYearsWorkedRequirement(int yearsWorked)
        {
            Years = yearsWorked;
        }
    }

    public class YearsWorkedHandler : AuthorizationHandler<MinimumYearsWorkedRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MinimumYearsWorkedRequirement requirement)
        {
            // If the user is not authenticated, return.
            if (!context.User.Identity.IsAuthenticated)
                return Task.CompletedTask;

            try
            {
                // Grab the date started value from the user context. Prase the date into a DateTime object.
                var started = context.User.Claims.FirstOrDefault(x => x.Type == "DateStarted").Value;
                var dateStarted = DateTime.Parse(started);

                // If the dateStarted satisfies the claim requirement, call .Succeed() on the user context.
                if (DateTime.Now.Subtract(dateStarted).TotalDays > 365 * requirement.Years)
                    context.Succeed(requirement);
            }
            catch(Exception ex)
            {
                
            }

            return Task.CompletedTask;
        }
    }
}
