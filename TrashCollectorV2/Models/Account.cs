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
        [Display(Name = "Service Suspended")]
        public bool IsSuspended { get; set; }

        [Display(Name = "Pickup Day")]
        public DayOfWeek PickupDay { get; set; }
        public DateTime NextPickupDate { get; set; }
        public bool PickedUp { get; set; }

        [Display(Name = "One Time Pickup")]
        public DateTime OneTimePickup { get; set; }

        [Display(Name = "Start Suspend")]
        public DateTime StartSuspend { get; set; }

        [Display(Name = "End Suspend")]
        public DateTime EndSuspend { get; set; }
    }
}
