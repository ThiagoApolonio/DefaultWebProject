using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Text;
using DefaultWebProject.Controllers;

namespace DefaultWebProject.Log
{
    class GravarLog
    {

        private string File { get; set; }
        private string Path { get; set; }
        private string[] Lines { get; set; }

        public GravarLog()
        {
            Path = $@"{AppDomain.CurrentDomain.BaseDirectory}Log\";
            File = this.Path + DateTime.Now.ToString("yyyyMMddHHmmss") + ".txt";

            if (!Directory.Exists(Path))
            {
                Directory.CreateDirectory(Path);
            }
        }

        public GravarLog(bool erro, string nameLog)
        {
            Path = $@"{AppDomain.CurrentDomain.BaseDirectory}Log\";
            File = this.Path + nameLog + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".txt";

            if (!Directory.Exists(Path))
            {
                Directory.CreateDirectory(Path);
            }
        }

        public GravarLog(string logFile)
        {
            this.Path = $@"{AppDomain.CurrentDomain.BaseDirectory}Log\";
            this.File = logFile;

            if (!Directory.Exists(Path))
            {
                Directory.CreateDirectory(Path);
            }
        }

        public GravarLog(bool logGlobal)
        {
            this.Path = $@"{AppDomain.CurrentDomain.BaseDirectory}Log\";
            this.File = JournalVouchersController.LogFile;

            if (!Directory.Exists(Path))
            {
                Directory.CreateDirectory(Path);
            }
        }

        public string Escrever(string registro)
        {
            try
            {
                StreamWriter writer = new StreamWriter(File, true);
                using (writer)
                {
                    // Escreve uma nova linha no final do arquivo
                    writer.WriteLine(" ## " + DateTime.Now.ToString("yyyyMMddHHmmss") + " ### " + registro);
                }

                return File;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}