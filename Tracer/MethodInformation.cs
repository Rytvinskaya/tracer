using System.Collections.Generic;

namespace Tracer
{
    public class MethodInformation
    {
        public string Name { get; private set; }

        public string ClassName { get; private set; }

        public double ExecutionTime { get; private set; }

        public List<MethodInformation> ChildMethods { get; private set; }

        public MethodInformation(string name, string className, double executionTime, List<MethodInformation> childMethods)
        {
            Name = name;
            ClassName = className;
            ExecutionTime = executionTime;
            ChildMethods = new List<MethodInformation>(childMethods);
        }
    }
}