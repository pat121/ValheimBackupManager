using System;
using System.IO;
using System.Reflection;

namespace VBM
{
    static class Program
    {
        static void Main(string[] args)
        {
            Utility.CheckOS();             // make sure OS is Windows or Linux
            Utility.CheckGameInstalled();  // make sure the OS-specific game save directory exists
            Utility.Initialize();          // create backups directory if it doesn't already exist

            if (args.Length == 0)
            {
                Utility.ShowHelp();
                return;
            }

            var command = args[0];
            var arguments = args.Length > 1 ? args[1..] : Array.Empty<string>();
            
            var method = GetMethod(command);
            if (method == null)
                Utility.PrintErrorAndExit("Command not recognized");

            var info = new CommandInfo(method);

            if (arguments.Length < info.RequiredArgCount)
                Utility.PrintErrorAndExit("Too few arguments for command " + GetCommandSignature(method));
            if (arguments.Length > info.ArgCount)
                Utility.PrintErrorAndExit("Too many arguments for command " + GetCommandSignature(method));

            var methodArgs = new object[info.ArgCount];

            for (var i = 0; i < info.ArgCount; i++)
            {
                if (i >= arguments.Length)
                    methodArgs[i] = info.Parameters[i].DefaultValue;
                else
                    methodArgs[i] = ConvertToArgType(arguments[i], info.ArgTypes[i]);
            }

            var resultAsObject = method.Invoke(null, methodArgs);
            if (resultAsObject == null)
            {
                Console.WriteLine("Command returned void");
                return;
            }
            var result = (Result)resultAsObject;
            switch (result.ResultType)
            {
                case ResultType.Canceled:
                    Utility.Print(result.ToString(), ConsoleColor.Yellow);
                    break;
                case ResultType.Failure:
                    Utility.Print(result.ToString(), ConsoleColor.Red);
                    break;
                case ResultType.Success:
                    Utility.Print(result.ToString(), ConsoleColor.Green);
                    break;
            }
        }

        public static object ConvertToArgType(string argument, Type argType)
        {
            try
            {
                if (argType == typeof(string))
                    return argument;
                if (argType == typeof(int))
                    return int.Parse(argument);

                if (argType == typeof(double))
                    return double.Parse(argument);

                if (argType == typeof(float))
                    return float.Parse(argument);
            }
            catch (Exception)
            {
                var msg = string.Format("Could not convert string value \"{0}\" to required type \"{1}\"", argument, argType.Name);
                Utility.PrintErrorAndExit(msg);
            }
            return null;  // this line is only included because otherwise the program would not compile
        }
        public static string GetCommandSignature(MethodInfo method)
        {
            var sb = new System.Text.StringBuilder(method.Name);

            foreach (var param in method.GetParameters())
            {
                sb.Append(" <");
                sb.Append(param.ParameterType.Name);
                sb.Append(' ');
                sb.Append(param.Name);
                sb.Append('>');
            }
            return sb.ToString();
        }
        public static MethodInfo GetMethod(string command)
        {
            var methods = typeof(Commands).GetMethods();
            
            foreach (var m in methods)
            {
                if (Attribute.GetCustomAttribute(m, typeof(CommandAttribute)) is CommandAttribute attr)
                    if (attr.CommandNameEquals(command))
                        return m;
            }
            return null;
        }
    }

    struct CommandInfo
    {
        public int ArgCount { get; }
        public Type[] ArgTypes { get; }
        public string Name { get; }
        public int OptionalArgCount { get; }
        public ParameterInfo[] Parameters { get; }
        public int RequiredArgCount { get; }

        public CommandInfo(MethodInfo method)
        {
            Parameters = method.GetParameters();
            ArgCount = Parameters.Length;
            ArgTypes = new Type[ArgCount];
            Name = method.Name;
            OptionalArgCount = 0;
            RequiredArgCount = 0;

            for (var i = 0; i < ArgCount; i++)
            {
                var param = Parameters[i];
                ArgTypes[i] = param.ParameterType;

                if (param.IsOptional)
                {
                    OptionalArgCount = ArgCount - RequiredArgCount;
                    break;
                }
                RequiredArgCount++;
            }
        }
    }
}
