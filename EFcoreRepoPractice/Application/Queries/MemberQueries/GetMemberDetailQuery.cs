namespace EFcoreRepoPractice.Application.Queries.MemberQueries
{
  
    ///<summary>查一筆 Member </summary>
    public record GetDetailQueryById(int Id);

    ///<summary>帳密登入用 </summary>
    public record LoginInfo(string email,string pwd);
}
