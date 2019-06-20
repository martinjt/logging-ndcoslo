using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using logging_oslo.Models;
using Microsoft.Extensions.Logging;
using App.Metrics;
using App.Metrics.Counter;

namespace logging_oslo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IMetrics _metrics;

        private CounterOptions _counterOptions = new CounterOptions {
            MeasurementUnit = Unit.Calls,
            Name = "Home Counter",
            ResetOnReporting = true
        };

        public HomeController(ILogger<HomeController> logger, IMetrics metrics)
        {
            _logger = logger;
            _metrics = metrics;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Log()
        {
            _logger.LogInformation("HELLO NDC OSLO!");
            
            _logger.LogCritical(
                new ArgumentException("Stick to questions, not comments", "Comment"), 
                "Q&A Instruction Violation");

            _logger.LogInformation("Log Request {@params}", Request.Query.ToDictionary(q => q.Key, q => q.Value));

            return Ok("Logged");
        }

        public IActionResult Increment(string tag = null)
        {
            var tags = new MetricTags("userTag", string.IsNullOrEmpty(tag) ? "undefined" : tag);
            _metrics.Measure.Counter.Increment(_counterOptions, tags);
            return Ok("done");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
