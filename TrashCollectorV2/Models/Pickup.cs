using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TrashCollectorV2.Models
{
    public class Pickup
    {
        [Key]
        public int Id { get; set; }
        public string PickupDay { get; set; }
        public DateTime OneTimePickup { get; set; }
        public DateTime StartSuspend { get; set; }
        public DateTime EndSuspend { get; set; }
    }
}
