using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quartz;
using SecurityProvider;

namespace ISS
{
    class Display : IJob
    {
        private SecurityCore SecCore;

        public void Execute(IJobExecutionContext context)
        {
            JobDataMap dataMap = context.JobDetail.JobDataMap;

            SecCore = (SecurityCore)dataMap["security"];

            Console.Clear();

            Header();

            foreach (var task in SecCore.Tasks)
            {
                Console.Write("║");
                Console.Write($"{task.Name}" + "                   ".Substring(0, 19 - task.Name.Length));
                Console.Write("║");
                Console.Write($"{task.Status}"  + "            ".Substring(0, 12 - task.Status.Length));
                Console.Write("║");
                Console.Write($"{task.ExecCount}" + "              ".Substring(0, 14 - task.ExecCount.ToString().Length));
                Console.Write("║");
                Console.Write($"{task.Rule}" + "                     ".Substring(0, 21 - task.Rule.Length));
                Console.WriteLine("║");
            }

            Footer();
        }

        public void Header()
        {
            Console.WriteLine("╔═══════════════════╦════════════╦══════════════╦═════════════════════╗");
            Console.WriteLine("║     Task name     ║   Status   ║  Exec Count  ║       Rule          ║");
            Console.WriteLine("╠═══════════════════╬════════════╬══════════════╬═════════════════════╣");
        }

        public void Footer()
        {
            Console.WriteLine("╚═══════════════════╩════════════╩══════════════╩═════════════════════╝");
        }
    }
}
