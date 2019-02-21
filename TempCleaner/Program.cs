using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace TempCleaner
{
    class Program
    {  
        static void Main(string[] args)
        {
            try
            {
                var UserTemp = Environment.GetEnvironmentVariable("TEMP");
                var SystemTemp = Environment.GetEnvironmentVariable("TEMP", EnvironmentVariableTarget.Machine);
                Directory.Delete(UserTemp, true);
                Directory.Delete(SystemTemp, true);
                ClearRecycleBin();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Task.Delay(5000).Wait();
                Environment.Exit(0);
            }            
        }

        enum RecycleFlags : uint
        {
            SHRB_NOCONFIRMATION = 0x00000001,
            SHRB_NOPROGRESSUI = 0x00000002,
            SHRB_NOSOUND = 0x00000004
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
