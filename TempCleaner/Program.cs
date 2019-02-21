using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Threading.Tasks;

namespace TempCleaner
{
    class Program
    {  
        static void Main(string[] args)
        {
            if (!IsAdministrator())
            {
                ClearUserTemp();
                ClearRecycleBin();
            }
            else
            {
                ClearSystemTemp();
                ClearUserTemp();
                ClearRecycleBin();
            }
        }

        public static bool IsAdministrator()
        {
            var identity = WindowsIdentity.GetCurrent();
            var principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }

        enum RecycleFlags : uint
        {
            SHRB_NOCONFIRMATION = 0x00000001,
            SHRB_NOPROGRESSUI = 0x00000002,
            SHRB_NOSOUND = 0x00000004
        }

        private static void ClearSystemTemp()
        {
            try
            {
                var SystemTemp = Environment.GetEnvironmentVariable("TEMP", EnvironmentVariableTarget.Machine);
                var systemTempFiles = Directory.GetFiles(SystemTemp);

                foreach (var fil in systemTempFiles)
                {
                    try
                    {
                        File.SetAttributes(fil, FileAttributes.Normal);
                        
                        File.Delete(fil);
                    }
                    catch (Exception e)
                    {
                        if(e is UnauthorizedAccessException)
                        {
                            continue;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Task.Delay(5000).Wait();
                Environment.Exit(0);
            }
        }

        private static void ClearUserTemp()
        {
            try
            {
                var UserTemp = Environment.GetEnvironmentVariable("TEMP");
                var userTempFiles = Directory.GetFiles(UserTemp);

                foreach (var file in userTempFiles)
                {
                    try
                    {
                        File.SetAttributes(file, FileAttributes.Normal);
                        File.Delete(file);
                    }
                    catch (Exception e)
                    {
                        if(e is UnauthorizedAccessException)
                        {
                            continue;
                        }
                    }
                    
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Task.Delay(5000).Wait();
                Environment.Exit(0);
            }
        }

        [DllImport("Shell32.dll", CharSet = CharSet.Unicode)]
        static extern uint SHEmptyRecycleBin(IntPtr hwnd, string pszRootPath, RecycleFlags dwFlags);

        private static void ClearRecycleBin()
        {
            try
            {                
                uint IsSuccess = SHEmptyRecycleBin(IntPtr.Zero, null, RecycleFlags.SHRB_NOCONFIRMATION);
                Console.WriteLine("Cleared Recycle Bin!");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Task.Delay(5000).Wait();
                Environment.Exit(0);
            }
        }
    }
}
