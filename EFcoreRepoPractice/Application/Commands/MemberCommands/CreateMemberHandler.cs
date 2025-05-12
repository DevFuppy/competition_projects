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


            await _uow.ExecuteTransactionAsync(() => entity.Create(member));


            return new MemberDTO(member.MemberId, member.Name, member.Email, member.Age);


        }



    }
}
