using EFcoreRepoPractice.Application.DTos;
using EFcoreRepoPractice.Data;
using EFcoreRepoPractice.Infrastructure.repos;
using EFcoreRepoPractice.Models;
 

namespace EFcoreRepoPractice.Application.Commands.MemberCommands
{
    public class CreateMemberHandler
    {
        private IUnitOfWork _uow;

        public CreateMemberHandler(IUnitOfWork uow) => _uow = uow;

        public async Task<MemberDTO?> CreateOneMember(CreateMemberCommand q, CancellationToken ct = default)
        {


            var entity = _uow.GetRepository<Member>();
            var member = new Member { Name = q.Name, Email = q.Email, Age = q.Age };


            await _uow.ExecuteTransactionAsync(async () =>
            {

                await entity.CreateAsync(member, ct);
                await entity.Save();

            });




            return new MemberDTO(member.MemberId, member.Name, member.Email, member.Age);


        }

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



    }
}
