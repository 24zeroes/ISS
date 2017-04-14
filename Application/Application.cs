using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppPattern
{
    public abstract class Application : IDisposable
    {
        public void Run()
        {
            GetConfiguration();
            InitialiseInputData();
            ProcessData();
            PublishResult();
        }

        public abstract void GetConfiguration();

        public abstract void InitialiseInputData();

        public abstract void ProcessData();

        public abstract void PublishResult();

        public abstract void Dispose();
    }
}
