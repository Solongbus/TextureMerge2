using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using ImageMagick;

namespace TextureMerge
{
    public static class CommandLineParser
    {
        public static void Run(string[] args)
        {
            try
            {
                string redPath = GetArgValue(args, "-r", "--red");
                string greenPath = GetArgValue(args, "-g", "--green");
                string bluePath = GetArgValue(args, "-b", "--blue");
                string alphaPath = GetArgValue(args, "-a", "--alpha");
                string outputPath = GetArgValue(args, "-o", "--output");
                string colorStr = GetArgValue(args, "-c", "--color") ?? "#000000";
                string depthStr = GetArgValue(args, "-d", "--depth") ?? "-1";

                if (string.IsNullOrEmpty(outputPath))
                {
                    Console.WriteLine("Error: Output path is required (-o or --output).");
                    return;
                }

                if (string.IsNullOrEmpty(redPath) && string.IsNullOrEmpty(greenPath) && 
                    string.IsNullOrEmpty(bluePath) && string.IsNullOrEmpty(alphaPath))
                {
                    Console.WriteLine("Error: At least one input channel must be specified.");
                    return;
                }

                Merge merge = new Merge();
                if (!string.IsNullOrEmpty(redPath)) merge.LoadChannel(redPath, Channel.Red, Channel.Red);
                if (!string.IsNullOrEmpty(greenPath)) merge.LoadChannel(greenPath, Channel.Green, Channel.Green);
                if (!string.IsNullOrEmpty(bluePath)) merge.LoadChannel(bluePath, Channel.Blue, Channel.Blue);
                if (!string.IsNullOrEmpty(alphaPath)) merge.LoadChannel(alphaPath, Channel.Alpha, Channel.Red);

                MagickColor defaultColor = new MagickColor(colorStr);
                int depth = int.Parse(depthStr);

                if (!merge.CheckResolution(out uint width, out uint height))
                {
                    Console.WriteLine("Error: Input images must have the same resolution.");
                    return;
                }

                var result = merge.DoMerge(defaultColor, depth);
                result.Image.Write(outputPath);
                Console.WriteLine($"Successfully merged to {outputPath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        private static string GetArgValue(string[] args, string shortFlag, string longFlag)
        {
            for (int i = 0; i < args.Length; i++)
            {
                if ((args[i] == shortFlag || args[i] == longFlag) && i + 1 < args.Length)
                {
                    return args[i + 1];
                }
            }
            return null;
        }

        public static string CreateCommand(string red, string green, string blue, string alpha, string output, string color, int depth)
        {
            string exePath = Process.GetCurrentProcess().MainModule.FileName;
            var cmd = $"\"{exePath}\"";
            if (!string.IsNullOrEmpty(red)) cmd += $" -r \"{red}\"";
            if (!string.IsNullOrEmpty(green)) cmd += $" -g \"{green}\"";
            if (!string.IsNullOrEmpty(blue)) cmd += $" -b \"{blue}\"";
            if (!string.IsNullOrEmpty(alpha)) cmd += $" -a \"{alpha}\"";
            if (!string.IsNullOrEmpty(output)) cmd += $" -o \"{output}\"";
            if (color != "#000000") cmd += $" -c \"{color}\"";
            if (depth != -1) cmd += $" -d {depth}";
            return cmd;
        }
    }
}
