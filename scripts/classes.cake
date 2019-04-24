using System;
using System.Collections;

public enum OSType
{
    /// <summary>
    /// Windows.
    /// </summary>
    Win,
    /// <summary>
    /// Linux.
    /// </summary>
    Nix,
    /// <summary>
    /// Apple MacOS.
    /// </summary>
    Mac
}

public class DatabaseSetting
{
    /// <summary>
    /// 
    /// </summary>
    public OSType OS { get; set; }
    public string Host{ get; set; }
    public string DbType { get; set; }
    public string Edition { get; set; }
    public string ServiceName { get; set; }
    public string Server{ get; set; }
    public string Port{ get; set; }
    public string Path { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public bool Enabled { get; set; }
}

/// <summary>
/// Administrative machine credentials if databases are on a remote machine.
/// </summary>
public class AdminCredentials
{
    public string Name{ get; set; }
    public string Password{ get; set; }
}

public class ProductSettings
{
    public AdminCredentials Credentials{ get; set; }
    public IntegrationTestsSettings IntegrationTests{ get; set; }
    public UnitTestsSettings UnitTests{ get; set; }
}

public class UnitTestsSettings
{
}

public class IntegrationTestsSettings
{
    public List<DatabaseSetting> Databases { get; set; }
}