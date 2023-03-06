﻿namespace ToDoList.Api.Helpers;

public class OptionManager
{
	public AppSettings AppSettings { get; set; }
	public ConnectionStrings ConnectionStrings { get; set; }
	public PermissionCacheSettings PermissionCacheSettings { get; set; }
	public Jwt Jwt { get; set; }
	public SmtpSettings SmtpSettings { get; set; }
}

public class AppSettings
{
	public string AesKey { get; set; }
	public string AlgorithmIV { get; set; }
	public int PasswordResetExpirationInMin { get; set; }
}
public class Jwt
{
	public string JWTSecret { get; set; }
	public int JWTExpirationInDay { get; set; }
}

public class ConnectionStrings
{
	public string ProjectXConnectionString { get; set; }
}

public class PermissionCacheSettings
{
	public string Key { get; set; }
	public int ExpirationTimeInMin { get; set; }
}

public class SmtpSettings 
{
	public string Host { get; set; }
	public string UserName { get; set; }
	public string Password { get; set; }
	public string Sender { get; set; }
}