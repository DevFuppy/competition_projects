using EFcoreRepoPractice.Models;
using Microsoft.EntityFrameworkCore;

namespace EFcoreRepoPractice.Infrastructure.repos
{
    public class MemberRepository:IMemberRepository
    {
        private readonly AjaxClassContext _db;


        public MemberRepository(AjaxClassContext db) => _db = db;

        public async Task<Member?> GetAsync(int id, CancellationToken ct = default)
       => await _db.Members.AsNoTracking().FirstOrDefaultAsync(m=>m.MemberId==id,ct);

        public async Task CreateAsync(Member member, CancellationToken ct = default)
        {
              _db.Members.Add(member);

              await _db.SaveChangesAsync(ct);           
             

        }

        public async Task UpdateAsync(Member member, CancellationToken ct = default)
        {
            _db.Members.Update(member);

            await _db.SaveChangesAsync(ct);


        }

        public async Task DeleteAsync(Member member, CancellationToken ct = default)
        {
            _db.Members.Remove(member);

            await _db.SaveChangesAsync(ct);


        }

    }
}

