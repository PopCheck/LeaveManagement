using System.Collections.Generic;

namespace HR.LeaveManagement.MVC.Models
{
    public class ViewLeaveAllocationsVM
    {
        public string EmployeeId { get; set; }
        public List<LeaveAllocationVM> LeaveAllocations { get; set; }
    }
}
