using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tracer;

namespace ConsoleApp
{
    class Program
    {
        public class Foo
        {
            private Bar _bar;
            private ITracer _tracer;

            internal Foo(ITracer tracer)
            {
                _tracer = tracer;
                _bar = new Bar(_tracer);
            }

            public void MyMethod()
            {
                _tracer.StartTrace();
                _bar.InnerMethod();
                _tracer.StopTrace();
            }

            public void MyMethod2()
            {
                _tracer.StartTrace();
                _tracer.StopTrace();
            }
        }

        public class Bar
        {
            private ITracer _tracer;

            internal Bar(ITracer tracer)
            {
                _tracer = tracer;
            }

            public void InnerMethod()
            {
                _tracer.StartTrace();
                
                Thread.Sleep(100);
                _tracer.StopTrace();
            }
        }

        public void Method(object o)
        {
            Tracer.Tracer tracer = (Tracer.Tracer)o;
            tracer.StartTrace();
            Thread.Sleep(100);
            tracer.StopTrace();
        }

        static void Main(string[] args)
        {
            Program program = new Program();
            Thread thread = new Thread(new ParameterizedThreadStart(program.Method));

            ITracer tracer = new Tracer.Tracer();
            Foo foo = new Foo(tracer);
            foo.MyMethod();
            foo.MyMethod2();
            thread.Start(tracer);
            thread.Join();

            XmlSerializer xmlTracerSerializer = new XmlSerializer();
            JsonTracerSerializer jsonTracerSerializer = new JsonTracerSerializer();
            TraceResult traceResult = tracer.GetTraceResult();
            string xml = xmlTracerSerializer.Serialize(traceResult);
            string json = jsonTracerSerializer.Serialize(traceResult);

            FileSaver fs = new FileSaver("trace.json");
            fs.Print(xml);
            fs.Print(json);

            ConsolePrinter cp = new ConsolePrinter();
            cp.Print(xml);
            cp.Print(json);

            Console.ReadKey();
        }
    }
}
