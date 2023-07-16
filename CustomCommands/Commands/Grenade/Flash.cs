﻿using CommandSystem;
using InventorySystem;
using InventorySystem.Items;
using InventorySystem.Items.ThrowableProjectiles;
using PluginAPI.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Playables;

namespace CustomCommands.Commands
{
	[CommandHandler(typeof(RemoteAdminCommandHandler))]
	public class FlashCommand : ICustomCommand
	{
		public string Command => "flash";

		public string[] Aliases => null;

		public string Description => "Spawns a flashbang at a player's location";

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

				ThrowableItem item = (ThrowableItem)plr.ReferenceHub.inventory.CreateItemInstance(new ItemIdentifier(ItemType.GrenadeFlash, ItemSerialGenerator.GenerateNext()), false);
				Vector3 Pos = plr.Position;
				Pos.y += 1;

				FlashbangGrenade grenade = (FlashbangGrenade)UnityEngine.Object.Instantiate(item.Projectile, Pos, Quaternion.identity);
				grenade.NetworkInfo = new InventorySystem.Items.Pickups.PickupSyncInfo(item.ItemTypeId, item.Weight, item.ItemSerial);
				grenade.Position = Pos;
				grenade.Rotation = Quaternion.identity;
				grenade.GetComponent<Rigidbody>().velocity = new Vector3(UnityEngine.Random.Range(0, 1), UnityEngine.Random.Range(0, 1), UnityEngine.Random.Range(0, 1));

				grenade.PreviousOwner = new Footprinting.Footprint(plr.ReferenceHub);
				Mirror.NetworkServer.Spawn(grenade.gameObject);
				grenade.ServerActivate();
			}

			response = $"#Spawned a flashbang on {players.Count} {(players.Count > 1 ? "players" : "player")}";
			return true;
		}
	}
}
