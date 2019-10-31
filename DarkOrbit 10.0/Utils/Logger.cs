using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Ow.Utils
{
    class Logger
    {
        public static void Log(string fileName, string message)
        {
            var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + Path.DirectorySeparatorChar + "logs" + Path.DirectorySeparatorChar;

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            fileName += $"_{DateTime.Now.ToString("dd.MM.yyyy")}.txt";

            try
            {
                if (!File.Exists(path))
                {
                    using (FileStream fs = File.Create(path + fileName))
                    {
                        fs.Flush();
                        fs.Close();
                    }
                }

                using (StreamWriter sw = File.AppendText(path + fileName))
                {
                    sw.WriteLine($"[{DateTime.Now.ToString("dd.MM.yyyy HH: mm:ss")}] " + message);
                    sw.Flush();
                    sw.Close();
                }
            }
            catch (Exception e)
            {
                Out.WriteLine("Log void exception: " + e, "Logger.cs");
            }
        }
    }
}
