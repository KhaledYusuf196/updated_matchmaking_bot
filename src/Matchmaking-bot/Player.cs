using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Matchmaking_bot
{
    public class Player
    {
        string name;
        string mention;
        ulong id;
        public Player()
        {
            name = " ";
            mention = " ";
            id = 0;
        }
        public void setname(string n)
        {
            name = n;
        }
        public void setmention(string n)
        {
            mention = n;
        }
        public void setid(ulong n)
        {
            id = n;
        }
        public ulong getid()
        {
            return id;
        }
        public string getname()
        {
            return name;
        }
        public string getmention()
        {
            return mention;
        }

    }
}
