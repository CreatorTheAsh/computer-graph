using System;
using System.Linq;

namespace ConsoleInterface
{
    public static class KeyHandler
    {
        public static (string source, string destination, string format) GetValues(string[] args)
        {
            var source = ParseArgument("--source", args);
            var outputFormat = ParseArgument("--goal-format", args);
            if (!TryParseArgument("--output", args, out var output))
            {
                output = source.Substring(0, source.LastIndexOf('.'));
            }
            return (source, output, outputFormat);
        }

        private static string ParseArgument(string parameter, string[] args)
        {
            string arg = args.FirstOrDefault(x => x.Contains(parameter));

            if (string.IsNullOrEmpty(arg))
            {
                throw new ArgumentException($"No parameter with key {parameter}");
            }

            string[] split = arg.Split('=');
            if (split.Length != 2 || split[0] != parameter)
            {
                throw new ArgumentException($"Invalid value for {parameter}");
            }
            return split[1];
        }

        private static bool TryParseArgument(string parameter, string[] args, out string res)
        {
            try
            {
                res = ParseArgument(parameter, args);
                return true;
            }
            catch (ArgumentException)
            {
                res = "";
                return false;
            }
        }
    }
}
