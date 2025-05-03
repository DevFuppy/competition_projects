using EFcoreRepoPractice.Application.DTos;
using EFcoreRepoPractice.Data;
using EFcoreRepoPractice.Infrastructure.repos;
using EFcoreRepoPractice.Models;

namespace EFcoreRepoPractice.Application.Queries
{
    public class GetMemberDetailHandler 
    {
        private IUnitOfWork _uow;

        public GetMemberDetailHandler(IUnitOfWork uow) => _uow = uow;

        public async Task<MemberDTO?> GetMemberHandler(GetDetailQueryById  q, CancellationToken ct = default)
        {
             

            var entity = _uow.GetRepository<Member>();
            var model = await entity.GetAsync(q.Id, ct);


            return model is null ? null : new MemberDTO(model.MemberId, model.Name, model.Email);        


        }

    }
}
