using System;

namespace Tracer
{
    public class ConsolePrinter : IPrinter
    {
        public void Print(string serializedResult)
        {
            Console.WriteLine(serializedResult);
        }
    }
}