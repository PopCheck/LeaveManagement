using AutoMapper;
using HR.LeaveManagement.MVC.Contracts;
using HR.LeaveManagement.MVC.Models;
using HR.LeaveManagement.MVC.Services.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HR.LeaveManagement.MVC.Services
{
    public class LeaveRequestService : BaseHttpService, ILeaveRequestService
    {
        private readonly IMapper _mapper;

        public LeaveRequestService(IMapper mapper, IClient client, ILocalStorageService localStorageService)
            :base(client, localStorageService)
        {
            _mapper = mapper;
        }

        public async Task ApproveLeaveRequest(int id, bool approved)
        {
            AddBearerToken();

            try
            {
                var request = new ChangeLeaveRequestApprovalDto
                {
                    Approved = approved,
                    Id = id
                };

                await _client.ChangeapprovalAsync(id, request);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<Response<int>> CreateLeaveRequest(CreateLeaveRequestVM leaveRequest)
        {
            try
            {
                var response = new Response<int>();
                CreateLeaveRequestDto createLeaveRequest = _mapper.Map<CreateLeaveRequestDto>(leaveRequest);
                AddBearerToken();
                var apiResponse = await _client.LeaveRequestsPOSTAsync(createLeaveRequest);

                response.Success = apiResponse.Success;
                response.ValidationErrors = apiResponse.Errors == null ? string.Empty :
                    string.Join(Environment.NewLine, apiResponse.Errors);


                return response;
            }
            catch (ApiException ex)
            {
                return ConvertApiExceptions<int>(ex);
            }
        }

        public Task DeleteLeaveRequest(int id)
        {
            throw new System.NotImplementedException();
        }

        public async Task<AdminLeaveRequestViewVM> GetAdminLeaveRequestList()
        {
            AddBearerToken();

            var leaveRequests = await _client.LeaveRequestsAllAsync(isLoggedInUser: false);

            var model = new AdminLeaveRequestViewVM
            {
                TotalRequests = leaveRequests.Count,
                ApprovedRequests = leaveRequests.Count(x => x.Approved == true),
                PendingRequests = leaveRequests.Count(x => x.Approved == null),
                RejectedRequests = leaveRequests.Count(x => x.Approved == false),
                LeaveRequests = _mapper.Map<List<LeaveRequestVM>>(leaveRequests)
            };

            return model;
        }

        public async Task<LeaveRequestVM> GetLeaveRequest(int id)
        {
            AddBearerToken();

            var leaveRequest = await _client.LeaveRequestsGETAsync(id);
            return _mapper.Map<LeaveRequestVM>(leaveRequest);
        }

        public async Task<EmployeeLeaveRequestViewVM> GetUserLeaveRequests()
        {
            AddBearerToken();

            var leaveRequests = await _client.LeaveRequestsAllAsync(isLoggedInUser: true);
            var allocations = await _client.LeaveAllocationsAllAsync(isLoggedInUser: true);

            var model = new EmployeeLeaveRequestViewVM
            {
                LeaveAllocations = _mapper.Map<List<LeaveAllocationVM>>(allocations),
                LeaveRequests = _mapper.Map<List<LeaveRequestVM>>(leaveRequests)
            };

            return model;
        }
    }
}
