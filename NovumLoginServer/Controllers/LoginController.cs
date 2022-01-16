using System.Text;
using HashLib;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NovumLoginServer.DBModels;
using NovumLoginServer.EFCore;
using NovumLoginServer.Models;

namespace NovumLoginServer.Controllers;

public class LoginController : Controller
{
    private LoginViewModel _model = new();
    private readonly MySqlContext _dbContext;

    public LoginController(MySqlContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View(_model);
    }


    [HttpPost]
    public async Task<IActionResult> Index(string username, string password)
    {
        bool isofficialClient = false;
        string userAgent = Request.Headers["User-Agent"];
        if (userAgent.Contains("SQEXAuthor"))
        {
            _model.IsOfficialClient = true;
        }


        Users? user = _dbContext.Users.FirstOrDefault(u => u.Name == username);

        if (user != null)
        {
            string saltedPass = password + user.Salt;

            string hashedString = BitConverter
                .ToString(HashFactory.Crypto.CreateSHA224().ComputeString(saltedPass, Encoding.Default).GetBytes())
                .Replace("-", "");
            if (string.Equals(hashedString.ToLower(), user.Passhash.ToLower()))
            {
                Sessions? session = await _dbContext.Sessions.FirstOrDefaultAsync(s => s.UserID == user.ID);

                if (session == null)
                {
                    session = new Sessions
                    {
                        UserID = user.ID,
                        Expiration = DateTime.Now
                    };
                }
                else
                {
                    _dbContext.Sessions.Remove(session);
                    await _dbContext.SaveChangesAsync();
                }

                // if session date is expired
                if (session.Expiration > DateTime.Now)
                {
                    session.Expiration = DateTime.Now;
                }

                session.ID = GetRandomSessionNumber(50);
                session.Expiration = session.Expiration.AddDays(5);
                await _dbContext.AddAsync(session);

                await _dbContext.SaveChangesAsync();

                _model.Sid = session.ID;
                _model.IsAuthenticated = true;
                _model.IsLoginFailed = false;
                return View(_model);
            }
        }

        _model.IsLoginFailed = true;
        return View(_model);
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