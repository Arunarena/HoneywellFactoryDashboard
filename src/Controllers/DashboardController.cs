using System.Linq;
using Microsoft.AspNetCore.Mvc;
using HoneywellFactoryDashboard.Services;

namespace HoneywellFactoryDashboard.Controllers
{
    public class DashboardController : Controller
    {
        private readonly DataService _dataService;

        public DashboardController(DataService dataService)
        {
            _dataService = dataService;
        }

        public IActionResult Index()
        {
            var machines = _dataService.GetMachineStatuses();
            return View(machines);
        }

        public IActionResult GetLiveData()
        {
            var machines = _dataService.GetMachineStatuses()
                .Select(m => new {
                    machineName = m.MachineName,
                    status = m.Status,
                    outputPerHour = m.OutputPerHour,
                    lastUpdated = m.LastUpdated
                });
            return Json(machines);
        }

        public IActionResult GetTrendData()
        {
            var trend = _dataService.GetProductionHistory()
                .Select(t => new { time = t.Item1.ToString("HH:mm:ss"), output = t.Item2 })
                .ToList();
            return Json(trend);
        }
    }
}
