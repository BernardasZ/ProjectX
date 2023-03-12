namespace ToDoList.Api.Helpers;

public interface IAesCryptoHelper
{
    string EncryptString(string text);

    string DecryptString(string text);
}
