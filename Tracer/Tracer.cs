using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;

namespace Tracer
{
    public class Tracer : ITracer
    {
        public Tracer()
        {
            _threadTracers = new ConcurrentDictionary<int, ThreadTracer>();
            _threadsInfo = new ConcurrentDictionary<int, ThreadInformation>();
        }

        private readonly ConcurrentDictionary<int, ThreadTracer> _threadTracers;

        private readonly ConcurrentDictionary<int, ThreadInformation> _threadsInfo;

        public TraceResult GetTraceResult()
        {
            return new TraceResult(_threadsInfo);
        }



        private ThreadInformation GetThreadInfoById(int threadId)
        {
            _threadsInfo.TryGetValue(threadId, out var threadInfo);      
            return threadInfo;
        }

        private ThreadTracer GetCurrentThreadTracer()
        {
            int threadId = Thread.CurrentThread.ManagedThreadId;
            _threadTracers.TryGetValue(threadId, out var threadTracer);
            return threadTracer;
        }

        public void StartTrace()
        {
            ThreadTracer threadTracer = GetCurrentThreadTracer();
            if (threadTracer == null)
            {
                int currentThreadId = Thread.CurrentThread.ManagedThreadId;
                threadTracer = new ThreadTracer();       
                _threadTracers.TryAdd(currentThreadId, threadTracer);
                
            }
            threadTracer.StartTrace();
        }

        public void StopTrace()
        {
            ThreadTracer threadTracer = GetCurrentThreadTracer();
            threadTracer.StopTrace();
            int currentThreadId = Thread.CurrentThread.ManagedThreadId;
            ThreadInformation threadInfo = GetThreadInfoById(currentThreadId);
            if (threadInfo == null)
            {
                List<MethodInformation> threadMethodInfos = threadTracer.GetThreadMethodList();
                threadInfo = new ThreadInformation(currentThreadId, threadMethodInfos);
                _threadsInfo.TryAdd(currentThreadId, threadInfo);
            }
        }
    }
}