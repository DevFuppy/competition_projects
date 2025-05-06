using EFcoreRepoPractice.Application.DTos;
using EFcoreRepoPractice.Data;
using EFcoreRepoPractice.Infrastructure.repos;
using EFcoreRepoPractice.Models;
 

namespace EFcoreRepoPractice.Application.Commands.MemberCommands
{
    public class LoginHandler
    {
        private IUnitOfWork _uow;

        public LoginHandler(IUnitOfWork uow) => _uow = uow;
        

        public async Task  MemberRegistration(RegisterMemberCommand q, CancellationToken ct = default)
        {

            var entity = _uow.GetRepository<Member>();
            var pwd =  PasswordHasher.GenerateHashPwd(q.Password);
            var member = new Member {  Email = q.Email, Password = pwd };


            await _uow.ExecuteTransactionAsync(async () =>
            {

                await entity.CreateAsync(member, ct);
                await entity.Save();

            });           


        }


        public async Task<MemberDTO?> LoginVerification(LoginInfo q, CancellationToken ct = default)
        {



            var entity = _uow.GetRepository<Member>();
            var memberList = await entity.GetAllAsync(ct);
            var member = memberList.FirstOrDefault(x => x?.Email == q.email);

            if (member is null || !PasswordHasher.VerifyHashPwd(q.pwd, member.Password)) return null;



            return new MemberDTO(member.MemberId, member.Name, member.Email, member.Age);

        }

    }
}
