using System;
using System.Collections.Generic;
using System.Linq;
using HoneywellFactoryDashboard.Models;

namespace HoneywellFactoryDashboard.Services
{
    public class DataService
    {
        private readonly AppDbContext _context;
        private static List<(DateTime, int)> _productionHistory = new();

        public DataService(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<MachineStatus> GetMachineStatuses()
        {
            var rand = new Random();
            foreach (var machine in _context.MachineStatuses)
            {
                machine.OutputPerHour += rand.Next(-5, 6);
                machine.OutputPerHour = Math.Max(machine.OutputPerHour, 0);
                machine.LastUpdated = DateTime.Now;

                machine.Status = machine.OutputPerHour == 0 ? "Idle" : "Running";
            }

            var totalOutput = _context.MachineStatuses.Sum(m => (int)m.OutputPerHour);
            _productionHistory.Add((DateTime.Now, totalOutput));

            if (_productionHistory.Count > 20)
                _productionHistory.RemoveAt(0);

            _context.SaveChanges();
            return _context.MachineStatuses.ToList();
        }

        public List<(DateTime, int)> GetProductionHistory()
        {
            return _productionHistory;
        }

        public void SeedData()
        {
            if (!_context.MachineStatuses.Any())
            {
                _context.MachineStatuses.AddRange(
                    new MachineStatus { MachineName = "Cutter-01", Status = "Running", OutputPerHour = 120, LastUpdated = DateTime.Now },
                    new MachineStatus { MachineName = "Oven-03", Status = "Idle", OutputPerHour = 0, LastUpdated = DateTime.Now },
                    new MachineStatus { MachineName = "Packager-02", Status = "Running", OutputPerHour = 85, LastUpdated = DateTime.Now }
                );
                _context.ProductionReports.AddRange(
                    new ProductionReport { ProductName = "Bread Loaf", QuantityProduced = 500, ReportDate = DateTime.Today },
                    new ProductionReport { ProductName = "Bagel", QuantityProduced = 200, ReportDate = DateTime.Today }
                );
                _context.SaveChanges();
            }
        }
    }
}
