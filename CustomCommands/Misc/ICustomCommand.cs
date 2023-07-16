﻿using CommandSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomCommands
{
	public interface ICustomCommand : ICommand, IUsageProvider
	{
		PlayerPermissions? Permission { get; }
		string PermissionString { get; }
		bool RequirePlayerSender { get; }
	}
}
