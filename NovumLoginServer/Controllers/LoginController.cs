using System.Text;
using HashLib;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NovumLoginServer.DBModels;
using NovumLoginServer.Models;
using StackExchange.Redis;
using StackExchange.Redis.Extensions.Core.Abstractions;

namespace NovumLoginServer.Controllers;

public class LoginController : Controller
{
    private readonly LoginViewModel _model = new();
    private readonly DBContext _dbContext;
    private readonly IRedisDatabase _redisDatabase;

    public LoginController(DBContext dbContext, IRedisDatabase redisDatabase)
    {
        _dbContext = dbContext;
        _redisDatabase = redisDatabase;
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


        User? user = _dbContext.Users.FirstOrDefault(u => u.Name == username);

        if (user != null)
        {
            string saltedPass = password + user.Salt;

            string hashedString = BitConverter
                .ToString(HashFactory.Crypto.CreateSHA224().ComputeString(saltedPass, Encoding.Default).GetBytes())
                .Replace("-", "");
            if (string.Equals(hashedString.ToLower(), user.Passhash.ToLower()))
            {
                /*
                Session? session = await _dbContext.Sessions.FirstOrDefaultAsync(s => s.UserID == user.ID);

                if (session == null)
                {
                    session = new Session
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
                return View(_model);*/

                // TODO: We need to check if the user is already within a valid game session so users can't multi log with the same toon
                if (!string.IsNullOrEmpty(user.GameSessionId))
                {
                    // For now we just delete the current server session, we might need a "pubsub" to alert the game servers the user is trying to login
                    // and nuke them off the server to prevent the multi login
                    await _redisDatabase.RemoveAsync(user.GameSessionId);
                }

                string sid = GetRandomSessionNumber(50); 
                await _redisDatabase.AddAsync($"{sid}", user.ID, TimeSpan.FromDays(1));

                user.GameSessionId = sid;
                await _dbContext.SaveChangesAsync();

                _model.Sid = sid;
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