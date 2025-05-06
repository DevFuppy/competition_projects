using EFcoreRepoPractice.Application.DTos;
using EFcoreRepoPractice.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace EFcoreRepoPractice.Application.Services
{
    public class AuthService : IAuthService
    {
        IHttpContextAccessor _acc;

        public AuthService(IHttpContextAccessor acc) => _acc = acc;

        public async Task SignInAsync(MemberDTO member)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, member.Id.ToString()),
                new Claim(ClaimTypes.Name, member.Email??"使用者無email")
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await _acc.HttpContext!.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,principal);

        }

    }
}
