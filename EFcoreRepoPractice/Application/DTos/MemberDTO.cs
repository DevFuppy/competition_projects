namespace EFcoreRepoPractice.Application.DTos
{ 
    public record MemberDTO(int Id, string? Name = null, string? Email = null, int? Age = null, string? Password = null);

}
