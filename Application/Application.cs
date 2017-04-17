using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SecurityProvider;
using Sodium;

namespace AppPattern
{
    public abstract class Application : IDisposable
    {
        #region execution_plan
        public void Run()
        {
            GetConfiguration();
            InitialiseInputData();
            ProcessData();
            PublishResult();
        }
        #endregion

        #region methods
        public abstract void GetConfiguration();

        public abstract void InitialiseInputData();

        public abstract void ProcessData();

        public abstract void PublishResult();

        public abstract void Dispose();

        #endregion

        #region properties

        protected KeyPair AppKeyPair;

        protected SecurityCore SecCore;

        #endregion
    }
}
