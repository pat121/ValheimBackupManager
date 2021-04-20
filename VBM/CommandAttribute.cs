using System;

namespace VBM
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    class CommandAttribute : Attribute
    {
        public string CommandName { get; private set; }

        public CommandAttribute(string commandName)
        {
            CommandName = commandName;
        }

        public bool CommandNameEquals(string command)
        {
            return CommandName.Equals(command, StringComparison.OrdinalIgnoreCase);
        }
    }
}
