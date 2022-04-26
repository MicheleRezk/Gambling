using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gambling.Backend.IntegrationTests
{
    public static class ApiRoutes
    {
        public const string RootApi = "api/";

        public static class Players
        {
            public const string GetById = RootApi+"players/{playerId}";
            public const string Post = RootApi + "players";
        }
        public static class Bets
        {
            public const string Post = RootApi + "bets";
        }

    }
}
