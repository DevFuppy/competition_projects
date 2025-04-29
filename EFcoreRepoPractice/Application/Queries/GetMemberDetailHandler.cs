using EFcoreRepoPractice.Application.DTos;
using EFcoreRepoPractice.Infrastructure.repos;

namespace EFcoreRepoPractice.Application.Queries
{
    public class GetMemberDetailHandler
    {
        private readonly IMemberRepository _repo;

        public GetMemberDetailHandler(IMemberRepository repo) =>_repo =repo;

        public async Task<MemberDTO?> Handler(GetMemberDetailQuery q, CancellationToken ct = default)
        {
            var m = await _repo.GetAsync(q.Id, ct);

            return m is null ? null : new MemberDTO(m.MemberId, m.Name,m.Email);        
        
        }

    }
}
