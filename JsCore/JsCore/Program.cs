using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography.X509Certificates;
using NiL.JS;
using NiL.JS.BaseLibrary;
using NiL.JS.Core;
using NiL.JS.Extensions;

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


            var @delegate = new Action<JSValue>(x=>
            {
                var d = x.As<Function>();

                IFormatter formatter = new BinaryFormatter();
                Stream stream = new FileStream("MyFile.bin", FileMode.Create, FileAccess.Write, FileShare.None);
                formatter.Serialize(stream, d);
                stream.Close();

                var val = d.Call(new Arguments());
                Console.WriteLine(x.ToString());
            });
            mainModule.Context.DefineVariable("alert").Assign(JSValue.Marshal(@delegate));
            mainModule.Context.DefineConstructor(typeof(Program));
            mainModule.Context.Eval(File.ReadAllText("test.js"));

            Console.ReadLine();
        }
    }
}
