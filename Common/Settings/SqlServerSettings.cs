namespace Hecey.TTM.Common.Settings;

public class SqlServerSettings
{
    public string? Host { get; init; }
    public string? Port { get; init; }
    public string? Instance { get; init; }
    public string? Database { get; init; }
    public string? UserId { get; init; }
    public string? Password { get; init; }
    public string DefaultContext => $"Server={Host},{Port};Database={Database};User Id={UserId};Password={Password};";
}