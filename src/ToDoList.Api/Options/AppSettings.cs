﻿namespace ToDoList.Api.Options;

public class AppSettings
{
	public string AesKey { get; set; }
	public string AlgorithmIV { get; set; }
	public int PasswordResetExpirationInMin { get; set; }
}
