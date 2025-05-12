using EFcoreRepoPractice.Application.DTos;
using EFcoreRepoPractice.Data;
using EFcoreRepoPractice.Infrastructure.repos;
using EFcoreRepoPractice.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;


namespace EFcoreRepoPractice.Application.Commands.MemberCommands
{
    public class UpdateMemberHandler
    {
        private IUnitOfWork _uow;


        public UpdateMemberHandler(IUnitOfWork uow)
        {
            _uow = uow;

        }

        public async Task<MemberDTO?> UpdateOneMemberAsync(UpdateMemberCommand q, CancellationToken ct = default)
        {


            var entity = _uow.GetRepository<Member>();
            var existing = await entity.GetSelectively(x => q.Id == x.MemberId)?.FirstOrDefaultAsync();

            if (existing == null)
            {
                return null;
            }


            existing.Name = q.Name;
            existing.Email = q.Email;
            existing.Age = q.Age;
            existing.Password = q.Password;


            //判斷selectiveMode 值是否正常
            if (!Enum.IsDefined(typeof(UpdateMode), q.um))
            {
                throw new ArgumentOutOfRangeException(nameof(q.um) ,"輸入模式不在updateMode裡面");
            }
            
            

            await _uow.ExecuteTransactionAsync(  () =>
            {
                if (q.um == UpdateMode.Full)
                { 
                    entity.Update(existing);
                }
                else if (q.um == UpdateMode.Selective)
                { 
                    entity.UpdateSelective(existing);                   
                }
                                 

            });

            return new MemberDTO(existing.MemberId, existing.Name, existing.Email, existing.Age);

        }



    }
}
