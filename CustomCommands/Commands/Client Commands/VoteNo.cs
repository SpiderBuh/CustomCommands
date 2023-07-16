﻿using CommandSystem;
using PluginAPI.Core;
using RemoteAdmin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomCommands.Commands.Client_Commands
{
	[CommandHandler(typeof(ClientCommandHandler))]
	public class VoteNo : ICommand
	{
		public string Command => "no";
		public string[] Aliases => null;
		public string Description => "Vote no on the current vote";

		public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
		{
			if (sender is PlayerCommandSender pSender)
			{
				if (!Plugin.VoteInProgress)
				{
					response = "There is no vote in progress";
					return false;
				}

				var plr = Player.Get(pSender.ReferenceHub);

				if (plr.TemporaryData.Contains("vote_yes") || plr.TemporaryData.Contains("vote_no"))
				{
					response = "You have already voted";
					return false;
				}

				plr.TemporaryData.Override("vote_no", string.Empty);

				response = "You have voted no";
				return false;
			}

			response = "You must be a player to run this command";
			return false;
		}
	}
}
