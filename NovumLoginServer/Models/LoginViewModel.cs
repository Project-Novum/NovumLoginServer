namespace NovumLoginServer.Models;

public class LoginViewModel
{
    public bool IsOfficialClient = false;
    public bool IsLoginFailed = false;
    public bool IsAuthenticated { get; set; } = false;

    public string Sid { get; set; }
}