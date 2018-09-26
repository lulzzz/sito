using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maddalena.Core.Npm.Model;
using Sprache;

namespace Maddalena.Core.Npm.Converters
{
    static class DependencyParser
    {
        public static Parser<string> version =
            from str in Parse.Number.DelimitedBy(Parse.Char('.'))
            select string.Join(".", str);

        private static Parser<NpmDependecy> precise = 
            from w in Parse.WhiteSpace.Many().Optional()
            from v in version
            select new NpmDependecy(v, NpmDependecyType.Precise);

        private static Parser<NpmDependecy> any = from s in Parse.Char('*')
            from w in Parse.WhiteSpace.Many().Optional()
            select new NpmDependecy(null, NpmDependecyType.Any);

        private static Parser<NpmDependecy> around = from s in Parse.Char('~')
            from w in Parse.WhiteSpace.Many().Optional()
            from v in version
            select new NpmDependecy(v, NpmDependecyType.Around);

        private static Parser<NpmDependecy> lower = from s in Parse.Char('<')
            from w in Parse.WhiteSpace.Many().Optional()
            from v in version
            select new NpmDependecy(v, NpmDependecyType.Lower);

        private static Parser<NpmDependecy> greater = from s in Parse.Char('>')
            from w in Parse.WhiteSpace.Many().Optional()
            from v in version
            select new NpmDependecy(v, NpmDependecyType.Greater);

        private static Parser<NpmDependecy> lowerOrEqual = from s in Parse.String("<=")
            from w in Parse.WhiteSpace.Many().Optional()
            from v in version
            select new NpmDependecy(v, NpmDependecyType.LowerOrEqual);

        private static Parser<NpmDependecy> greaterOrEqual = from s in Parse.String(">=")
            from w in Parse.WhiteSpace.Many().Optional()
            from v in version
            select new NpmDependecy(v, NpmDependecyType.GreaterOrEqual);

        private static Parser<NpmDependecy[]> All =
            any.Or(precise).Or(around.Or(lower).Or(greater).Or(lowerOrEqual).Or(greaterOrEqual))
                .DelimitedBy(Parse.WhiteSpace)
                .Select(x => x.ToArray());

        public static NpmDependecy[] FromString(string str, string name)
        {
            var array = All.Parse(str);

            for (int i = 0; i < array.Length; i++)
            {
                array[i].Name = name;
            }

            return array;
        }
    }
}
