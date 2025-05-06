namespace EFcoreRepoPractice.Application.Commands.MemberCommands
{
    ///<summary>註冊用</summary>
    public record RegisterMemberCommand(string Email, string Password);

    ///<summary>帳密登入用 </summary>
    public record LoginInfo(string email, string pwd);

}
