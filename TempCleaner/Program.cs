using System;
using System.IO;

namespace TempCleaner
{
    class Program
    {  
        static void Main(string[] args)
        {
            // Define Root path
            var UserTemp = Environment.GetEnvironmentVariable("TEMP");
            var SystemTemp = Environment.GetEnvironmentVariable("TEMP", EnvironmentVariableTarget.Machine);            

            try
            {
                Directory.Delete(UserTemp, true);
                Directory.Delete(SystemTemp, true);                
            }
            catch (Exception)
            {

            }            
        }
    }       
    
}
