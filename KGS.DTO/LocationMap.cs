using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGS.DTO
{
    public class LocationMap
    {
      
            public long Id { get; set; }          
            public string Latitude { get; set; }
            public string Longitude { get; set; }
            public string Name { get; set; }            
            public string Description { get; set; }
            public string ConnectionType { get; set; }

    }
}
