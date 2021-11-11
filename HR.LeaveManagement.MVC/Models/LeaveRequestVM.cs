using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HR.LeaveManagement.MVC.Models
{
    public class LeaveRequestVM : CreateLeaveRequestVM
    {
        public int Id { get; set; }

        [Display(Name = "Date Requested")]
        public DateTime DateRequested { get; set; }

        [Display(Name = "Date Actioned")]
        public DateTime DateActioned { get; set; }

        [Display(Name = "Approved State")]
        public bool? Approved { get; set; }
        public bool Cancelled { get; set; }
        public LeaveTypeVM LeaveType { get; set; }
        public EmployeeVM Employee { get; set; }
    }
}
