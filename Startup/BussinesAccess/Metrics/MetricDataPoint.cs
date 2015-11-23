using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinesAccess.Metrics
{
    public class MetricDataPoint
    {
        #region Public Properties
        public string Entity { get; set; }
        public string Type { get; set; }
        public string Metric { get; set; }
        public string Granularity { get; set; }
        public string FilterOperator1 { get; set; }
        public string FilterValue1 { get; set; }
        public string FilterOperator2 { get; set; }
        public string FilterValue2 { get; set; }
        private bool graph = true;
        public bool Graph
        {
            get { return graph; }
            set { graph = value; }
        }
        #endregion
    }


    public class MetricValue
    {
        #region Public Propeties
        public long Min { get; set; }
        public long Max { get; set; }
        public long Total { get; set; }
        public double Average { get; set; }
        public DateTime Timestamp { get; set; }
        #endregion
    }
}
