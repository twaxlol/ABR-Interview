using System.Security.Cryptography;

namespace ABR_Interview;

public class SessionKey
{
    public string Code { get; init; } = null!;
    public string UserId { get; init; } = null!;

    public static SessionKey Create(string userId)
    {
        return new SessionKey
        {
            Code = RandomNumberGenerator.GetInt32(0, 999999).ToString("D6"),
            UserId = userId,
        };
    }
}