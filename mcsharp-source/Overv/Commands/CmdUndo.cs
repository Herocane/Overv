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

namespace Minecraft_Server
{
    public class CmdUndo : Command
    {
        //public override string name { get { return "undo"; } }
        //public CmdUndo() { }
        //public override void Use(Player p, string message)
        //{
        //    if (message == "") { Help(p); return; }
        //    if (Player.Exists(message))
        //    {
        //        Player who = Player.Find(message);
        //        if (who.group == Group.Find("operator"))
        //        {
        //            p.SendMessage("You can't undo actions of an operator!"); return;
        //        } if (who.actions.Count == 0)
        //        {
        //            p.SendMessage("There are no actions to undo!"); return;
        //        } int actions = who.actions.Count;
        //        //who.actions.ForEach(delegate(Edit e) { e.Undo(); });
        //        who.actions.Clear();
        //        Player.GlobalChat(p, p.color + "*" + p.name + "&e undid " + actions + " actions of " + who.color + who.name + "&e.", false);
        //    }
        //    else if (Player.left.ContainsKey(message.ToLower()))
        //    {
        //        LeftPlayer who = Player.left[message.ToLower()];
        //        if (Player.GetGroup(message) == Group.Find("operator"))
        //        {
        //            p.SendMessage("You can't undo actions of an operator!"); return;
        //        } if (who.actions.Count == 0)
        //        {
        //            p.SendMessage("There are no actions to undo!"); return;
        //        } int actions = who.actions.Count;
        //        //who.actions.ForEach(delegate(Edit e) { e.Undo(); });
        //        who.actions.Clear();
        //        Player.GlobalChat(p, p.color + "*" + p.name + "&e undid " + actions + " actions of " +
        //                          Player.GetColor(who.name) + who.name + " &f(offline)&e.", false);
        //    }
        //    else { p.SendMessage("No entry found for \"" + message + "\"."); }
        //}
        //public override void Help(Player p)
        //{
        //    p.SendMessage("/undo <name> - Undoes all actions of a player.");
        //}
    }
}