using System.Text;
using HashLib;
using Microsoft.AspNetCore.Mvc;
using NovumLoginServer.DBModels;
using NovumLoginServer.EFCore;
using NovumLoginServer.Models;

namespace NovumLoginServer.Controllers;

public class CreateUserController : Controller
{
    private CreateUserModel _model = new CreateUserModel();
    
    [HttpGet]
    public IActionResult Index()
    {
        return View(_model);
    }
    
    [HttpPost]
    
    public IActionResult Index(string username, string password,string repeatPassword ,string email)
    {
        Users? user = MySqlContext.Instance.Users.FirstOrDefault(u => u.Name == username);

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

        var transaction = MySqlContext.Instance.Database.BeginTransaction();
        MySqlContext.Instance.Users.Add(user);
        MySqlContext.Instance.SaveChanges();
        transaction.Commit();
        
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