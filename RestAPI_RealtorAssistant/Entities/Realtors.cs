using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RestAPI_RealtorAssistant.Entities
{
    public partial class Realtors
    {
        public Realtors()
        {
            Asset = new HashSet<Asset>();
        }

        [Key]
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }

        public virtual ICollection<Asset> Asset { get; set; }
    }
}
