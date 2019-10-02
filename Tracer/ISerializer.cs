namespace Tracer
{
    public interface ISerializer
    {
        string Serialize(TraceResult traceResult);
    }
}