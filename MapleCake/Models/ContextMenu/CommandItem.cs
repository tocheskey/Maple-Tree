﻿// Project: MapleCake
// File: CommandItem.cs
// Updated By: Jared
// 

using System.Windows.Input;
using MapleCake.Models.Interfaces;

namespace MapleCake.Models.ContextMenu
{
    public class CommandItem : ICommandItem
    {
        public string Text { get; set; }
        public string ToolTip { get; set; }
        public ICommand Command { get; set; }
    }
}