using EFcoreRepoPractice.Application.DTos;
using EFcoreRepoPractice.Data;
using EFcoreRepoPractice.Infrastructure.repos;
using EFcoreRepoPractice.Models;
using Microsoft.EntityFrameworkCore;

namespace EFcoreRepoPractice.Application.Queries.MemberQueries
{
    public class GetMemberDetailHandler 
    {
        private IUnitOfWork _uow;

        public GetMemberDetailHandler(IUnitOfWork uow) => _uow = uow;

        public async Task<MemberDTO?> GetMemberHandler(GetDetailQueryById q, CancellationToken ct = default)
        {
             

            var entity = _uow.GetRepository<Member>();
            var model = await entity.GetSelectively( x=>  x.MemberId == q.Id)?.FirstOrDefaultAsync(ct);


            return model is null ? null : new MemberDTO(model.MemberId, model.Name, model.Email, model.Age);        


        }


        public async Task<IEnumerable<MemberDTO>?> GetAllMemberHandler(CancellationToken ct = default)
        {


            var entity = _uow.GetRepository<Member>();
            var model = await entity.GetAll().ToListAsync();


            return model is null ? null :  model.Select( model=> new MemberDTO(model!.MemberId, model.Name, model.Email, model.Age));

        }


    }
}
