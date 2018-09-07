﻿using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using JS.Core.Core;
using JS.Core.Extensions;
using NiL.JS;
using NiL.JS.BaseLibrary;

namespace JsCore
{
    class TestJS
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
            mainModule.Context.DefineConstructor(typeof(TestJS));
            mainModule.Context.Eval(File.ReadAllText("test.js"));

            Console.ReadLine();
        }
    }
}