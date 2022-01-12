using System.Text;
using HashLib;
using Microsoft.AspNetCore.Mvc;
using NovumLoginServer.DBModels;
using NovumLoginServer.EFCore;
using NovumLoginServer.Models;

namespace NovumLoginServer.Controllers;

public class CreateUserController : Controller
{
    private readonly MySqlContext _dbContext;
    private CreateUserModel _model = new CreateUserModel();

    public CreateUserController(MySqlContext dbContext)
    {
        _dbContext = dbContext;
    }


    [HttpGet]
    public IActionResult Index()
    {
        return View(_model);
    }
    
    [HttpPost]
    
    public async Task<IActionResult> Index(string username, string password,string repeatPassword ,string email)
    {
        // Case insensitive lookup
        Users? user = _dbContext.Users.FirstOrDefault(u => u.Name.ToLower() == username.ToLower());

        if (user != null)
        {
            _model.IsError = true;
            _model.ErrorMessage = "User Already Exist";
            return View(_model);
        }


        if (!string.Equals(password, repeatPassword))
        {
            _model.IsError = true;
            _model.ErrorMessage = "Password does not match";
            return View(_model);
        }

        user = new Users
        {
            Name = username,
            Email = email,
            Salt = GetRandomSessionNumber(50)
        };

        string saltedPassword = password + user.Salt;
        user.Passhash = BitConverter
            .ToString(HashFactory.Crypto.CreateSHA224().ComputeString(saltedPassword, Encoding.Default).GetBytes())
            .Replace("-", "").ToLower();

        await _dbContext.Users.AddAsync(user);
        await _dbContext.SaveChangesAsync();
        
        return View("Success");
    }
    
    
    private string GetRandomSessionNumber(int length)
    {
        string chars = "abcdefghijklmnopqrstuvwxyz0123456789";
        Random random = new ();
        string result = new (
            Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)])
                .ToArray());

        return result;
    }
}