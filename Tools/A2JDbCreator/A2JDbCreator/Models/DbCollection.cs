using System;
using System.Collections.Generic;
using System.Text;

namespace A2JDbCreator.Models
{
    public class DbCollection
    {
        public string Name { get; set; }
        public string PartitionKey { get; set; }
        public int Throughput { get; set; }
    }
}
