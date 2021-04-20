using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestAPI_RealtorAssistant.Models
{
    public class AssetDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Type { get; set; }
        public bool isOwned { get; set; }
        public bool isSold { get; set; }
    }
}
