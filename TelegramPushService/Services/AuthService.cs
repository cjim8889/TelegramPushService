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


        public void AddAuth(string adminToken, string challengeCode, int subsriberId)
        {
            authTable.Add(adminToken, (challengeCode, subsriberId));
        }

        public string GetChallengeCode(string adminToken)
        {
            return authTable.GetValueOrDefault(adminToken).Item1;
        }

        public int GetSubsriberId(string adminToken)
        {
            return authTable.GetValueOrDefault(adminToken).Item2;
        }

        public bool Authenticate(string adminToken, string challengeToken)
        {
            return GetChallengeCode(adminToken) == challengeToken;
        }

        public void Remove(string adminToken)
        {
            authTable.Remove(adminToken);
        }

        public bool ContainsPublisher(string adminToken)
        {
            return authTable.ContainsKey(adminToken);
        }
        public string GenerateChallengeCode()
        {
            var random = new Random();

            return random.Next(100000, 1000000).ToString();
        }

    }
}
