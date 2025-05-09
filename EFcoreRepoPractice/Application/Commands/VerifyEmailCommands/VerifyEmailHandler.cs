using EFcoreRepoPractice.Application.Commands.MemberCommands;
using EFcoreRepoPractice.Application.DTos;
using EFcoreRepoPractice.Data;
using EFcoreRepoPractice.Models; 
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity.UI.V4.Pages.Account.Internal;
using System;
using System.Web;


namespace EFcoreRepoPractice.Application.Commands.VerifyEmailCommands
{
    public class VerifyEmailHandler
    {
        private IUnitOfWork _unitOfWork;
        public VerifyEmailHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;


        #region 寄驗證信
        public async Task<dynamic> SendEmailwithTokenAsync(VerifyEmailCommand cmd, string url, CancellationToken ct = default)
        {

            var repo = _unitOfWork.GetRepository<Member>();
            var MachedMember = (await repo.GetSelectivelyAsync(x => cmd.Email == x.Email, ct))?.FirstOrDefault();

            if (MachedMember is null) return null;


            var repo2 = _unitOfWork.GetRepository<PasswordToken>();

            await _unitOfWork.ExecuteTransactionAsync(
                async () =>
                {
                    await repo2.CreateAsync(new PasswordToken
                    {
                        MemberId = MachedMember.MemberId,
                        Email = MachedMember.Email,
                        ExpireAt = DateTime.Now.AddMinutes(20),
                        //Token = url.Substring(url.IndexOf("=") + 1),
                        //Token = url.Substring(url.Length-36),
                        Token = (HttpUtility.ParseQueryString((new Uri(url)).Query))["token"],
                        IsUsed = false
                    });
                    await repo2.Save();
                }

                );


            EmailSender.SendMail(MachedMember.Email, url);

            return new { mail = MachedMember.Email, url };
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
