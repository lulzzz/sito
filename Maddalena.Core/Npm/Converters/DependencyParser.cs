using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maddalena.Core.Npm.Model;
using Sprache;

namespace Maddalena.Core.Npm.Converters
{
    class DependencyParser
    {
        private static Parser<int[]> version =
            from str in Parse.Number.DelimitedBy(Parse.Char('.'))
            select str.Select(int.Parse).ToArray();

        private static Parser<NpmDependecy> around = from s in Parse.Char('~')
            from w in Parse.WhiteSpace.Many().Optional()
            from v in version
            select new NpmDependecy(v);

        public static NpmDependecy ParseDependecy(string str)
        {

        }
    }
}
