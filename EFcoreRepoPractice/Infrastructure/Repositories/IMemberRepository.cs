using EFcoreRepoPractice.Models;

namespace EFcoreRepoPractice.Infrastructure.repos
{
    public interface IMemberRepository
    {
        Task<Member?> GetAsync(int id, CancellationToken ct = default);
        
        Task CreateAsync(Member member, CancellationToken ct = default);

        Task UpdateAsync(Member member, CancellationToken ct = default);

        Task DeleteAsync(Member member, CancellationToken ct = default);
    }
}
