using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using Maddalena.Core.Javascript;
using Maddalena.Core.Javascript.BaseLibrary;
using Maddalena.Core.Javascript.Core;
using Maddalena.Core.Javascript.Extensions;
using Maddalena.Core.Npm;

namespace Maddalena.Core
{
    class TestJS
    {
        static void Main(string[] args)
        {
            var t = NpmClient.DownloadWithDependencies("express", @"D:\CULO");
            t.Wait();

            var mainModule = new Module("fakedir/superscript.js", @"");

            Module.ResolveModule += (sender, e) =>
            {
                e.Module = new Module(e.ModulePath, "");
            };


            var @delegate = new Action<GeneratorIterator>(x=>
            {
                for (int i = 0; i < 1000; i++)
                {
                    var empty = new Arguments();
                    Console.WriteLine(x.next(empty).value.toString(empty));
                }

                IFormatter formatter = new BinaryFormatter();
                Stream stream = new FileStream("MyFile.bin", FileMode.Create, FileAccess.Write, FileShare.None);
                formatter.Serialize(stream, x);
                stream.Close();
            });
            mainModule.Context.DefineVariable("alert").Assign(JSValue.Marshal(@delegate));
            mainModule.Context.DefineConstructor(typeof(TestJS));
            mainModule.Context.Eval(File.ReadAllText("test.js"));

            Console.ReadLine();
        }
    }
}
