using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Linq;

namespace Tracer
{
    public class XmlSerializer : ISerializer
    {
        public string Serialize(TraceResult traceResult)
        {
            List<ThreadInformation> threadsInfo = traceResult.ThreadsInfo;
            XDocument xDocument = new XDocument(new XElement("root"));

            foreach (ThreadInformation thread in threadsInfo)
            {
                XElement threadXElement = GetThreadXElement(thread);
                xDocument.Root.Add(threadXElement);
            }

            StringWriter stringWriter = new StringWriter();
            using (XmlTextWriter xmlWriter = new XmlTextWriter(stringWriter))
            {
                xmlWriter.Formatting = Formatting.Indented;
                xDocument.WriteTo(xmlWriter);
            }
            return stringWriter.ToString();
        }

        private XElement GetMethodXElement(MethodInformation methodInfo)
        {
            return new XElement(
                "method",
                new XAttribute( "name", methodInfo.Name ),
                new XAttribute( "class", methodInfo.ClassName),
                new XAttribute( "time", methodInfo.ExecutionTime)
                );
        }

        private XElement GetMethodXElementWithChildMethods(MethodInformation methodInfo)
        {
            XElement methodXElement = GetMethodXElement(methodInfo);
            foreach (MethodInformation method in methodInfo.ChildMethods)
            {
                XElement childMethod = GetMethodXElement(method);
                if (method.ChildMethods.Count > 0)
                {
                    childMethod = GetMethodXElementWithChildMethods(method);
                }
                methodXElement.Add(childMethod);
            }
            return methodXElement;
        }

        private XElement GetThreadXElement(ThreadInformation threadInfo)
        {
            XElement threadXElement = new XElement(
                "thread",
                new XAttribute("id", threadInfo.Id),
                new XAttribute("time", threadInfo.ExecutionTime)
                );
            foreach (MethodInformation method in threadInfo.Methods)
            {
                XElement methodXElement = GetMethodXElementWithChildMethods(method);
                threadXElement.Add(methodXElement);
            }
            return threadXElement;
        }
    }
}