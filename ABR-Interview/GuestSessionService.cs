using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace ABR_Interview;

public class GuestSessionService
{
    private readonly IDistributedCache _cache;
    private readonly ILogger<GuestSessionService> _logger;
    private readonly CacheSettingsOptions _options;

    public GuestSessionService(
        IDistributedCache cache,
        ILogger<GuestSessionService> logger,
        IOptions<CacheSettingsOptions> options)
    {
        _cache = cache;
        _logger = logger;
        _options = options.Value;
    }

    public async Task<SessionKey?> CreateSessionKey(string userId)
    {
        var key = SessionKey.Create(userId);

        var isValid = await ValidateSessionKey(key.Code, key.UserId);

        if (!isValid)
        {
            _logger.LogWarning("Session key already exists: {Code} for user: {User}", key.Code, key.UserId);
            return null;
        }

        await _cache.SetAsync(
            key.Code,
            JsonSerializer.SerializeToUtf8Bytes(key),
            new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(_options.DefaultExpirationInMinutes)
            }
        );
        _logger.LogInformation("Created session key: {Code} for user: {User}", key.Code, key.UserId);

        return key;
    }

    public async Task UseSessionKey(string sessionKey)
    {
        var data = await _cache.GetAsync(sessionKey);
        var key = JsonSerializer.Deserialize<SessionKey>(data);

        _logger.LogInformation("Using session key: {Code} for user: {User}", key.Code, key.UserId);
        await _cache.RemoveAsync(key.Code);
    }

    public async Task<bool> ValidateSessionKey(string code, string userId)
    {
        var data = await _cache.GetAsync(code);

        if (data is not null)
        {
            return false;
        }

        var key = JsonSerializer.Deserialize<SessionKey>(data);
        if (key?.UserId != userId)
        {
            _logger.LogWarning("Session key: {Code} does not belong to user: {User}", code, userId);
            return false;
        }

        return true;
    }
}
