using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.AspNetCore.Mvc;
using MvcCoreInsights.Models;
using System.Diagnostics;

namespace MvcCoreInsights.Controllers
{
    public class HomeController : Controller
    {
        private TelemetryClient telemetryClient;
        public HomeController(TelemetryClient telemetryClient)
        {
            this.telemetryClient = telemetryClient;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Index(string usuario,int cantidad)
        {
            ViewData["MENSAJE"] = "Su donativo de " + cantidad + "€ ha sido aceptado." +
                "Muchas Gracias Sr/Sra" + usuario;
            this.telemetryClient.TrackEvent("DonativosRequest");
            MetricTelemetry metric = new MetricTelemetry();
            metric.Name = "Donativos";
            metric.Sum = cantidad;
            this.telemetryClient.TrackMetric(metric);
            string mensaje = "";
            SeverityLevel level;
            if(cantidad == 0)
            {
                level = SeverityLevel.Error;
            }
            else if(cantidad<=5)
            {
                level = SeverityLevel.Critical;
            }
            else if(cantidad <= 20){
                level = SeverityLevel.Warning;
            }
            else
            {
                level = SeverityLevel.Information;
            }
            mensaje = usuario + " " + cantidad + "€";
            TraceTelemetry traza = 
                new TraceTelemetry(mensaje, level);
            this.telemetryClient.TrackTrace(traza);
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}
