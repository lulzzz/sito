using System;
using System.Linq;

namespace Maddalena.Core.Npm.Model
{
    public class NpmVersionNumber : IEquatable<NpmVersionNumber>
                                  , IComparable<NpmVersionNumber>
    {
        private int[] _values;

        public NpmVersionNumber(int[] values)
        {
            _values = values;
        }

        public int CompareTo(NpmVersionNumber other)
        {
            var shorter = Math.Min(_values.Length, other._values.Length);

            for (int i = 0; i < shorter; i++)
            {
                if (_values[i] < other._values[i]) return -1;
                if (_values[i] > other._values[i]) return 1;
            }

            if (_values.Length < other._values.Length) return -1;
            if (_values.Length > other._values.Length) return 1;

            return 0;
        }

        public bool Equals(NpmVersionNumber other)
        {
            return CompareTo(other) == 0;
        }

        public override bool Equals(object obj)
        {
            var s = obj as NpmVersionNumber;

            if (s != null) return Equals(s);

            return false;
        }

        public override int GetHashCode()
        {
            return _values.Sum();
        }
    }
}
