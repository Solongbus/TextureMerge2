using System.Windows;
using System.Linq;
using System.Runtime.InteropServices;
using TextureMerge.Utils;

namespace TextureMerge
{
    public partial class App : Application
    {
        public static bool IsCliMode { get; private set; } = false;

        [DllImport("kernel32.dll")]
        static extern bool AttachConsole(int dwProcessId);
        private const int ATTACH_PARENT_PROCESS = -1;

        protected override void OnStartup(StartupEventArgs e)
        {
            // 自动注册文件关联
            FileAssociation.Register();

            if (e.Args.Length > 0 && (e.Args.Contains("-o") || e.Args.Contains("--output")))
            {
                IsCliMode = true;
                AttachConsole(ATTACH_PARENT_PROCESS);
                CommandLineParser.Run(e.Args);
                Shutdown();
            }
            else
            {
                base.OnStartup(e);
            }
        }
    }
}
