using EFcoreRepoPractice.Application.Commands.MemberCommands;
using EFcoreRepoPractice.Application.DTos;
using EFcoreRepoPractice.Data;
using EFcoreRepoPractice.Models;
using Microsoft.AspNetCore.Identity.UI.V4.Pages.Account.Internal;

namespace EFcoreRepoPractice.Application.Commands.EmailCommands
{
    public class EmailHandler
    {
        IUnitOfWork _unitOfWork;
        EmailHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        //public async Task<MemberDTO> VerifyTokenUpdatingPassword(UpdateMemberCommand cmd)
        //{

        //    //先假設token是對的           

        //    //var newTokenEntity = new PasswordToken
        //    //{
        //    //    Email = fpm.Email,
        //    //    Token = new Guid().ToString(),
        //    //    ExpireAt = DateTime.Now.AddMinutes(30),
        //    //};

        //    var member = new Member
        //    {
        //        Password = cmd.Password,
        //    };

        //    var repo = _unitOfWork.GetRepository<Member>();            ;

        //    await _unitOfWork.ExecuteTransactionAsync(async () =>
        //    {

        //        await repo.UpdateAsync(member);
        //        await repo.Save();

        //    });

        //    return

        //}

    }
}
