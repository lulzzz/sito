using Maddalena.Core.Javascript.Core;

namespace Maddalena.Core.Javascript
{
    public enum MessageLevel
    {
        Regular = 0,
        Recomendation,
        Warning,
        CriticalWarning,
        Error
    }

    public delegate void CompilerMessageCallback(MessageLevel level, CodeCoordinates coords, string message);
    public delegate void InternalCompilerMessageCallback(MessageLevel level, int position, int length, string message);
}
