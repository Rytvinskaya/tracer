using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Tracer
{
    public class TraceResult
    {
        public TraceResult(ConcurrentDictionary<int, ThreadInformation> threadsInfo)
        {
            ThreadsInfo = new List<ThreadInformation>();
            ThreadsInfo.AddRange(threadsInfo.Values);
        }

        public List<ThreadInformation> ThreadsInfo { get; private set; }
    }
}