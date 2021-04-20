using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestAPI_RealtorAssistant.Models
{
    public class RealtorDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }

        public int NumberOfAsset
        {
            get
            {
                return Asset.Count;
            }
        }

        public ICollection<AssetDto> Asset { get; set; }
        = new List<AssetDto>();
    }
}
