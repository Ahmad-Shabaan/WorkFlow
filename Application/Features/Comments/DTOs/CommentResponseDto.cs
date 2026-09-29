namespace Application.Features.Comments.DTOs
{
    public sealed record CommentResponseDto(Guid PublicId,string Author, string Content)
    {
    }
}
