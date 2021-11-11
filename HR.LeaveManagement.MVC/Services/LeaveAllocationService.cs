using HR.LeaveManagement.MVC.Contracts;
using HR.LeaveManagement.MVC.Services.Base;
using System;
using System.Threading.Tasks;

namespace HR.LeaveManagement.MVC.Services
{
    public class LeaveAllocationService : BaseHttpService, ILeaveAllocationService
    {
        private readonly ILocalStorageService _localStorageService;
        private readonly IClient _client;

        public LeaveAllocationService(ILocalStorageService localStorageService, IClient client): base(client, localStorageService)
        {
            _localStorageService = localStorageService;
            _client = client;
        }

        public async Task<Response<int>> CreateLeaveAllocations(int leaveTypeId)
        {
            try
            {
                var response = new Response<int>();
                CreateLeaveAllocationDto createLeaveAllocationDto = new CreateLeaveAllocationDto
                {
                    LeaveTypeId = leaveTypeId
                };

                AddBearerToken();

                var apiResponse = await _client.LeaveAllocationsPOSTAsync(createLeaveAllocationDto);

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
    }
}
