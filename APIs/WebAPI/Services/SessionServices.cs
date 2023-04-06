using Application.Interfaces;
using Newtonsoft.Json;

namespace WebAPI.Services
{
    public class SessionServices : ISessionServices
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private const string KEY_ACCESS = "LOGIN_TOKEN";
        private readonly ILogger<SessionServices> _logger;

        public SessionServices(IHttpContextAccessor contextAccessor, ILogger<SessionServices> logger)
        {
            _contextAccessor = contextAccessor;
            _logger = logger;
        }

        public void SaveToken(int id, string token)
        {
            if (_contextAccessor.HttpContext != null)
            {
                var session = _contextAccessor.HttpContext.Session;

                //Get session with key 'LOGIN_TOKEN'
                var json = session.GetString(KEY_ACCESS);
                Dictionary<int, string>? loginTokens;
                if (json != null)
                {
                    loginTokens = JsonConvert.DeserializeObject<Dictionary<int, string>>(json);
                }
                else
                {
                    loginTokens = new Dictionary<int, string>();
                }

                if (loginTokens != null)
                {
                    loginTokens.TryAdd(id, token);

                    //Convert Dictionary loginTokens to Json and save to Session 'LOGIN_TOKEN'
                    var jsonSave = JsonConvert.SerializeObject(loginTokens);
                    session.SetString(KEY_ACCESS, jsonSave);

                    //Console.ForegroundColor = ConsoleColor.Red;
                    //Console.WriteLine($"[User Login {DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")}] User Id {id} logged in." +
                    //    $"\n[Token {DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")}] {loginTokens[id]}\nToken List:");
                    //Console.ForegroundColor = ConsoleColor.White;
                    _logger.LogInformation($"[User Login {DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")}] User Id {id} logged in." +
                        $"\n[Token ID {id} {DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")}] {loginTokens[id]}");
                }
            }
        }

        public string GetTokenByKey(int id)
        {
            if (_contextAccessor.HttpContext != null)
            {
                var session = _contextAccessor.HttpContext.Session;

                var json = session.GetString(KEY_ACCESS);
                Dictionary<int, string>? loginTokens = null;
                if (json != null)
                {
                    loginTokens = JsonConvert.DeserializeObject<Dictionary<int, string>>(json);
                }

                _logger.LogInformation($"[Verify Token {DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")}] Current User Id is {id}.");

                if (loginTokens != null && loginTokens.ContainsKey(id))
                {
                    return loginTokens[id];
                }
            }

            return string.Empty;
        }

        public bool RemoveToken(int id)
        {
            if (_contextAccessor.HttpContext != null)
            {
                var session = _contextAccessor.HttpContext.Session;

                var json = session.GetString(KEY_ACCESS);
                Dictionary<int, string>? loginTokens = null;
                if (json != null)
                {
                    loginTokens = JsonConvert.DeserializeObject<Dictionary<int, string>>(json);
                }

                _logger.LogInformation($"[User Logout {DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")}] Current User Id is {id}.");

                if (loginTokens != null && loginTokens.ContainsKey(id))
                {
                    loginTokens.Remove(id);

                    //Convert Dictionary loginTokens to Json and save to Session 'LOGIN_TOKEN'
                    var jsonSave = JsonConvert.SerializeObject(loginTokens);
                    session.SetString(KEY_ACCESS, jsonSave);

                    //Console.ForegroundColor = ConsoleColor.Red;
                    //Console.WriteLine($"[User Logout {DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")}] User Id {id} logged out. [Token] removed.\nToken List: ");
                    //foreach (var token in loginTokens.Keys)
                    //{
                    //    Console.WriteLine($"{token} : {loginTokens[token]}");
                    //}
                    //Console.ForegroundColor = ConsoleColor.White;
                    _logger.LogInformation($"[User Logout {DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")}] User Id {id} logged out." +
                        $"\n[Token ID {id} {DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")}] removed.");

                    return true;
                }
            }

            return false;
        }
    }
}