using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RDotNet;

namespace MutualFundsComparison.RLanguageEngine
{
    public class RLangEngine
    {
        public REngine engine;
        public RLangEngine(string RHomePath)
        {
            string instanceId = "RLanguageEngine";
            REngine.CreateInstance(instanceId);
            engine = REngine.GetInstanceFromID(instanceId);
        }
    }
}