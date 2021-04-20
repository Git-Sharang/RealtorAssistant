using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RestAPI_RealtorAssistant.Entities
{
    public partial class Asset
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Type { get; set; }
        public bool isOwned { get; set; }
        public int RealtorId { get; set; }
        public bool isSold { get; set; }

        public virtual Realtors Realtor { get; set; }
    }
}
