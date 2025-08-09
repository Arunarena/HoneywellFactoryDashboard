using System;

namespace HoneywellFactoryDashboard.Models
{
    public class ProductionReport
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public int QuantityProduced { get; set; }
        public DateTime ReportDate { get; set; }
    }
}
