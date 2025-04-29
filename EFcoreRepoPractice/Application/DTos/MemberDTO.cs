namespace EFcoreRepoPractice.Application.DTos
{
    //public class MemberDTO
    //{
    //    //public int Id { get; set; }
    //    //public string? Name { get; set; }

    //    //public string? Email { get; set; }

    //    //public int Age { get; set; }

    //}
    public record MemberDTO(int Id, string Name, string Email);

    public record CreateMemberDTO(string Name, string Email, int Age);

    public record UpdateMemberDTO(int Id,string Name, string Email, int Age);

    public record DeleteMemberDTO(int Id);

}
