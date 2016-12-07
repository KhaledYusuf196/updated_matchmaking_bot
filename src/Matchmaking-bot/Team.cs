using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Matchmaking_bot
{
    public class Team
    {
        Player def;
        string name;
        List<string> positions;
        List<Player> players;
        bool isMix;

        public Team()
        {
            name = "Mix";
            def = new Player();
            isMix = true;
        }

        public bool sign(string pos, Player p)
        {
            if (isMix)
            {
                for (int i = 0; i < positions.Count; i++)
                {
                    if (positions[i].Equals(pos))
                    {
                        if (players[i].getid() == def.getid() && players[i].getname().Equals(def.getname()))
                        {
                            players[i] = p;
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public bool unsign(Player p)
        {
            for (int i = 0; i < players.Count; i++)
            {
                if (p.getid() == 0)
                {
                    if (players[i].getname().Equals(p.getname()))
                    {
                        players[i] = def;
                        return true;
                    }
                }
                else
                if (players[i].getid() == p.getid())
                {
                    players[i] = def;
                    return true;
                }
            }
            return false;
        }

        public string toString()
        {
            string x = "***Team " + name + "***";
            if (isMix)
            {
                x += Environment.NewLine + "```";
                for (int i = 0; i < positions.Count; i++)
                {
                    x += positions[i] + ":" + players[i].getname() + " ";
                }
                x += "```";
            }
            return x;
        }
        public string getmentions()
        {
            string x = name + " ";
            if (isMix)
                for (int i = 0; i < positions.Count; i++)
                {
                    x += players[i].getmention() + " ";
                }
            return x;
        }

        public bool inTeam(Player p)
        {
            for (int i = 0; i < positions.Count; i++)
            {
                if (p.getid() == players[i].getid())
                    return true;
            }
            return false;
        }

        public void setmix(bool m)
        {
            isMix = m;
        }

        public bool getmix()
        {
            return isMix;
        }

        public void setname(string n)
        {
            name = n;
        }

        public string getname()
        {
            return name;
        }

        public void setlist(string x)
        {
            positions = new List<string>();
            string z;
            while (x.Contains(" "))
            {
                z = x.Substring(0, x.IndexOf(' '));
                positions.Add(z.ToUpper());
                x = x.Remove(0, x.IndexOf(' ') + 1);
            }
            positions.Add(x.ToUpper());

            string m;
            {
                m = Environment.NewLine + "```";
                for (int i = 0; i < positions.Count; i++)
                {
                    m += positions[i] + " ";
                }
                m += "```";
            }
        }

        public void init()
        {
            name = "Mix";
            isMix = true;
            players = new List<Player>();
            for (int i = 0; i < positions.Count; i++)
            {
                players.Add(new Player());
            }
        }

    }
}
