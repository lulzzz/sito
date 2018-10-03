using System.IO;
using System.Text;
using System.Threading.Tasks;
using Maddalena.Core.GridFs;
using Maddalena.Core.Javascript;
using Maddalena.Core.Javascript.Core;

namespace Maddalena.Core.Scripts.Js
{
    public class JavascriptRunner
    {
        private IGridFileSystem _fs;

        public JavascriptRunner(IGridFileSystem fs)
        {
            _fs = fs;
            Module.ResolveModule += ResolveModule;
        }

        private void ResolveModule(Module sender, ResolveModuleEventArgs e)
        {
            e.Module = new Module(e.ModulePath, "");
        }

        public Task Run(Script script)
        {
            var mainModule = new Module($"maddalena:{script.Name}", @"");
            mainModule.Context.DefineVariable("console").Assign(JSValue.Wrap(new Console()));

            mainModule.Context.DefineConstructor(typeof(TestJS));
            mainModule.Context.Eval(script.Source);

            return Task.CompletedTask;
        }
    }
}