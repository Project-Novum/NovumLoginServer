using System.Text;
using HashLib;
using Microsoft.AspNetCore.Mvc;
using NovumLoginServer.DBModels;
using NovumLoginServer.EFCore;
using NovumLoginServer.Models;

namespace NovumLoginServer.Controllers;

public class LoginController : Controller
{
    private LoginViewModel _model = new();

    [HttpGet]
    public IActionResult Index()
    {
        return View(_model);
    }


    [HttpPost]
    public IActionResult Index(string username, string password)
    {
        bool isofficialClient = false;
        string userAgent = Request.Headers["User-Agent"];
        if (userAgent.Contains("SQEXAuthor"))
        {
            _model.IsOfficialClient = true;
        }


        Users? user = MySqlContext.Instance.Users.FirstOrDefault(u => u.Name == username);

        if (user != null)
        {
            string saltedPass = password + user.Salt;

            string hashedString = BitConverter
                .ToString(HashFactory.Crypto.CreateSHA224().ComputeString(saltedPass, Encoding.Default).GetBytes())
                .Replace("-", "");
            if (string.Equals(hashedString.ToLower(), user.Passhash.ToLower()))
            {
                Sessions? session = MySqlContext.Instance.Sessions.FirstOrDefault(s => s.UserID == user.ID);
                using var transaction = MySqlContext.Instance.Database.BeginTransaction();
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
                    MySqlContext.Instance.Sessions.Remove(session);
                    MySqlContext.Instance.SaveChanges();
                }

                // if session date is expired
                if (session.Expiration > DateTime.Now)
                {
                    session.Expiration = DateTime.Now;
                }

                session.ID = GetRandomSessionNumber(50);
                session.Expiration = session.Expiration.AddDays(5);

                MySqlContext.Instance.Sessions.Add(session);
                MySqlContext.Instance.SaveChanges();
                transaction.Commit();
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