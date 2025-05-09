namespace EFcoreRepoPractice.Application.Commands.MemberCommands
{

    ///<summary>變更一筆 Member </summary>
    public record UpdateMemberCommand(int Id, string? Name = null, string? Email = null, int? Age = null, string? Password = null, UpdateMode um = (UpdateMode)1);
}
