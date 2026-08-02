using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;

namespace StudentPortalWeb.Constraints
{
    public class IntakeCodeConstraint : IRouteConstraint
    {
        public bool Match(
            HttpContext? httpContext,
            IRouter? route,
            string routeKey,
            RouteValueDictionary values,
            RouteDirection routeDirection)
        {
            if (!values.TryGetValue(routeKey, out var value) || value is null)
            {
                return false;
            }


            return string.Equals(
                value.ToString(),
                "itiB",
                StringComparison.OrdinalIgnoreCase);
        }
    }
}