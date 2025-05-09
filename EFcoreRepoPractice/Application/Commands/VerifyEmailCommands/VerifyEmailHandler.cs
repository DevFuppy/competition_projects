using EFcoreRepoPractice.Application.Commands.MemberCommands;
using EFcoreRepoPractice.Application.DTos;
using EFcoreRepoPractice.Data;
using EFcoreRepoPractice.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity.UI.V4.Pages.Account.Internal;
using System;
 

namespace EFcoreRepoPractice.Application.Commands.VerifyEmailCommands
{
    public class VerifyEmailHandler
    {
        private IUnitOfWork _unitOfWork;
        public VerifyEmailHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;


        #region 寄驗證信
        public async Task<dynamic> SendEmailwithTokenAsync(VerifyEmailCommand cmd,string url,CancellationToken ct=default) {

            var repo = _unitOfWork.GetRepository<Member>();
            var MachedEmail = (await repo.GetSelectivelyAsync(x => cmd.Email == x.Email, ct))?.FirstOrDefault();

            if (MachedEmail is null) return null;

           

            EmailSender.SendMail(MachedEmail.Email, url);

            return new { mail=MachedEmail.Email, url};
        }
        #endregion


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
