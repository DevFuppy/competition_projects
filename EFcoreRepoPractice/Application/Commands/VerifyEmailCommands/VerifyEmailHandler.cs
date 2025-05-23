using EFcoreRepoPractice.Application.Commands.MemberCommands;
using EFcoreRepoPractice.Application.DTos;
using EFcoreRepoPractice.Data;
using EFcoreRepoPractice.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity.UI.V4.Pages.Account.Internal;
using Microsoft.EntityFrameworkCore;
using System;
using System.Web;
using static EFcoreRepoPractice.Application.Queries.EmailQueries.EmailDetailQuery;


namespace EFcoreRepoPractice.Application.Commands.VerifyEmailCommands
{
    public class VerifyEmailHandler
    {
        private IUnitOfWork _unitOfWork;
        public VerifyEmailHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;


        /// <summary>
        /// 寄驗證信
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="url"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<dynamic> SendEmailwithTokenAsync(VerifyEmailCommand cmd, string url, CancellationToken ct = default)
        {

            var repo = _unitOfWork.GetRepository<Member>();
            var MachedMember = (repo.GetSelectively(x => cmd.Email == x.Email))?.FirstOrDefault();

            if (MachedMember is null) return null;


            var repo2 = _unitOfWork.GetRepository<PasswordToken>();

            await _unitOfWork.ExecuteTransactionAsync(
                 () =>
                {
                    repo2.Create(new PasswordToken
                    {
                        MemberId = MachedMember.MemberId,
                        Email = MachedMember.Email,
                        ExpireAt = DateTime.Now.AddMinutes(20),
                        //Token = url.Substring(url.IndexOf("=") + 1),
                        //Token = url.Substring(url.Length-36),
                        Token = (HttpUtility.ParseQueryString((new Uri(url)).Query))["token"],
                        IsUsed = false
                    });

                }

                );


            await EmailSender.SendMail(MachedMember.Email, url);

            return new { mail = MachedMember.Email, url };
        }


        /// <summary>
        /// 更新Token使用狀況
        /// </summary>
        /// <param name="q"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<dynamic> UpdatePasswordWithTokenAsync(UpdatePasswordViewModel up, CancellationToken ct = default)
        {

            var TokenRepo = _unitOfWork.GetRepository<PasswordToken>();
            var MemberRepo = _unitOfWork.GetRepository<Member>();


            var TheTokenEntity = TokenRepo.GetSelectively(x => x.Token == up.Token);

            if (TheTokenEntity is null) return "認證出錯";

            var member = TheTokenEntity.Join(MemberRepo.GetAll(), te => te.Email, mem => mem.Email, (te, mem) => mem);
                        
            PasswordToken TheToken = await TheTokenEntity.FirstOrDefaultAsync();
            

            if (member is null) return "找不到使用者";
            if (TheToken.IsUsed is true) return "連結已使用" ;
            if (TheToken.ExpireAt <= DateTime.Now) return "連結已過期";

          
            TheToken.IsUsed = true;

            var TheMember = await member.FirstAsync();
            TheMember.Password = PasswordHasher.GenerateHashPwd(up.Password);

            await _unitOfWork.ExecuteTransactionAsync(

                () =>
                {
                    TokenRepo.UpdateSelective(TheToken);
                    MemberRepo.UpdateSelective(TheMember);

                });

 
            return new MemberDTO(TheMember.MemberId, TheMember.Name, TheMember.Email, TheMember.Age, TheMember.Password);

        }





    }
}
