using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Matchmaking_bot
{
    public class Server
    {
        string name;
        string ip;

        public Server(string n, string i)
        {
            name = n;
            ip = "steam://connect/" + i;
        }
        public string toString()
        {
            string x = name + " Ip: " + ip;
            return x;
        }

    }
}
