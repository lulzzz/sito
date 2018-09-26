using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sprache;

namespace Maddalena.Core.Npm.Model
{
    public class NpmDependecy
    {
        private int[] v;

        public NpmDependecy(int[] v)
        {
            this.v = v;
        }

        public string Name { get; set; }

        public Version From { get; set; }
    }
}
