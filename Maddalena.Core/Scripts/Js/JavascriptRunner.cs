﻿using System;
using System.Threading.Tasks;
using Maddalena.Core.Javascript;
using Maddalena.Core.Javascript.Core;
using Maddalena.Core.Scripts.Model;
using Console = Maddalena.Core.Scripts.Model.Console;

namespace Maddalena.Core.Scripts.Js
{
    public class JavascriptRunner
    {
        private IServiceProvider _services;

        public JavascriptRunner(IServiceProvider services)
        {
            _services = services;
            Module.ResolveModule += ResolveModule;
        }

        private void ResolveModule(Module sender, ResolveModuleEventArgs e)
        {
            e.Module = new Module(e.ModulePath, "");
        }

        public Task<ScriptContext> Run(Script script)
        {
            var mainModule = new Module($"maddalena:{script.Name}", @"");

            var context = new ScriptContext
            {
                Console = new Console(),
                Script = script
            };

            try
            {
                mainModule.Context.DefineVariable("console").Assign(JSValue.Wrap(context.Console));

                //mainModule.Context.DefineConstructor(typeof(TestJS));
                mainModule.Context.Eval(script.Source);

                return Task.FromResult(context);
            }
            catch (Exception e)
            {
                context.Exception = e;
            }

            return Task.FromResult(context);
        }
    }
}