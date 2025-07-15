namespace ABR_Interview;

public class CacheSettingsOptions
{
    public const string SectionName = "CacheSettings";

    public string ConnectionString { get; set; } = string.Empty;
    public int DefaultExpirationInMinutes { get; set; }
}
