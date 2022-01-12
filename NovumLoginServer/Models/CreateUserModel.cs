namespace NovumLoginServer.Models;

public class CreateUserModel
{
    
    public string Username { get; set; }
    
    public string Password { get; set; }
    
    public string Email { get; set; }

    public bool IsError { get; set; } = false;
    public string ErrorMessage { get; set; }
}