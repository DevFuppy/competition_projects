namespace EFcoreRepoPractice.Application.Queries.EmailQueries
{
    public class EmailDetailQuery
    {
        ///<summary>查找token</summary>
        public record TokenDetailQuery(string? Token = null);
    }
}
