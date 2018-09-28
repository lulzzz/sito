using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sprache;

namespace Maddalena.Core.Npm.Model
{
    public class NpmDependecyConstraint
    {
        public string Name { get; internal set; }

        public NpmVersionNumber Version { get; internal set; }

        public NpmDependecyType Type { get; internal set; }

        internal NpmDependecyConstraint()
        {
        }

        internal NpmDependecyConstraint(NpmVersionNumber v, NpmDependecyType type)
        {
            Version = v;
            Type = type;
        }

        public IEnumerable<NpmPackageVersion> Apply(IEnumerable<NpmPackageVersion> versions)
        {
            switch (Type)
            {
                case NpmDependecyType.Precise:
                    return new[] { versions.First(x => x.Version.Equals(Version)) };

                case NpmDependecyType.Minor:
                    {
                        var values = Version.Values;

                        var lower = new NpmVersionNumber(values[0], values[1]);
                        var upper = new NpmVersionNumber(values[0], values[1] + 1);

                        return versions.Where(x => x.Version >= lower && x.Version < upper);
                    }
                case NpmDependecyType.Major:
                    {
                        var values = Version.Values;

                        var lower = new NpmVersionNumber(values[0]);
                        var upper = new NpmVersionNumber(values[0] + 1);

                        return versions.Where(x => x.Version >= lower && x.Version < upper);
                    }
                case NpmDependecyType.Lower:
                    return versions.Where(x => x.Version < Version);

                case NpmDependecyType.LowerOrEqual:
                    return versions.Where(x => x.Version <= Version);

                case NpmDependecyType.Greater:
                    return versions.Where(x => x.Version > Version);

                case NpmDependecyType.GreaterOrEqual:
                    return versions.Where(x => x.Version >= Version);

                case NpmDependecyType.Any:
                case NpmDependecyType.Forbidden:
                case NpmDependecyType.Latest:
                    return new[] { versions.OrderBy(x => x.Version).Last() };

                case NpmDependecyType.WebReference:

                    break;
            }
            throw new Exception("Invalid type");
        }
    }
}
