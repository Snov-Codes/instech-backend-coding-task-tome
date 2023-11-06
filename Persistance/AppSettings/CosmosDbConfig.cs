using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistance.AppSettings
{
    public class CosmosDbConfig
    {
        public string EndpointUrl { get; set; }
        
        public string PrimaryKey { get; set; }
        
        public string DatabaseName { get; set; }

        public List<ContainerConfig> Containers { get; set; }
    }

    public class ContainerConfig
    {
        public string Name { get; set; }

        public string PartitionKey { get; set; }
    }
}
