using AutoMapper;
using HR.LeaveManagement.MVC.Contracts;
using HR.LeaveManagement.MVC.Models;
using HR.LeaveManagement.MVC.Services.Base;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HR.LeaveManagement.MVC.Services
{
    public class AuthenticationService : BaseHttpService, Contracts.IAuthenticationService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private JwtSecurityTokenHandler _tokenHandler;
        private readonly IMapper _mapper;

        public AuthenticationService(
            IClient client, 
            ILocalStorageService localStorage,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
            :base(client, localStorage)
        {
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _tokenHandler = new JwtSecurityTokenHandler();
        }

        public async Task<bool> Authenticate(string email, string password)
        {
            try
            {
                var authRequest = new AuthRequest
                {
                    Email = email,
                    Password = password
                };

                var authResponse = await _client.LoginAsync(authRequest);

                if (authResponse.Token != string.Empty)
                {
                    var tokenContent = _tokenHandler.ReadJwtToken(authResponse.Token);
                    var claims = ParseClaims(tokenContent);
                    var user = new ClaimsPrincipal(new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme));
                    await _httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, user);
                    _localStorage.SetStorageValue("token", authResponse.Token);

                    return true;
                }

                return false;
            }
            catch
            {
                return false;
            }
        }

        private IList<Claim> ParseClaims(JwtSecurityToken tokenContent)
        {
            var claims = tokenContent.Claims.ToList();
            claims.Add(new Claim(ClaimTypes.Name, tokenContent.Subject));
            return claims;
        }

        public async Task Logout()
        {
            _localStorage.ClearStorage(new List<string>
            {
                "token"
            });

            await _httpContextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        public async Task<bool> Register(RegisterVM register)
        {
            var registrationRequest = _mapper.Map<RegistrationRequest>(register);

            var response = await _client.RegisterAsync(registrationRequest);

            if(!string.IsNullOrEmpty(response.UserId))
            {
                await Authenticate(register.Email, register.Password);
                return true;
            }

            return false;
        }
    }
}
