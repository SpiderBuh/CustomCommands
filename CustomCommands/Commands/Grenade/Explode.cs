﻿using CommandSystem;
using PluginAPI.Core;
using System;
using System.Linq;
using Utils;

namespace CustomCommands.Commands.Grenade
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class ExplodeCommand : ICustomCommand
    {
        public string Command => "explode";

        public string[] Aliases => null;

        public string Description => "Causes the player to explode";

        public string[] Usage { get; } = { "%player%" };

        public PlayerPermissions? Permission => null;
        public string PermissionString => "cuscom.grenade";

        public bool RequirePlayerSender => false;

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!sender.CanRun(this, arguments, out response, out var players, out var pSender))
                return false;

            string[] args = arguments.ToArray();

            foreach (Player plr in players)
            {
                if (plr.Role == PlayerRoles.RoleTypeId.Spectator || plr.Role == PlayerRoles.RoleTypeId.Overwatch)
                    continue;
                ExplosionUtils.ServerExplode(plr.ReferenceHub);
            }
            response = "Player successfully detonated";
            return true;
        }
    }
}