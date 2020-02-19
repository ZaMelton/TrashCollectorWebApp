using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TrashCollectorV2.Models
{
    public class Account
    {
        [Key]
        public int Id { get; set; }
        public double Balance { get; set; }
        public bool IsSuspended { get; set; }
        [ForeignKey("Pickup")]
        public int? PickupId { get; set; }
        public Pickup Pickup { get; set; }
    }
}
