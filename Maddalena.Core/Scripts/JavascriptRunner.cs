using System;
using Maddalena.Core.Javascript;
using Maddalena.Core.Javascript.Core;
using Maddalena.Core.Scripts.Model;

namespace Maddalena.Core.Scripts.Js
{
    public static class JavascriptRunner
    {
        static JavascriptRunner()
        {
            Module.ResolveModule += ResolveModule;
        }

        private static void ResolveModule(Module sender, ResolveModuleEventArgs e)
        {
            e.Module = new Module(e.ModulePath, "");
        }

        public static void Run(ScriptContext context)
        {
            var mainModule = new Module($"maddalena:{context.Script.Name}", @"");
            mainModule.Context.DefineVariable("system").Assign(JSValue.Wrap(context.SystemInterface));
            mainModule.Context.Eval(context.Script.Source);
        }
    }
}