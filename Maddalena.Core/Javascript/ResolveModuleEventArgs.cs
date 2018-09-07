using System;

namespace NiL.JS
{
    public delegate void ResolveModuleHandler(Module sender, ResolveModuleEventArgs e);

    public class ResolveModuleEventArgs : EventArgs
    {
        public string ModulePath { get; private set; }
        public Module Module { get; set; }

        /// <summary>
        /// Indicates that module will be added to cache. True by default
        /// </summary>
        public bool AddToCache { get; set; }

        public ResolveModuleEventArgs(string moduleName)
        {
            ModulePath = moduleName;
            AddToCache = true;
        }
    }
}
