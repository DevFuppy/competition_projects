using EFcoreRepoPractice.Application.DTos;
using EFcoreRepoPractice.Data;
using EFcoreRepoPractice.Infrastructure.repos;
using EFcoreRepoPractice.Models;
 

namespace EFcoreRepoPractice.Application.Commands.MemberCommands
{
    public class UpdateMemberHandler
    {
        private IUnitOfWork _uow;

        public UpdateMemberHandler(IUnitOfWork uow) => _uow = uow;

        public async Task<MemberDTO?> UpdateOneMember(UpdateMemberCommand q, CancellationToken ct = default)
        {


            var entity = _uow.GetRepository<Member>();
            var existing = await entity.GetAsync(q.Id);

            if (existing == null)
            {
                return null;
            }
             

            existing.Name = q.Name;
            existing.Email = q.Email;
            existing.Age = q.Age;


            await _uow.ExecuteTransactionAsync(async () =>
            {
                await entity.UpdateAsync(existing);
                await entity.Save(ct);

            });

 

            return new MemberDTO(existing.MemberId, existing.Name, existing.Email, existing.Age);

        }
 


    }
}
