using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APICatalogo.Filter
{
    public class APILoggingFilter : IActionFilter
    {

        private readonly ILogger<APILoggingFilter> _logger;

        public APILoggingFilter(ILogger<APILoggingFilter> logger)
        {
            _logger = logger;
        }

        //Executa antes da action
        public void OnActionExecuting(ActionExecutingContext context)
        {
            _logger.LogInformation("Executando antes da action");
        }

        //Executa depois da action
        public void OnActionExecuted(ActionExecutedContext context)
        {
            _logger.LogInformation("Executando depois da action");
        }

        
    }
}
