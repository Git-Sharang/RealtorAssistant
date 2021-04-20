using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RestAPI_RealtorAssistant.Models
{
    public class RealtorForCreationDto
    {
        [Required(ErrorMessage = "You should provide a name value.")]
        [MaxLength(50)]
        public string Username { get; set; }

        [Required(ErrorMessage = "You should provide an address")]
        [MaxLength(50)]
        public string Password { get; set; }

        [Required(ErrorMessage = "You should provide a type")]
        [MaxLength(50)]
        public string Firstname { get; set; }

        [Required(ErrorMessage = "You should provide a type")]
        [MaxLength(50)]
        public string Lastname { get; set; }
    }
}
