using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TelegramPushService.Services
{
    public class AuthService
    {


        private Dictionary<string, string> authTable;
        public AuthService()
        {
            authTable = new Dictionary<string, string>();
        }


        public void AddAuth(string id, string challengeCode)
        {
            authTable.Add(id, challengeCode);
        }

        public string GetChallengeCode(string id)
        {
            return authTable.GetValueOrDefault(id);
        }

        public bool Authenticate(string id, string key)
        {
            return GetChallengeCode(id) == key;
        }

        public string GenerateChallengeCode()
        {
            var random = new Random();

            return random.Next(100000, 1000000).ToString();
        }

    }
}
