using CommandSystem;
using static CustomCommands.Extensions;
using PlayerRoles;
using PluginAPI.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomCommands.Commands.Events
{
    public class TheNutJar
    {
        [CommandHandler(typeof(RemoteAdminCommandHandler))]
        public class NutJarStartCommand : ICustomCommand
        {
            public string Command => "NutJar";

            public string[] Aliases => null;

            public string Description => "Starts the NutJar event";

            public string[] Usage { get; } = { };
            public PlayerPermissions? Permission => null;
            public string PermissionString => "cuscom.events";
            public bool RequirePlayerSender => true;

            public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
            {
                if (!sender.CanRun(this, arguments, out response, out var players, out var pSender))
                    return false;

                var svrPlrs = Server.GetPlayers();

                foreach (var plr in Player.GetPlayers().Where(p => p.IsSCP))
                {
                    plr.SetRole(RoleTypeId.Scp173);
                    ScalePlayer(plr, UnityEngine.Random.Range(0.5f, 0.75f), UnityEngine.Random.Range(0.5f, 0.75f), UnityEngine.Random.Range(0.5f, 0.75f), svrPlrs);
                }
                Plugin.CurrentEvent = EventType.NutJar;
                Cassie.Message("pitch_1.25 here they come");
                response = "The NutJar event round has begun";
                return true;
            }
        }
    }
}
