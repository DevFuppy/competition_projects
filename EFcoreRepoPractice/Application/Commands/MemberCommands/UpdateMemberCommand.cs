namespace EFcoreRepoPractice.Application.Commands.MemberCommands
{

    ///<summary>變更一筆 Member </summary>
    public record UpdateMemberCommand(int Id, string Name, string Email, int Age);
}
