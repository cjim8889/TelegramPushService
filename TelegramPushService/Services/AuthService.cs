using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TelegramPushService.Services
{
    public class AuthService
    {


        private Dictionary<string, (string, int)> authTable;
        public AuthService()
        {
            authTable = new Dictionary<string, (string, int)>();
        }


        public void AddAuth(string id, string challengeCode, int subsriberId)
        {
            authTable.Add(id, (challengeCode, subsriberId));
        }

        public string GetChallengeCode(string id)
        {
            return authTable.GetValueOrDefault(id).Item1;
        }

        public int GetSubsriberId(string id)
        {
            return authTable.GetValueOrDefault(id).Item2;
        }

        public bool Authenticate(string id, string key)
        {
            return GetChallengeCode(id) == key;
        }

        public void Remove(string id)
        {
            authTable.Remove(id);
        }

        public string GenerateChallengeCode()
        {
            var random = new Random();

            return random.Next(100000, 1000000).ToString();
        }

    }
}
