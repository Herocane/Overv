/*
	Copyright 2010 MCSharp Team Licensed under the
	Educational Community License, Version 2.0 (the "License"); you may
	not use this file except in compliance with the License. You may
	obtain a copy of the License at
	
	http://www.osedu.org/licenses/ECL-2.0
	
	Unless required by applicable law or agreed to in writing,
	software distributed under the License is distributed on an "AS IS"
	BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express
	or implied. See the License for the specific language governing
	permissions and limitations under the License.
*/
using System;
using System.Collections.Generic;
using System.Text;
using Meebey.SmartIrc4net;
using System.Threading;

namespace Overv
{
    class IRCBot
    {
        static IrcClient irc = new IrcClient();
        static string server = Server.ircServer;
        static string channel = Server.ircChannel;
        static string nick = Server.ircNick;
        static Thread ircThread;

        static string[] names;

        public IRCBot()
        {
            // The IRC Bot must run in a seperate thread, or else the server will freeze.
            ircThread = new Thread(new ThreadStart(delegate
            {
                // Attach event handlers
                irc.OnConnecting += new EventHandler(OnConnecting);
                irc.OnConnected += new EventHandler(OnConnected);
                irc.OnChannelMessage += new IrcEventHandler(OnChanMessage);
                irc.OnJoin += new JoinEventHandler(OnJoin);
                irc.OnPart += new PartEventHandler(OnPart);
                irc.OnQuit += new QuitEventHandler(OnQuit);
                irc.OnNickChange += new NickChangeEventHandler(OnNickChange);
                //irc.OnDisconnected += new EventHandler(OnDisconnected);
                irc.OnQueryMessage += new IrcEventHandler(OnPrivMsg);
                irc.OnNames += new NamesEventHandler(OnNames);
                irc.OnChannelAction += new ActionEventHandler(OnAction);

                // Attempt to connect to the IRC server
                try { irc.Connect(server, Server.ircPort); }
                catch (Exception ex) { Console.WriteLine("Unnable to connect to IRC server: {0}", ex.Message); }
            }));
            ircThread.Start();
        }

        // While connecting
        void OnConnecting(object sender, EventArgs e)
        {
            Server.s.Log("Connecting to IRC");
        }
        // When connected
        void OnConnected(object sender, EventArgs e)
        {
            Server.s.Log("Connected to IRC");
            irc.Login(nick, nick, 0, nick);

            // Check to see if we want to register our bot with nickserv
            
            if (Server.ircIdentify && Server.ircPassword != string.Empty)
            {
                Server.s.Log("Identifying with Nickserv");
                irc.SendMessage(SendType.Message, "nickserv", "IDENTIFY " + Server.ircPassword);
            }

            Server.s.Log("Joining channel");
            irc.RfcJoin(channel);
           
            
            irc.Listen();
        }

        void OnNames(object sender, NamesEventArgs e)
        {
            names = e.UserList;
        }

        void OnChanMessage(object sender, IrcEventArgs e)
        {

            string temp = e.Data.Message;
            
            string allowedchars = "1234567890-=qwertyuiop[]\\asdfghjkl;'zxcvbnm,./!@#$%^*()_+QWERTYUIOPASDFGHJKL:\"ZXCVBNM<>? ";

            foreach (char ch in temp)
            {
                if (allowedchars.IndexOf(ch) == -1)
                    temp = temp.Replace(ch.ToString(), "");
            }

            Server.s.Log("IRC: " + e.Data.Nick + ": " + temp);
            Player.GlobalMessage("IRC: &1" + e.Data.Nick + ": &f" + temp);

            //s.Log("IRC: " + e.Data.Nick + ": " + e.Data.Message);
            //Player.GlobalMessage("IRC: &1" + e.Data.Nick + ": &f" + e.Data.Message);
        }
        // When someone joins the IRC
        void OnJoin(object sender, JoinEventArgs e)
        {
            Server.s.Log(e.Data.Nick + " has joined the IRC");
            Player.GlobalMessage(e.Data.Nick + " has joined the IRC");
            irc.RfcNames(channel);
        }
        // When someone leaves the IRC
        void OnPart(object sender, PartEventArgs e)
        {
            Server.s.Log(e.Data.Nick + " has left the IRC");
            Player.GlobalMessage(e.Data.Nick + " has left the IRC");
            irc.RfcNames(channel);
        }
        void OnQuit(object sender, QuitEventArgs e)
        {
            Server.s.Log(e.Data.Nick + " has Left the IRC");
            Player.GlobalMessage(e.Data.Nick + " has left the IRC");
            irc.RfcNames(channel);
        }
        void OnPrivMsg(object sender, IrcEventArgs e)
        {
            if (Server.ircControllers.Contains(e.Data.Nick))
            {
                string cmd;
                string msg;
                int len = e.Data.Message.Split(' ').Length;
                cmd = e.Data.Message.Split(' ')[0];
                if (len > 1)
                    msg = e.Data.Message.Substring(e.Data.Message.IndexOf(' ')).Trim();
                else
                    msg = "";

                //Console.WriteLine(cmd + " : " + msg);
                Server.s.Log(cmd + " : " + msg);
                switch (cmd)
                {
                    case "kick":
                        Command.all.Find("kick").Use(null, msg); break;
                    case "ban":
                        Command.all.Find("ban").Use(null, msg); break;
                    case "banip":
                        Command.all.Find("banip").Use(null, msg); break;
                    case "guest":
                        Command.all.Find("guest").Use(null, msg); break;
                    case "builder":
                        Command.all.Find("builder").Use(null, msg); break;
                    case "say":
                        irc.SendMessage(SendType.Message, channel, msg); break;
                    default:
                        irc.SendMessage(SendType.CtcpReply, e.Data.Nick, "Fail No Such Command"); break;
                }
            }
        }
        void OnNickChange(object sender, NickChangeEventArgs e)
        {
            string key;
            if (e.NewNickname.Split('|').Length == 2)
            {
                key = e.NewNickname.Split('|')[1];
                if (key != null && key != "")
                {
                    switch (key)
                    {
                        case "AFK":
                            Player.GlobalMessage("IRC: " + e.OldNickname + " is AFK"); Server.afkset.Add(e.OldNickname); break;
                        case "Away":
                            Player.GlobalMessage("IRC: " + e.OldNickname + " is Away"); Server.afkset.Add(e.OldNickname); break;
                    }
                }
            }
            else if (Server.afkset.Contains(e.NewNickname))
            {
                Player.GlobalMessage("IRC: " + e.NewNickname + " is no longer away");
                Server.afkset.Remove(e.NewNickname);
            }
            else
                Player.GlobalMessage("IRC: " + e.OldNickname + " is now known as " + e.NewNickname);

            irc.RfcNames(channel);
        }
        void OnAction(object sender, ActionEventArgs e)
        {
            Player.GlobalMessage("* " + e.Data.Nick + " " + e.ActionMessage);
        }
        
        
        /// <summary>
        /// A simple say method for use outside the bot class
        /// </summary>
        /// <param name="msg">what to send</param>
        public static void Say(string msg)
        {
            if (irc != null && irc.IsConnected && Server.irc)
                irc.SendMessage(SendType.Message, channel, msg);
        }
        public static bool IsConnected()
        {
            if (irc.IsConnected)
                return true;
            else
                return false;
        }
        public static void Reset()
        {
            if (irc.IsConnected)
                irc.Disconnect();
            ircThread = new Thread(new ThreadStart(delegate
            {
                try { irc.Connect(server, Server.ircPort); }
                catch (Exception e)
                {
                    Server.s.Log("Error Connecting to IRC");
                    Server.s.Log(e.ToString());
                }
            }));
            ircThread.Start();
        }
        public static string[] GetConnectedUsers()
        {
            return names;
        }
    }
}
