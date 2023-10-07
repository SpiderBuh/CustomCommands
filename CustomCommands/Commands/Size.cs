using CommandSystem;
using Mirror;
using PluginAPI.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CustomCommands.Commands
{
	[CommandHandler(typeof(RemoteAdminCommandHandler))]
	public class Size : ICustomCommand
	{
		public string Command => "size";

		public string[] Aliases { get; } = { "scale" };
		public string Description => "Modify the size of a specified player";
		public string[] Usage { get; } = { "%player%", "[x]", "[y]", "[z]" };

		public PlayerPermissions? Permission => null;
		public string PermissionString => "cuscom.size";
		public bool RequirePlayerSender => true;

		public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
		{
			if (!sender.CanRun(this, arguments, out response, out var players, out var pSender, 3))
				return false;

			float x = float.TryParse(arguments.ElementAtOrDefault(2), out float tx) ? tx : 1;
			float y = float.TryParse(arguments.ElementAtOrDefault(3), out float ty) ? ty : x;
			float z = float.TryParse(arguments.ElementAtOrDefault(4), out float tz) ? tz : x;

            var svrPlrs = Server.GetPlayers();

			foreach (var p in players)
				Extensions.ScalePlayer(p, x, y, z, svrPlrs);

			response = $"Scale of {players.Count} {(players.Count != 1 ? "players have" : "player has")} been set to {x}, {y}, {z}";
			return true;
		}
	}
}
