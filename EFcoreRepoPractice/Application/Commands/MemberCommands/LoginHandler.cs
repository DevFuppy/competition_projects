using BCrypt.Net;
using EFcoreRepoPractice.Application.DTos;
using EFcoreRepoPractice.Data;
using EFcoreRepoPractice.Infrastructure.repos;
using EFcoreRepoPractice.Models;
using Microsoft.EntityFrameworkCore;


namespace EFcoreRepoPractice.Application.Commands.MemberCommands
{
    public class LoginHandler
    {
        private IUnitOfWork _uow;

        public LoginHandler(IUnitOfWork uow) => _uow = uow;


        public async Task MemberRegistration(RegisterMemberCommand q, CancellationToken ct = default)
        {

            var entity = _uow.GetRepository<Member>();
            var pwd = PasswordHasher.GenerateHashPwd(q.Password);
            var member = new Member { Email = q.Email, Password = pwd };


            await _uow.ExecuteTransactionAsync(() =>
            {

                entity.Create(member);


            });


        }


        public async Task<MemberDTO?> LoginVerification(LoginInfo q, CancellationToken ct = default)
        {
            var entity = _uow.GetRepository<Member>();
            var memberList = entity.GetAll();
            var member = await memberList.FirstOrDefaultAsync(x => x.Email == q.email);

            if (member is null) return null;


            try
            {

                var x = PasswordHasher.VerifyHashPwd(q.pwd, member.Password);

                if (member.Password is null || !PasswordHasher.VerifyHashPwd(q.pwd, member.Password)) return null;

                return new MemberDTO(member.MemberId, member.Name, member.Email, member.Age);

            }
            catch (SaltParseException saltwrong)
            {


                throw new SaltParseException("資料庫密碼無加鹽", saltwrong);

            }

            


        }


    }
}
