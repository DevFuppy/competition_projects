namespace EFcoreRepoPractice.Application.Commands.MemberCommands
{

    ///<summary>變更一筆 Member </summary>
    public record UpdatePasswordCommand(int Id,  string? Password = null, UpdateMode um = (UpdateMode)1);

    
}
