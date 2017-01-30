using Discore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Matchmaking_bot
{
    public class Program
    {

        private DiscordClient _client;
        string lineup;
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
        string svdata;
        FileStream file;
        StreamReader reader;

        public static void Main(string[] args) => new Program().Start();
        public void Start()
        {
            lineup = "GK CB LB RB CM LW RW CF";
            channellist def = new channellist();
            ulong channelId;
            List<ulong> ids = new List<ulong>();
            List<ulong> blacklist = new List<ulong>();
            List<channellist> channels = new List<channellist>();
            file = new FileStream("config/channels.txt", FileMode.Open, FileAccess.Read);
            reader = new StreamReader(file);
            Console.WriteLine("running in channels : ");
            while((svdata = reader.ReadLine()) != null)
            {
                channelId = ulong.Parse(svdata);
                Console.WriteLine(channelId);
                def = new channellist();
                def.init();
                def.t1.setlist(lineup);
                def.t2.setlist(lineup);
                def.t1.init();
                def.t2.init();
                ids.Add(channelId);
                channels.Add(def);
                
            }
            List<Server> servers = new List<Server>();
            file = new FileStream("config/servers.txt",FileMode.Open,FileAccess.Read);
            reader = new StreamReader(file);
            Console.WriteLine("with servers : ");
            while ((svdata = reader.ReadLine()) != null)
            {
                Console.WriteLine(svdata);
                servers.Add(new Server(svdata.Substring(0, svdata.LastIndexOf(' ')),svdata.Substring(svdata.LastIndexOf(' ')+1)));
            }
            /*servers.Add(new Server("Amsterdam", "95.211.1.210:27029"));
            servers.Add(new Server("London", "77.246.174.116:27035"));
            servers.Add(new Server("Paris", "195.154.168.136:27029"));
            servers.Add(new Server("New York City", "206.221.184.242:27029"));
            servers.Add(new Server("Chicago", "46.21.154.203:27029"));*/


            Console.WriteLine("using token : ");
            string token =File.ReadAllText("config/Token.txt");
            Console.WriteLine(token);
            _client = new DiscordClient();

            _client.Connect(token);
            try
            {
                _client.OnMessageCreated += async (s, e) => {
                    // Check to make sure that the bot is not the author

                    if (e.Message.Author != _client.User && !blacklist.Contains(ulong.Parse(e.Message.Author.Id)))
                    {
                        if (ids.Contains(ulong.Parse(e.Message.Channel.Id)))
                        {



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
                                    _client.UpdateStatus(ids.Count + " channels");
                                }


                            }
                            else
                            if (e.Message.Content.ToLower().Equals("!list"))
                            {
                                await e.Message.Delete();
                                await e.Message.Channel.SendMessage("The team lists are " + Environment.NewLine + channels[ids.IndexOf(ulong.Parse(e.Message.Channel.Id))].t1.toString() + Environment.NewLine + channels[ids.IndexOf(ulong.Parse(e.Message.Channel.Id))].t2.toString());
                            }
                            else
                            if (e.Message.Content.ToLower().StartsWith("!add "))
                            {

                                if (e.Message.AuthorMember.HasPermission(DiscordPermission.ManageChannels))
                                {
                                    string z = e.Message.Content.Substring(5);
                                    await e.Message.Delete();
                                    if(z.Contains(" "))
                                    {
                                        string n = z.Substring(0, z.LastIndexOf(' '));
                                        string po = z.Substring(z.LastIndexOf(' ') + 1);
                                        p = new Player();
                                        p.setname(n);
                                        if (channels[ids.IndexOf(ulong.Parse(e.Message.Channel.Id))].t1.sign(po.ToUpper(), p))
                                        {
                                            await e.Message.Channel.SendMessage(z + " added to team");
                                            await e.Message.Channel.SendMessage("The team lists are " + Environment.NewLine + channels[ids.IndexOf(ulong.Parse(e.Message.Channel.Id))].t1.toString() + Environment.NewLine + channels[ids.IndexOf(ulong.Parse(e.Message.Channel.Id))].t2.toString());
                                        }
                                        else if (channels[ids.IndexOf(ulong.Parse(e.Message.Channel.Id))].t2.sign(po.ToUpper(), p))
                                        {
                                            await e.Message.Channel.SendMessage(z + " added to team");
                                            await e.Message.Channel.SendMessage("The team lists are " + Environment.NewLine + channels[ids.IndexOf(ulong.Parse(e.Message.Channel.Id))].t1.toString() + Environment.NewLine + channels[ids.IndexOf(ulong.Parse(e.Message.Channel.Id))].t2.toString());
                                        }
                                        else
                                            await e.Message.Channel.SendMessage("<@" + e.Message.Author.Id + ">" + " position already taken.");
                                    }
                                    else
                                    {
                                        await e.Message.Channel.SendMessage("<@" + e.Message.Author.Id + "> you have to add player name and position");
                                    }
                                    
                                }
                                //await e.Channel.Edit(null, t1.toString() + Environment.NewLine + t2.toString());
                            }
                            else
                            if (e.Message.Content.ToLower().StartsWith("!remove "))
                            {

                                if (e.Message.AuthorMember.HasPermission(DiscordPermission.ManageChannels))
                                {
                                    string z = e.Message.Content.Substring(8);
                                    await e.Message.Delete();
                                    p = new Player();
                                    p.setname(z);
                                    if (channels[ids.IndexOf(ulong.Parse(e.Message.Channel.Id))].t1.unsign(p))
                                    {
                                        await e.Message.Channel.SendMessage(z + " removed from team");
                                        await e.Message.Channel.SendMessage("The team lists are " + Environment.NewLine + channels[ids.IndexOf(ulong.Parse(e.Message.Channel.Id))].t1.toString() + Environment.NewLine + channels[ids.IndexOf(ulong.Parse(e.Message.Channel.Id))].t2.toString());
                                    }
                                    else if (channels[ids.IndexOf(ulong.Parse(e.Message.Channel.Id))].t2.unsign(p))
                                    {
                                        await e.Message.Channel.SendMessage(z + " removed from team");
                                        await e.Message.Channel.SendMessage("The team lists are " + Environment.NewLine + channels[ids.IndexOf(ulong.Parse(e.Message.Channel.Id))].t1.toString() + Environment.NewLine + channels[ids.IndexOf(ulong.Parse(e.Message.Channel.Id))].t2.toString());
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
                                if (e.Message.AuthorMember.HasPermission(DiscordPermission.ManageChannels))
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
                                        channels[ids.IndexOf(ulong.Parse(e.Message.Channel.Id))].t1.setlist(lineup);
                                        channels[ids.IndexOf(ulong.Parse(e.Message.Channel.Id))].t2.setlist(lineup);
                                        channels[ids.IndexOf(ulong.Parse(e.Message.Channel.Id))].t1.init();
                                        channels[ids.IndexOf(ulong.Parse(e.Message.Channel.Id))].t2.init();
                                        //await e.Channel.Edit(null, t1.toString() + Environment.NewLine + t2.toString());
                                    }
                                    else
                                    {
                                        await e.Message.Channel.SendMessage("Lineup isn't entered correctly. ");

                                    }
                                    await e.Message.Delete();
                                }

                            }
                            else
                            if (e.Message.Content.ToLower().Equals("!unsign"))
                            {
                                await e.Message.Delete();
                                p = new Player();
                                p.setname(e.Message.Author.Username);
                                p.setmention("<@" + e.Message.Author.Id + ">");
                                p.setid(ulong.Parse(e.Message.Author.Id));
                                if (channels[ids.IndexOf(ulong.Parse(e.Message.Channel.Id))].t1.unsign(p))
                                {
                                    await e.Message.Channel.SendMessage(e.Message.Author.Username + " unsigned.");
                                    await e.Message.Channel.SendMessage("The team lists are " + Environment.NewLine + channels[ids.IndexOf(ulong.Parse(e.Message.Channel.Id))].t1.toString() + Environment.NewLine + channels[ids.IndexOf(ulong.Parse(e.Message.Channel.Id))].t2.toString());
                                    //await e.Channel.Edit(null, t1.toString() + Environment.NewLine + t2.toString());
                                }
                                else if (channels[ids.IndexOf(ulong.Parse(e.Message.Channel.Id))].t2.unsign(p))
                                {
                                    await e.Message.Channel.SendMessage(e.Message.Author.Username + " unsigned.");
                                    await e.Message.Channel.SendMessage("The team lists are " + Environment.NewLine + channels[ids.IndexOf(ulong.Parse(e.Message.Channel.Id))].t1.toString() + Environment.NewLine + channels[ids.IndexOf(ulong.Parse(e.Message.Channel.Id))].t2.toString());
                                    //await e.Channel.Edit(null, t1.toString() + Environment.NewLine + t2.toString());
                                }
                                else
                                    await e.Message.Channel.SendMessage("<@" + e.Message.Author.Id + ">" + " not in a team.");
                            }
                            else
                            if (e.Message.Content.ToLower().Equals("!ready"))
                            {
                                await e.Message.Delete();
                                await e.Message.Channel.SendMessage("join server " + channels[ids.IndexOf(ulong.Parse(e.Message.Channel.Id))].t1.getmentions() + " vs " + channels[ids.IndexOf(ulong.Parse(e.Message.Channel.Id))].t2.getmentions());
                                await e.Message.Channel.SendMessage(channels[ids.IndexOf(ulong.Parse(e.Message.Channel.Id))].t1.toString() + Environment.NewLine + channels[ids.IndexOf(ulong.Parse(e.Message.Channel.Id))].t2.toString());
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
                                    await e.Message.Channel.SendMessage("join server " + channels[ids.IndexOf(ulong.Parse(e.Message.Channel.Id))].t1.getmentions() + " vs " + channels[ids.IndexOf(ulong.Parse(e.Message.Channel.Id))].t2.getmentions() + " Server ->" + servers[num].toString());
                                    await e.Message.Channel.SendMessage(channels[ids.IndexOf(ulong.Parse(e.Message.Channel.Id))].t1.toString() + Environment.NewLine + channels[ids.IndexOf(ulong.Parse(e.Message.Channel.Id))].t2.toString());
                                }
                                catch (Exception)
                                {
                                    await e.Message.Channel.SendMessage("Server not found.");
                                }


                            }
                            else
                            if (e.Message.Content.ToLower().Equals("!reset"))
                            {
                                channels[ids.IndexOf(ulong.Parse(e.Message.Channel.Id))].t1.init();
                                channels[ids.IndexOf(ulong.Parse(e.Message.Channel.Id))].t2.init();
                                await e.Message.Delete();
                                //await e.Channel.Edit(null, t1.toString() + Environment.NewLine + t2.toString());
                                await e.Message.Channel.SendMessage(channels[ids.IndexOf(ulong.Parse(e.Message.Channel.Id))].t1.toString() + Environment.NewLine + channels[ids.IndexOf(ulong.Parse(e.Message.Channel.Id))].t2.toString());
                                _client.UpdateStatus(ids.Count + " channels");
                            }
                            else
                            if (e.Message.Content.ToLower().StartsWith("!vs "))
                            {
                                channels[ids.IndexOf(ulong.Parse(e.Message.Channel.Id))].t2.init();
                                channels[ids.IndexOf(ulong.Parse(e.Message.Channel.Id))].t2.setmix(false);
                                channels[ids.IndexOf(ulong.Parse(e.Message.Channel.Id))].t2.setname(e.Message.Content.Substring(4));
                                await e.Message.Delete();
                                //await e.Channel.Edit(null, t1.toString() + Environment.NewLine + t2.toString());
                                await e.Message.Channel.SendMessage("Team " + e.Message.Content.Substring(4) + " is challenging Mix");
                                await e.Message.Channel.SendMessage("The team lists are " + Environment.NewLine + channels[ids.IndexOf(ulong.Parse(e.Message.Channel.Id))].t1.toString() + Environment.NewLine + channels[ids.IndexOf(ulong.Parse(e.Message.Channel.Id))].t2.toString());
                            }
                            else
                            if (e.Message.Content.ToLower().Equals("!vsmix"))
                            {
                                await e.Message.Delete();
                                if (channels[ids.IndexOf(ulong.Parse(e.Message.Channel.Id))].t2.getmix() == false)
                                {
                                    channels[ids.IndexOf(ulong.Parse(e.Message.Channel.Id))].t2.init();
                                    //await e.Channel.Edit(null, t1.toString() + Environment.NewLine + t2.toString());
                                    await e.Message.Channel.SendMessage("Teams are now mix");
                                    await e.Message.Channel.SendMessage("The team lists are " + Environment.NewLine + channels[ids.IndexOf(ulong.Parse(e.Message.Channel.Id))].t1.toString() + Environment.NewLine + channels[ids.IndexOf(ulong.Parse(e.Message.Channel.Id))].t2.toString());
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
                                if(z.Contains(" "))
                                {
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
                                {
                                    await e.Message.Channel.SendMessage("Wrong ip inserted");
                                }
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
                                catch (Exception)
                                {
                                    await e.Message.Channel.SendMessage("Server not found");
                                }
                            }
                            else
                            if (e.Message.Content.ToLower().Equals("!sub"))
                            {
                                _client.UpdateStatus("sub requested");
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
                                p.setmention("<@" + e.Message.Author.Id + ">");
                                p.setid(ulong.Parse(e.Message.Author.Id));
                                await e.Message.Delete();
                                if (!channels[ids.IndexOf(ulong.Parse(e.Message.Channel.Id))].t1.inTeam(p) && !channels[ids.IndexOf(ulong.Parse(e.Message.Channel.Id))].t2.inTeam(p))
                                {
                                    if (channels[ids.IndexOf(ulong.Parse(e.Message.Channel.Id))].t1.sign(pos.ToUpper(), p))
                                    {
                                        await e.Message.Channel.SendMessage(p.getname() + " signed for match.");
                                        await e.Message.Channel.SendMessage("The team lists are " + Environment.NewLine + channels[ids.IndexOf(ulong.Parse(e.Message.Channel.Id))].t1.toString() + Environment.NewLine + channels[ids.IndexOf(ulong.Parse(e.Message.Channel.Id))].t2.toString());
                                    }
                                    else if (channels[ids.IndexOf(ulong.Parse(e.Message.Channel.Id))].t2.sign(pos.ToUpper(), p))
                                    {
                                        await e.Message.Channel.SendMessage(p.getname() + " signed for match.");
                                        await e.Message.Channel.SendMessage("The team lists are " + Environment.NewLine + channels[ids.IndexOf(ulong.Parse(e.Message.Channel.Id))].t1.toString() + Environment.NewLine + channels[ids.IndexOf(ulong.Parse(e.Message.Channel.Id))].t2.toString());
                                    }
                                    else
                                    {
                                        
                                        if (pos.Substring(pos.Length - 1) == "2")
                                        {
                                            pos = pos.Substring(0, pos.Length - 1);
                                            if (channels[ids.IndexOf(ulong.Parse(e.Message.Channel.Id))].t2.sign(pos.ToUpper(), p))
                                            {
                                                await e.Message.Channel.SendMessage(p.getname() + " signed for match.");
                                                await e.Message.Channel.SendMessage("The team lists are " + Environment.NewLine + channels[ids.IndexOf(ulong.Parse(e.Message.Channel.Id))].t1.toString() + Environment.NewLine + channels[ids.IndexOf(ulong.Parse(e.Message.Channel.Id))].t2.toString());
                                            }
                                            else
                                                await e.Message.Channel.SendMessage(p.getmention() + " position is taken or incorrect command.");
                                        }
                                            
                                        
                                        if (pos.Substring(pos.Length - 1) == "1")
                                        {
                                            pos = pos.Substring(0, pos.Length - 1);
                                            if (channels[ids.IndexOf(ulong.Parse(e.Message.Channel.Id))].t1.sign(pos.ToUpper(), p))
                                            {
                                                await e.Message.Channel.SendMessage(p.getname() + " signed for match.");
                                                await e.Message.Channel.SendMessage("The team lists are " + Environment.NewLine + channels[ids.IndexOf(ulong.Parse(e.Message.Channel.Id))].t1.toString() + Environment.NewLine + channels[ids.IndexOf(ulong.Parse(e.Message.Channel.Id))].t2.toString());
                                            }
                                            else
                                                await e.Message.Channel.SendMessage(p.getmention() + " position is taken or incorrect command.");
                                        }
                                            
                                    }
                                }
                                else
                                    await e.Message.Channel.SendMessage(p.getmention() + " incorrect command.");
                                //await e.Channel.Edit(null, t1.toString() + Environment.NewLine + t2.toString());
                            }
                        }
                        else
                        {
                            if(e.Message.Author.Id == "177911539553009665" && e.Message.Content.StartsWith("!ban "))
                            {
                                string banned = e.Message.Content.Substring(5);
                                blacklist.Add(ulong.Parse(banned));
                            }
                            else if (e.Message.Author.Id == "177911539553009665" && e.Message.Content.StartsWith("!unban "))
                            {
                                string banned = e.Message.Content.Substring(7);
                                if (blacklist.Contains(ulong.Parse(banned)))
                                {
                                    blacklist.Remove(ulong.Parse(banned));
                                }
                            }
                            try
                            {
                                if (e.Message.AuthorMember.HasPermission(DiscordPermission.Administrator))
                                {
                                    if (e.Message.Content.ToLower().Equals("!request"))
                                    {
                                        if (!ids.Contains(ulong.Parse(e.Message.Channel.Id)))
                                        {
                                            def = new channellist();
                                            def.init();
                                            def.t1.setlist(lineup);
                                            def.t2.setlist(lineup);
                                            def.t1.init();
                                            def.t2.init();
                                            channels.Add(def);
                                            ids.Add(ulong.Parse(e.Message.Channel.Id));
                                            _client.UpdateStatus(ids.Count + " channels");
                                        }

                                    }
                                }
                            }
                            catch (Exception)
                            {
                                await e.Message.Channel.SendMessage("do not disturb");
                            }
                            
                        }
                    }
                };
            }
            catch (Exception)
            {
                
            }
            Console.WriteLine("please don't press Enter, I don't want to die :(");
            Console.ReadLine();
               
        }
    }
}
