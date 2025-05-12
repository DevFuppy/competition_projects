using EFcoreRepoPractice.Application.Commands.MemberCommands;
using EFcoreRepoPractice.Application.DTos;
using EFcoreRepoPractice.Data;
using EFcoreRepoPractice.Models; 
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity.UI.V4.Pages.Account.Internal;
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
        public async Task<MemberDTO?> UpdatePasswordWithTokenAsync(TokenDetailQuery q, CancellationToken ct = default)
        {

            var entity = _unitOfWork.GetRepository<PasswordToken>();
            var existing = (await entity.GetSelectivelyAsync(x => x.Token == q.Token, ct))?.FirstOrDefault();

            if (existing == null)
            {
                return null;
            }

            existing.IsUsed = true;

            await _unitOfWork.ExecuteTransactionAsync(async () =>
            {
                await entity.UpdateSelectiveAsync(existing);
                await entity.Save(ct);

            });

            var member = _unitOfWork.GetRepository<PasswordToken>();


            //return new MemberDTO(existing.MemberId, existing.Name, existing.Email, existing.Age);

        }

    }
}
