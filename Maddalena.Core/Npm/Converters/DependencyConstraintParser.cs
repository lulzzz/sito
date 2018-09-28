using System;
using System.Linq;
using Maddalena.Core.Npm.Model;
using Sprache;

namespace Maddalena.Core.Npm.Converters
{
    static class DependencyConstraintParser
    {
        public static Parser<NpmVersionNumber> version =
            from v in Parse.Char('v').Optional()
            from v2 in Parse.WhiteSpace.Optional()
            from str in Parse.Number.DelimitedBy(Parse.Char('.').Or(Parse.Char('x')))
            select new NpmVersionNumber(str.Select(int.Parse).ToArray());

        private static Parser<NpmDependecyConstraint> precise = 
            from w in Parse.WhiteSpace.Many().Optional()
            from ug in Parse.Char('=').Optional()
            from w2 in Parse.WhiteSpace.Many().Optional()
            from v in version
            select new NpmDependecyConstraint(v, NpmDependecyType.Precise);

        private static Parser<NpmDependecyConstraint> latest = from s in Parse.String("latest")
                                from w in Parse.WhiteSpace.Many().Optional()
                                select new NpmDependecyConstraint(null, NpmDependecyType.Latest);


        private static Parser<NpmDependecyConstraint> any = from s in Parse.Char('*')
            from w in Parse.WhiteSpace.Many().Optional()
            select new NpmDependecyConstraint(null, NpmDependecyType.Any);

        private static Parser<NpmDependecyConstraint> forbidden = from s in Parse.Char('x')
                from w in Parse.WhiteSpace.Many().Optional()
                select new NpmDependecyConstraint(null, NpmDependecyType.Forbidden);

        /***
       
            http://fredkschott.com/post/2014/02/npm-no-longer-defaults-to-tildes/
            In the simplest terms, the tilde matches the most recent minor version
            (the middle number).
            ~1.2.3 will match all 1.2.x versions but will miss 1.3.0.

            The caret, on the other hand, is more relaxed.
            It will update you to the most recent major version (the first number).
            ^1.2.3 will match any 1.x.x release including 1.3.0, but will hold off on 2.0.0.
        ***/


        private static Parser<NpmDependecyConstraint> minor = from s in Parse.Char('~')
            from w in Parse.WhiteSpace.Many().Optional()
            from v in version
            select new NpmDependecyConstraint(v, NpmDependecyType.Minor);

        private static Parser<NpmDependecyConstraint> major = from s in Parse.Char('^')
                                from w in Parse.WhiteSpace.Many().Optional()
                                from v in version
                                select new NpmDependecyConstraint(v, NpmDependecyType.Major);

        private static Parser<NpmDependecyConstraint> lower = from s in Parse.Char('<')
            from w in Parse.WhiteSpace.Many().Optional()
            from v in version
            select new NpmDependecyConstraint(v, NpmDependecyType.Lower);

        private static Parser<NpmDependecyConstraint> greater = from s in Parse.Char('>')
            from w in Parse.WhiteSpace.Many().Optional()
            from v in version
            select new NpmDependecyConstraint(v, NpmDependecyType.Greater);

        private static Parser<NpmDependecyConstraint> lowerOrEqual = from s in Parse.String("<=")
            from w in Parse.WhiteSpace.Many().Optional()
            from v in version
            select new NpmDependecyConstraint(v, NpmDependecyType.LowerOrEqual);

        private static Parser<NpmDependecyConstraint> greaterOrEqual = from s in Parse.String(">=")
            from w in Parse.WhiteSpace.Many().Optional()
            from v in version
            select new NpmDependecyConstraint(v, NpmDependecyType.GreaterOrEqual);

        private static Parser<NpmDependecyConstraint[]> All =
              any.Or(latest.Or(forbidden))
              .Or(precise)
              .Or(minor.Or(major))
              .Or(lower.Or(lowerOrEqual))
              .Or(greater.Or(greaterOrEqual))
                .DelimitedBy(Parse.WhiteSpace)
                .Select(x => x.ToArray());

        public static NpmDependecyConstraint[] FromString(string str, string name)
        {
            if(string.IsNullOrWhiteSpace(str))
            {
                return new[]
                {
                    new NpmDependecyConstraint
                    {
                        Name = name,
                        Type = NpmDependecyType.Any,
                        Version = new NpmVersionNumber(0)
                    }
                };
            }

            if(Uri.IsWellFormedUriString(str, UriKind.Absolute))
            {
                return new[]
                {
                    new NpmDependecyConstraint
                    {
                        Name = str,
                        Type = NpmDependecyType.WebReference,
                        Version = new NpmVersionNumber(0)
                    }
                };
            }

            var array = All.Parse(str);

            for (int i = 0; i < array.Length; i++)
            {
                array[i].Name = name;
            }

            return array;
        }
    }
}
