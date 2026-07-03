namespace Acervo.Web.DTOs
{
    public record UserDto(
        long     Id,
        string   Name,
        string   Email,
        DateTime CreatedAt,
        int      Role);

    public record CreateUserDto(string Name, string Email, string PasswordHash);

    public record UpdateUserDto(long Id, string Name, string Email);
}
