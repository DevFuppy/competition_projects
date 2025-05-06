using EFcoreRepoPractice.Application.DTos;
using EFcoreRepoPractice.Models;

namespace EFcoreRepoPractice.Application.Services
{
    public interface IAuthService
    {
        public Task SignInAsync(MemberDTO member);
    }
}
