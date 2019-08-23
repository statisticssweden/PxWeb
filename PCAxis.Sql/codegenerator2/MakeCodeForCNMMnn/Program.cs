using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace generateClassesForQueryLib {
    class Program {
        static void Main(string[] args) {
            /* 
             2.1 used to files
            string masterFile = @"C:\kode\assembla6\PCAxis.Sql\generator\master.xml";
            string masterFile2 = @"C:\kode\assembla6\PCAxis.Sql\generator\for_SqlDbConfig_21.xml";
            string commentFile = @"C:\kode\assembla6\PCAxis.Sql\generator\CommentsInModel_21.xml";
            new Generator(masterFile, masterFile2,commentFile);
            */

            new Generator(ConfigurationManager.AppSettings["outroot"], "24");
           

            Console.WriteLine("done. Press Enter!");
            Console.ReadLine();
        }
    }
}
