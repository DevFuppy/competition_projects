using EFcoreRepoPractice.Application.DTos;
using EFcoreRepoPractice.Data;
using EFcoreRepoPractice.Infrastructure.repos;
using EFcoreRepoPractice.Models;
using Humanizer;

namespace EFcoreRepoPractice.Application.Commands.MemberCommands
{
    public class DeleteMemberHandler
    {
        private IUnitOfWork _uow;

        public DeleteMemberHandler(IUnitOfWork uow) => _uow = uow;

        public async Task<MemberDTO?> DeleteOneMember(DeleteMemberCommand  q, CancellationToken ct = default)
        {

             
            var entity = _uow.GetRepository<Member>();
            var existing = await entity.GetAsync(q.Id, ct);

            if (existing == null)
            {
                return null;
            }


            await _uow.ExecuteTransactionAsync(async () =>
            {

                await entity.DeleteAsync(existing);
                await entity.Save(ct);

            });


                                                             /*也可以在DTO就?設定nullable*/
            return new MemberDTO(existing.MemberId, existing.Name, existing.Email?? "" );
        }


       


    }
}
