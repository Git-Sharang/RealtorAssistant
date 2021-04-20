using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RestAPI_RealtorAssistant.Models
{
    public class AssetForCreationDto
    {

        [Required(ErrorMessage = "You should provide a name")]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required(ErrorMessage = "You should provide an address")]
        [MaxLength(600)]
        public string Address { get; set; }

        [Required(ErrorMessage = "Specify the type of the Asset")]
        [MaxLength(50)]
        public string Type { get; set; }

        [Required(ErrorMessage = "Specify if you own the Asset")]
        public bool isOwned { get; set; }

        [Required(ErrorMessage = "Specify if the asset is sold")]
        public bool isSold { get; set; }
    }
}
