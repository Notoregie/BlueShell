using System;
using System.Collections.Generic;
using System.Text;

namespace BlueShell.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public class CommandAttribute : Attribute
    {
        public CommandAttribute(string id, string description, string usage)
        {
            Id = id;
            Description = description;
            Usage = usage;
        }

        public string Id { get; }
        public string Description { get; }
        public string Usage { get; }
    }
}