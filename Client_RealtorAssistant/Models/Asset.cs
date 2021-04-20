using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Client_RealtorAssistant.Models
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

        [DisplayName("Sold")]
        public bool isSold { get; set; }

        public virtual Realtors Realtor { get; set; }
    }
}
