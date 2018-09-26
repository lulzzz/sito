using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sprache;

namespace Maddalena.Core.Npm.Model
{
    public class NpmDependecy
    {
        public string Name { get; internal set; }

        public string Version { get; internal set; }

        public NpmDependecyType Type { get; internal set; }

        internal NpmDependecy(string v, NpmDependecyType type)
        {
            Version = v;
            Type = type;
        }
    }
}
