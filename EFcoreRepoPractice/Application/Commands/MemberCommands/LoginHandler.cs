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

            if (member is null || member.Password is null|| !PasswordHasher.VerifyHashPwd(q.pwd, member.Password)) return null;

            return new MemberDTO(member.MemberId, member.Name, member.Email, member.Age);

        }

        public async Task<MemberDTO?> UpdatePasswordAsync(UpdateMemberCommand q, CancellationToken ct = default)
        {

            var entity = _uow.GetRepository<Member>();
            var existing = await entity.GetByIdAsync(q.Id);

            if (existing == null)
            {
                return null;
            }

            existing.Name = q.Name;
            existing.Email = q.Email;
            existing.Age = q.Age;
            existing.Password = q.Password;
            
            await _uow.ExecuteTransactionAsync(async () =>
            {
                await entity.UpdateSelectiveAsync(existing);
                await entity.Save(ct);

            });

            return new MemberDTO(existing.MemberId, existing.Name, existing.Email, existing.Age);

        }


    }
}
