using System;
using System.Threading.Tasks;

namespace HR.LeaveManagement.Application.Contracts.Persistence
{
    public interface IUnitOfWork : IDisposable
    {
        public ILeaveAllocationRepository LeaveAllocationRepository { get; }
        public ILeaveRequestRepository LeaveRequestRepository { get; }
        public ILeaveTypeRepository LeaveTypeRepository { get; }
        Task Save();
    }
}
