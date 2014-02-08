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
using System.Threading;
using System.IO;
using System.ComponentModel;


namespace Overv
{
	class AutoSaver
	{
		static int _interval;
		const string backupPath = "levels/backups";

		static int count = 1;
		public AutoSaver(int interval)
		{
			_interval = interval * 1000;

			System.Timers.Timer runner = new System.Timers.Timer(_interval);
			runner.Elapsed += delegate
			{
				Exec();
			};
			Exec();
			runner.Start();
		}

		static void Exec()
		{
			Server.ml.Queue(delegate
			{
				Run();
			});
		}

		static void Run()
		{

			try
			{
				count--;

				Server.levels.ForEach(delegate(Level l)
				{
                    if ( CTF.currLevel == l ) {
                        return;
                    }

					try
					{
						if (l.changed) { l.SaveProperties(); }

						if (count == 0)
						{
							int backupNumber = 1;
							if (Directory.Exists(backupPath + "/" + l.name))
							{
								backupNumber = Directory.GetDirectories(backupPath + "/" + l.name).Length + 1;
							}
							else
							{
								Directory.CreateDirectory(backupPath + "/" + l.name);
							}
							string path = backupPath + "/" + l.name + "/" + backupNumber;
							string previousBackup = backupPath + "/" + l.name + "/" + (backupNumber - 1);
							Directory.CreateDirectory(path);
                            //if (l.Backup(path))
                            //{
                            //    foreach (Player p in Player.players)
                            //    {
                            //        if (p.level == l)
                            //            p.SendMessage("Backup " + backupNumber + " saved.");
                            //    }
                            //    Server.s.Log("Backup " + backupNumber + " saved for " + l.name);
                            //}
						}
					}
					catch
					{
						Server.s.Log("Backup for " + l.name + " has caused an error.");
					}
				});

				if (count <= 0)
				{
					count = 15;
				}
			}
			catch (Exception e) { Server.ErrorLog(e); }
		}
	}
}
