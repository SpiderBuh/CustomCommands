﻿using CommandSystem;
using NWAPIPermissionSystem;
using PlayerRoles;
using PluginAPI.Core;
using RemoteAdmin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace CustomCommands
{
	public static class Extensions
	{
		public static bool CanRun(this ICommandSender sender, ICustomCommand cmd, ArraySegment<string> args, out string Response, out List<Player> Players, out PlayerCommandSender pSender, byte optArgs = 0)
		{
			Players = new List<Player>();
			pSender = null;
			if (cmd.RequirePlayerSender && !(sender is PlayerCommandSender pS))
			{
				Response = "You must be a player to run this command";
				return false;
			}

			if (cmd.Permission != null && !sender.CheckPermission((PlayerPermissions)cmd.Permission))
			{
				Response = $"You do not have the required permission to execute this command: {cmd.Permission}";
				return false;
			}
			else if (!string.IsNullOrEmpty(cmd.PermissionString) && !sender.CheckPermission(cmd.PermissionString))
			{
				Response = $"You do not have the required permission to execute this command: {cmd.PermissionString}";
				return false;
			}

			if (args.Count < cmd.Usage.Length - optArgs)
			{
				Response = $"Missing argument: {cmd.Usage[args.Count]}";
				return false;
			}

			if (cmd.Usage.Contains("%player%"))
			{
				var index = cmd.Usage.IndexOf("%player%");

				var hubs = RAUtils.ProcessPlayerIdOrNamesList(args, index, out string[] newArgs, false);

				if (hubs.Count < 1)
				{
					Response = $"No player(s) found for: {args.ElementAt(index)}";
					return false;
				}
				else
				{
					foreach (var plr in hubs)
					{
						Players.Add(Player.Get(plr));
					}
				}
			}

			Response = string.Empty;
			return true;
		}

		public static RoleTypeId GetRoleFromString(string role)
		{
			switch (role.ToLower().Replace("scp", string.Empty))
			{
				case "049":
					return RoleTypeId.Scp049;
				case "079":
					return RoleTypeId.Scp079;
				case "939":
					return RoleTypeId.Scp939;
				case "173":
					return RoleTypeId.Scp173;
				case "096":
					return RoleTypeId.Scp096;
				case "106":
				default:
					return RoleTypeId.Scp106;
			}
		}

		public static bool IsValidSCP(this RoleTypeId role)
		{
			return role == RoleTypeId.Scp173 || role == RoleTypeId.Scp049 || role == RoleTypeId.Scp079 || role == RoleTypeId.Scp096 || role == RoleTypeId.Scp106 || role == RoleTypeId.Scp939;
		}

        public static void ScalePlayer(Player p, float s = 1) => ScalePlayer(p, s, s, s);
        public static void ScalePlayer(Player p, float xz, float y) => ScalePlayer(p, xz, y, xz);
        public static void ScalePlayer(Player p, float x, float y, float z) => ScalePlayer(p, x, y, z, Server.GetPlayers());
        public static void ScalePlayer(Player p, float x, float y, float z, List<Player> svrPlrs)
        {
            var nId = p.ReferenceHub.networkIdentity;
            p.ReferenceHub.gameObject.transform.localScale = new UnityEngine.Vector3(1 * x, 1 * y, 1 * z);

            foreach (var player in Server.GetPlayers())
            {
                NetworkConnection nConn = player.ReferenceHub.connectionToClient;

            typeof(NetworkServer).GetMethod("SendSpawnMessage", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { nId, nConn });
        }
    }
}
