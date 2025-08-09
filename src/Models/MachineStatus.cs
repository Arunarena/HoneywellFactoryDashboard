using System;

namespace HoneywellFactoryDashboard.Models
{
    public class MachineStatus
    {
        public int Id { get; set; }
        public string MachineName { get; set; }
        public string Status { get; set; }
        public double OutputPerHour { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}
