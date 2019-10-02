using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace Tracer
{
    class MethodTracer
    {
        public MethodTracer()
        {
            _stopwatch = new Stopwatch();
            _childMethods = new List<MethodInformation>();
        }

        private readonly Stopwatch _stopwatch;

        private readonly List<MethodInformation> _childMethods;

        public void AddChildMethod(MethodInformation childMethod)
        {
            _childMethods.Add(childMethod);
        }

        public List<MethodInformation> GetChildMethods()
        {
            return _childMethods;
        }

        public double GetExecutionTime()
        {
            return _stopwatch.ElapsedMilliseconds;
        }

        public void StartTrace()
        {
            _stopwatch.Start();
        }

        public void StopTrace()
        {
            _stopwatch.Stop();
        }
    }
}