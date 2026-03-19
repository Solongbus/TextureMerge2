using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace TextureMerge.Utils
{
    public static class FileAssociation
    {
        private const string Extension = ".tmproj";
        private const string ProgId = "TextureMerge.Project";
        private const string Description = "TextureMerge Project File";

        /// <summary>
        /// 注册文件关联，使 .tmproj 文件双击时能通过当前程序打开
        /// </summary>
        public static void Register()
        {
            try
            {
                string exePath = ProcessFileName;
                string commandPath = $"\"{exePath}\" \"%1\"";

                // 检查是否已经注册了当前路径
                using (var shellKey = Registry.CurrentUser.OpenSubKey($@"Software\Classes\{ProgId}\shell\open\command"))
                {
                    if (shellKey != null)
                    {
                        string currentValue = shellKey.GetValue("") as string;
                        if (currentValue == commandPath)
                        {
                            return; // 已经注册过了，无需重复操作
                        }
                    }
                }

                // 1. 设置扩展名关联的 ProgId
                using (var key = Registry.CurrentUser.CreateSubKey($@"Software\Classes\{Extension}"))
                {
                    key.SetValue("", ProgId);
                }

                // 2. 设置 ProgId 的描述和打开方式
                using (var key = Registry.CurrentUser.CreateSubKey($@"Software\Classes\{ProgId}"))
                {
                    key.SetValue("", Description);
                    
                    // 设置图标（使用 EXE 的图标）
                    using (var iconKey = key.CreateSubKey("DefaultIcon"))
                    {
                        iconKey.SetValue("", $"\"{exePath}\",0");
                    }

                    // 设置打开命令
                    using (var shellKey = key.CreateSubKey(@"shell\open\command"))
                    {
                        shellKey.SetValue("", commandPath);
                    }
                }

                // 通知 Windows 壳层更改 (SHCNE_ASSOCCHANGED = 0x08000000)
                SHChangeNotify(0x08000000, 0x0000, IntPtr.Zero, IntPtr.Zero);
            }
            catch (Exception ex)
            {
                // 这里静默失败，因为可能没有注册表权限，虽然 CurrentUser 通常是可以写的
                Debug.WriteLine($"无法注册文件关联: {ex.Message}");
            }
        }

        private static string ProcessFileName
        {
            get
            {
                // 获取当前正在运行的可执行文件的完整路径
                return Process.GetCurrentProcess().MainModule.FileName;
            }
        }

        [DllImport("shell32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern void SHChangeNotify(uint wEventId, uint uFlags, IntPtr dwItem1, IntPtr dwItem2);
    }
}
