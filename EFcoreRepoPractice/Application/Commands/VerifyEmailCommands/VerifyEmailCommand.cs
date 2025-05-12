namespace EFcoreRepoPractice.Application.Commands.VerifyEmailCommands
{
    public record VerifyEmailCommand(int? Id =null, string? Email=null);

    public record UpdateTokenCommand(int? TokenId = null, UpdateMode um = (UpdateMode)1);

}
