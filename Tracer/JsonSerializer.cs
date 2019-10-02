using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;

namespace Tracer
{
public class JsonTracerSerializer : ISerializer
    {
        public string Serialize(TraceResult traceResult)
        {
            JArray threadJArray = new JArray();
            foreach (ThreadInformation thread in traceResult.ThreadsInfo)
            {
                JObject threadJObject = GetThreadJObject(thread);
                threadJArray.Add(threadJObject);
            }
            JObject resultJObject = new JObject
            {
                {"threads", threadJArray }
            };
            StringWriter stringWriter = new StringWriter();
            using (var jsonWriter = new JsonTextWriter(stringWriter))
            {
                jsonWriter.Formatting = Formatting.Indented;
                resultJObject.WriteTo(jsonWriter);
            }
            return stringWriter.ToString();
        }

        private JObject GetMethodJObject(MethodInformation methodInfo)
        {
            return new JObject
            {
                {"name", methodInfo.Name },
                {"class", methodInfo.ClassName },
                {"time", methodInfo.ExecutionTime },
            };
        }

        private JObject GetMethodJObjectWithChildMethods(MethodInformation methodInfo)
        {
            JObject methodJObject = GetMethodJObject(methodInfo);
            JArray methodsJArray = new JArray();
            foreach (MethodInformation method in methodInfo.ChildMethods)
            {
                JObject childMethodJObject = GetMethodJObject(method);
                if (method.ChildMethods.Count > 0)
                {
                    childMethodJObject = GetMethodJObjectWithChildMethods(method);
                }
                methodsJArray.Add(childMethodJObject);
            }
            methodJObject.Add("methods", methodsJArray);
            return methodJObject;
        }

        private JObject GetThreadJObject(ThreadInformation threadInfo)
        {
            JArray methodJArray = new JArray();
            foreach (MethodInformation method in threadInfo.Methods)
            {
                JObject methodJObject = GetMethodJObjectWithChildMethods(method);

                methodJArray.Add(methodJObject);
            }
            return new JObject
            {
                {"id", threadInfo.Id },
                {"time", threadInfo.ExecutionTime },
                {"methods", methodJArray }
            };
        }
    }
}