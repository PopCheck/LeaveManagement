using System;
using System.ComponentModel.DataAnnotations;

namespace HR.LeaveManagement.MVC.Models
{
    public class LeaveAllocationVM
    {
        public int Id { get; set; }

        [Display(Name = "Number of Days")]
        public int NumberOfDays { get; set; }
        public DateTime DateCreated { get; set; }
        public int Period { get; set; }
    }
}
