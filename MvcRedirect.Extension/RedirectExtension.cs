using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection; 
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace MvcRedirect.Extension
{
    public static class RedirectExtension
    {
        public static IActionResult RedirectTo<TController>(this Controller controller,
            Expression<Action<TController>> redirectAction)
        {
            if (redirectAction?.NodeType != ExpressionType.Lambda)
            {
                throw new InvalidOperationException(nameof(redirectAction));
            }

            var methodExpression = (redirectAction.Body as MethodCallExpression);
            var methodInfo = methodExpression?.Method;

            var controllerName = typeof(TController).Name.Replace("Controller", "");
            var actionName = GetValidActionName(methodInfo);

            var routeDictionary = UnwrapRouteDictionary(methodExpression, methodInfo);
            return routeDictionary == null
                ? controller.RedirectToAction(actionName, controllerName)
                : controller.RedirectToAction(actionName, controllerName, routeDictionary);
        }

        private static string GetValidActionName(MemberInfo methodInfo)
        {
            var attribute = methodInfo?.GetCustomAttributes()
                .OfType<ActionNameAttribute>()
                .FirstOrDefault();

            var actionName = attribute?.Name ?? methodInfo?.Name;
            return actionName;
        }

        private static RouteValueDictionary UnwrapRouteDictionary(MethodCallExpression methodExpression,
            MethodBase methodInfo)
        {
            var routeDictionary = new RouteValueDictionary();
            var methodParameters = methodExpression?.Arguments;
            var methodArguments = methodInfo?.GetParameters().Select(a => a.Name).ToList();
            
            for (var i = 0; i < methodParameters?.Count; i++)
            {
                var parameter = methodParameters[i];
                var argument = methodArguments![i];

                switch (parameter)
                {
                    case ConstantExpression constantParameterExpression:
                        routeDictionary.TryAdd(argument!, constantParameterExpression.Value!);
                        break;
                    case MemberExpression memberExpression:
                    {
                        var constantExpression = memberExpression.Expression as ConstantExpression;
                        var field = constantExpression?.Value?.GetType().GetField(memberExpression.Member.Name);
                        var fieldValue = field?.GetValue(constantExpression.Value)!;
                        routeDictionary.TryAdd(argument!, fieldValue!);
                        break;
                    }
                }
            }

            return routeDictionary;
        }
    }
}