using System;
using System.IO;
using NiL.JS;
using NiL.JS.Core;

namespace JsCore
{
    class Program
    {
        static void Main(string[] args)
        {
            var mainModule = new Module("fakedir/superscript.js", @"");

            Module.ResolveModule += (sender, e) =>
            {
                e.Module = new Module(e.ModulePath, "");
            };

            var @delegate = new Action<string>(Console.WriteLine);
            mainModule.Context.DefineVariable("alert").Assign(JSValue.Marshal(@delegate));
            mainModule.Context.DefineConstructor(typeof(Program));
            mainModule.Context.Eval(File.ReadAllText("test.js"));

            Console.ReadLine();
        }
    }
}
