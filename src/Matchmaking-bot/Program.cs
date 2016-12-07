using Discore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Matchmaking_bot
{
    public class Program
    {

        private DiscordClient _client;
        string lineup;
        Team t1, t2;
        Player p;
        struct channellist
        {
            public Team t1;
            public Team t2;
            public void init()
            {
                t1 = new Team();
                t2 = new Team();
            }
        }

        public static void Main(string[] args) => new Program().Start();
        public void Start()
        {
            channellist def = new channellist();
            def.init();
            ulong channelId = 252113301004222465;
            List<ulong> ids = new List<ulong>();
            List<channellist> channels = new List<channellist>();
            ids.Add(channelId);
            channels.Add(def);
            List<Server> servers = new List<Server>();
            servers.Add(new Server("Amsterdam", "95.211.1.210:27029"));
            lineup = "GK CB LB RB CM LW RW CF";

            t1 = def.t1;
            t2 = def.t2;
            t1.setlist(lineup);
            t2.setlist(lineup);
            t1.init();
            t2.init();
            Console.Write("enter token: ");
            string token = Console.ReadLine();
            _client = new DiscordClient();

            _client.Connect(token);

            _client.OnMessageCreated += async (s, e) => {
                // Check to make sure that the bot is not the author

                if (e.Message.Author != _client.User)
                {
                    if (ids.Contains(ulong.Parse(e.Message.Channel.Id)))
                    {

                        t1 = channels[ids.IndexOf(ulong.Parse(e.Message.Channel.Id))].t1;
                        t2 = channels[ids.IndexOf(ulong.Parse(e.Message.Channel.Id))].t2;

                        /*if (d.Message.Text.ToLower().Equals("!clear"))
                        {
                            channels.RemoveAt(ids.IndexOf(d.Channel.Id));
                            ids.Remove(d.Channel.Id);
                            m = new Game(ids.Count + " channels");
                            _client.SetGame(m);

                            int l = d.Channel.Position;
                            string nm = d.Channel.Name;
                            await d.Channel.Delete();
                            var _channel = await d.Server.CreateChannel("nm", ChannelTypd.Text);
                            await _channel.Edit(nm, t1.toString() + Environment.NewLine + t2.toString(),l);
                        }
                        else*/
                        if (e.Message.Content.ToLower().Equals("!unrequest"))
                        {
                            if (e.Message.AuthorMember.HasPermission(DiscordPermission.Administrator))
                            {
                                channels.RemoveAt(ids.IndexOf(ulong.Parse(e.Message.Channel.Id)));
                                ids.Remove(ulong.Parse(e.Message.Channel.Id));
                            }


                        }
                        else
                        if (e.Message.Content.ToLower().StartsWith("!add "))
                        {

                            if (e.Message.AuthorMember.HasPermission(DiscordPermission.Administrator))
                            {
                                string z = e.Message.Content.Substring(5);
                                await e.Message.Delete();
                                string n = z.Substring(0, z.LastIndexOf(' '));
                                string po = z.Substring(z.LastIndexOf(' ') + 1);
                                p = new Player();
                                p.setname(n);
                                if (t1.sign(po.ToUpper(), p))
                                {
                                    await e.Message.Channel.SendMessage(z + " added to team");
                                }
                                else if (t2.sign(po.ToUpper(), p))
                                {
                                    await e.Message.Channel.SendMessage(z + " added to team");
                                }
                                else
                                    await e.Message.Channel.SendMessage("<@"+e.Message.Author.Id+">" + " position already taken.");
                            }
                            //await e.Channel.Edit(null, t1.toString() + Environment.NewLine + t2.toString());
                        }
                        else
                        if (e.Message.Content.ToLower().StartsWith("!remove "))
                        {

                            if (e.Message.AuthorMember.HasPermission(DiscordPermission.Administrator))
                            {
                                string z = e.Message.Content.Substring(8);
                                await e.Message.Delete();
                                p = new Player();
                                p.setname(z);
                                if (t1.unsign(p))
                                {
                                    await e.Message.Channel.SendMessage(z + " removed from team");
                                }
                                else if (t2.unsign(p))
                                {
                                    await e.Message.Channel.SendMessage(z + " removed from team");
                                }
                                else
                                    await e.Message.Channel.SendMessage("No player in a team called " + z);
                            }
                            //await e.Channel.Edit(null, t1.toString() + Environment.NewLine + t2.toString());
                        }
                        else
                        if (e.Message.Content.ToLower().Equals("!help"))
                        {
                            await e.Message.Delete();
                            await e.Message.Channel.SendMessage(" Greetings from khaledinho :laughing:  \n``Commands:``\n```!help, !remove <player>, !add <player> <pos>, !lineup <pos> <pos> ... <pos>, !ready, !ready <server number>, !unsign, !list, !reset, !vs <team name>, !vsmix, !servers, !addsv <name> <ip>, !rmsv, !sub, !<pos>, !<pos><1/2> ```");
                        }
                        else
                        if (e.Message.Content.ToLower().StartsWith("!lineup "))
                        {
                            if (e.Message.AuthorMember.HasPermission(DiscordPermission.Administrator))
                            {
                                lineup = e.Message.Content.Substring(8);
                                while (lineup.Contains("  "))
                                {
                                    lineup = lineup.Remove(lineup.IndexOf("  "), 1);
                                }
                                if (lineup.Length > 1)
                                {
                                    if (lineup.StartsWith(" "))
                                        lineup = lineup.Substring(1);
                                    t1.setlist(lineup);
                                    t2.setlist(lineup);
                                    t1.init();
                                    t2.init();
                                    //await e.Channel.Edit(null, t1.toString() + Environment.NewLine + t2.toString());
                                }
                                else
                                {
                                    await e.Message.Channel.SendMessage("Lineup isn't entered correctly. ");

                                }
                            }

                        }
                        else
                        if (e.Message.Content.ToLower().Equals("!unsign"))
                        {
                            await e.Message.Delete();
                            p = new Player();
                            p.setname(e.Message.Author.Username);
                            p.setmention("<@"+e.Message.Author.Id+">");
                            p.setid(ulong.Parse(e.Message.Author.Id));
                            if (t1.unsign(p))
                            {
                                await e.Message.Channel.SendMessage(e.Message.Author.Username + " unsigned.");
                                //await e.Channel.Edit(null, t1.toString() + Environment.NewLine + t2.toString());
                            }
                            else if (t2.unsign(p))
                            {
                                await e.Message.Channel.SendMessage(e.Message.Author.Username + " unsigned.");
                                //await e.Channel.Edit(null, t1.toString() + Environment.NewLine + t2.toString());
                            }
                            else
                                await e.Message.Channel.SendMessage("<@"+e.Message.Author.Id+">" + " not in a team.");
                        }
                        else
                        if (e.Message.Content.ToLower().Equals("!ready"))
                        {
                            await e.Message.Delete();
                            await e.Message.Channel.SendMessage("join server " + t1.getmentions() + " vs " + t2.getmentions());
                            await e.Message.Channel.SendMessage(t1.toString() + Environment.NewLine + t2.toString());
                        }
                        else
                        if (e.Message.Content.ToLower().StartsWith("!ready "))
                        {
                            int num;
                            string z = e.Message.Content.Substring(7);
                            await e.Message.Delete();
                            try
                            {
                                num = int.Parse(z);
                                num--;
                                await e.Message.Channel.SendMessage("join server " + t1.getmentions() + " vs " + t2.getmentions() + " Server ->" + servers[num].toString());
                                await e.Message.Channel.SendMessage(t1.toString() + Environment.NewLine + t2.toString());
                            }
                            catch (Exception i)
                            {
                                await e.Message.Channel.SendMessage("Server not found.");
                            }


                        }
                        else
                        if (e.Message.Content.ToLower().Equals("!list"))
                        {
                            await e.Message.Delete();
                            await e.Message.Channel.SendMessage("The team lists are " + Environment.NewLine + t1.toString() + Environment.NewLine + t2.toString());
                        }
                        else
                        if (e.Message.Content.ToLower().Equals("!reset"))
                        {
                            t1.init();
                            t2.init();
                            await e.Message.Delete();
                            //await e.Channel.Edit(null, t1.toString() + Environment.NewLine + t2.toString());
                            await e.Message.Channel.SendMessage(t1.toString() + Environment.NewLine + t2.toString());
                        }
                        else
                        if (e.Message.Content.ToLower().StartsWith("!vs "))
                        {
                            t2.init();
                            t2.setmix(false);
                            t2.setname(e.Message.Content.Substring(4));
                            await e.Message.Delete();
                            //await e.Channel.Edit(null, t1.toString() + Environment.NewLine + t2.toString());
                            await e.Message.Channel.SendMessage("Team " + e.Message.Content.Substring(4) + " is challenging Mix");
                        }
                        else
                        if (e.Message.Content.ToLower().Equals("!vsmix"))
                        {
                            await e.Message.Delete();
                            if (t2.getmix() == false)
                            {
                                t2.init();
                                //await e.Channel.Edit(null, t1.toString() + Environment.NewLine + t2.toString());
                                await e.Message.Channel.SendMessage("Teams are now mix");
                            }
                            else await e.Message.Channel.SendMessage("The teams are already mix");
                        }
                        else
                        if (e.Message.Content.ToLower().Equals("!servers"))
                        {
                            await e.Message.Delete();
                            string z = "``Servers: ``" + Environment.NewLine;
                            for (int i = 0; i < servers.Count; i++)
                            {
                                z += "``" + (i + 1) + "`` " + servers[i].toString() + Environment.NewLine;
                            }
                            await e.Message.Channel.SendMessage(z);
                        }
                        else
                        if (e.Message.Content.ToLower().StartsWith("!addsv "))
                        {
                            string z = e.Message.Content.Substring(7);
                            string name = z.Substring(0, z.LastIndexOf(' '));
                            string ip = z.Substring(z.LastIndexOf(' ') + 1);
                            await e.Message.Delete();
                            if (ip.Contains(".") && ip.Contains(":"))
                            {
                                servers.Add(new Server(name, ip));
                            }
                            else
                                await e.Message.Channel.SendMessage("Wrong ip inserted");
                        }
                        else
                        if (e.Message.Content.ToLower().StartsWith("!rmsv "))
                        {
                            string z = e.Message.Content.Substring(6);
                            await e.Message.Delete();
                            try
                            {
                                int num = int.Parse(z);
                                num--;
                                servers.RemoveAt(num);
                            }
                            catch (Exception i)
                            {
                                await e.Message.Channel.SendMessage("Server not found");
                            }
                        }
                        else
                        if (e.Message.Content.ToLower().Equals("!sub"))
                        {
                            await e.Message.Delete();
                        }
                        else
                        if (e.Message.Content.ToLower().Equals("!"))
                        {
                           /* if (e.Message.AuthorMember.HasPermission(DiscordPermission.Administrator))
                            {
                                await e.Channel.DeleteMessages(e.Channel.Messages.ToArray());
                            }*/
                        }
                        else
                        if (e.Message.Content.ToLower().StartsWith("!"))
                        {
                            string pos = e.Message.Content.Substring(1);
                            p = new Player();
                            p.setname(e.Message.Author.Username);
                            p.setmention("<@"+e.Message.Author.Id+">");
                            p.setid(ulong.Parse(e.Message.Author.Id));
                            await e.Message.Delete();
                            if (!t1.inTeam(p) && !t2.inTeam(p))
                            {
                                if (t1.sign(pos.ToUpper(), p))
                                {
                                    await e.Message.Channel.SendMessage(p.getname() + " signed for match.");
                                }
                                else if (t2.sign(pos.ToUpper(), p))
                                {
                                    await e.Message.Channel.SendMessage(p.getname() + " signed for match.");
                                }
                                else
                                {
                                    Team tmp = t1;
                                    if (pos.Substring(pos.Length - 1) == "2")
                                        tmp = t2;
                                    pos = pos.Substring(0, pos.Length - 1);
                                    if (tmp.sign(pos.ToUpper(), p))
                                    {
                                        await e.Message.Channel.SendMessage(p.getname() + " signed for match.");
                                    }
                                    else
                                        await e.Message.Channel.SendMessage(p.getmention() + " position is taken or incorrect command.");
                                }
                            }
                            else
                                await e.Message.Channel.SendMessage(p.getmention() + " incorrect command.");
                            //await e.Channel.Edit(null, t1.toString() + Environment.NewLine + t2.toString());
                        }
                    }
                    else
                    {
                        if (e.Message.AuthorMember.HasPermission(DiscordPermission.Administrator))
                        {
                            if (e.Message.Content.ToLower().Equals("!request"))
                            {
                                if (!ids.Contains(ulong.Parse(e.Message.Channel.Id)))
                                {
                                    def = new channellist();
                                    def.init();
                                    t1 = def.t1;
                                    t2 = def.t2;
                                    t1.setlist(lineup);
                                    t2.setlist(lineup);
                                    t1.init();
                                    t2.init();
                                    channels.Add(def);
                                    ids.Add(ulong.Parse(e.Message.Channel.Id));
                                }


                            }
                        }
                    }
                }
            };
            Console.ReadLine();
               
        }
    }
}
