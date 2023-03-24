namespace Application.Authorization.Options;

public class PermissionCacheSettings
{
    public const string SelectionName = nameof(PermissionCacheSettings);

    public string Key { get; set; }

    public int ExpirationTimeInMin { get; set; }
}