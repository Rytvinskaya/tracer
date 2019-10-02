using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace Tracer
{
    public class ThreadTracer
    {
        public ThreadTracer()
        {
            _methodTracers = new Stack<MethodTracer>();
            _methodInfoList = new List<MethodInformation>();
        }

        private readonly Stack<MethodTracer> _methodTracers;

        private MethodTracer _currentMethodTracer;

        private readonly List<MethodInformation> _methodInfoList;

        public List<MethodInformation> GetThreadMethodList()
        {
            return _methodInfoList;
        }

        public void StartTrace()
        {
            if (_currentMethodTracer != null)
            {
                _methodTracers.Push(_currentMethodTracer);
            }
            _currentMethodTracer = new MethodTracer();
            _currentMethodTracer.StartTrace();
        }

        public void StopTrace()
        {
            _currentMethodTracer.StopTrace();
            StackTrace stackTrace = new StackTrace();
            MethodBase method = stackTrace.GetFrame(2).GetMethod();
            string methodName = method.Name;
            string className = method.ReflectedType.Name;
            double methodExecutionTime = _currentMethodTracer.GetExecutionTime();
            List<MethodInformation> methodInfos = _currentMethodTracer.GetChildMethods();
            MethodInformation methodInfo = new MethodInformation(methodName, className, methodExecutionTime, methodInfos);
            if (_methodTracers.Count > 0)
            {
                _currentMethodTracer = _methodTracers.Pop();
                _currentMethodTracer.AddChildMethod(methodInfo);
            }
            else
            {
                _methodInfoList.Add(methodInfo);
                _currentMethodTracer = null;
            }
        }
    }
}