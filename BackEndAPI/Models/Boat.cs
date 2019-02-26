using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RegistrationAPI.Models
{
    public class Boat
    {
        [Key]
        public int BoatId { get; set; }
        public string BoatName { get; set; }

        [Column(TypeName = "varchar(MAX)")]
        public string Picture { get; set; }
        public int LengthInFeet { get; set; }
        public string Make { get; set; }
        public string Description { get; set; }
    }
}
