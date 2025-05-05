using EFcoreRepoPractice.Application.DTos;
using EFcoreRepoPractice.Data;
using EFcoreRepoPractice.Infrastructure.repos;
using EFcoreRepoPractice.Models;

namespace EFcoreRepoPractice.Application.Queries.MemberQueries
{
    public class GetMemberDetailHandler 
    {
        private IUnitOfWork _uow;

        public GetMemberDetailHandler(IUnitOfWork uow) => _uow = uow;

        public async Task<MemberDTO?> GetMemberHandler(GetDetailQueryById q, CancellationToken ct = default)
        {
             

            var entity = _uow.GetRepository<Member>();
            var model = await entity.GetAsync(q.Id, ct);


            return model is null ? null : new MemberDTO(model.MemberId, model.Name, model.Email, model.Age);        


        }


        public async Task<IEnumerable<MemberDTO>?> GetAllMemberHandler(CancellationToken ct = default)
        {


            var entity = _uow.GetRepository<Member>();
            var model = await entity.GetAllAsync(ct);


            return model is null ? null :  model.Select( model=> new MemberDTO(model!.MemberId, model.Name, model.Email, model.Age));

        }


        public async Task<MemberDTO?> LoginVerification(LoginInfo q, CancellationToken ct = default)
        {



            var entity = _uow.GetRepository<Member>();
            var memberList = await entity.GetAllAsync(ct);
            var member =memberList.FirstOrDefault(x => x?.Email == q.email)  ;

            if (member is null || !PasswordHasher.VerifyHashPwd( q.pwd , member.Password) ) return null;



            return new MemberDTO(member.MemberId, member.Name, member.Email, member.Age);

        }


    }
}
