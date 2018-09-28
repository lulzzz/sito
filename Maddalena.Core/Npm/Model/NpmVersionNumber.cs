using Maddalena.Core.Npm.Converters;
using System;
using System.Linq;

namespace Maddalena.Core.Npm.Model
{
    public class NpmVersionNumber : IEquatable<NpmVersionNumber>
                                  , IComparable<NpmVersionNumber>
    {
        private int[] _values;

        public int[] Values => _values.ToArray();

        public NpmVersionNumber(params int[] values)
        {
            _values = values;
        }

        public NpmVersionNumber(string str)
        {
            var str2 = new string(str.Where(x => x == '.' || char.IsNumber(x)).ToArray());
            _values = str2.Split('.', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
        }

        public int CompareTo(NpmVersionNumber other)
        {
            var shorter = Math.Min(_values.Length, other._values.Length);

            for (int i = 0; i < shorter; i++)
            {
                if (_values[i] < other._values[i]) return -1;
                if (_values[i] > other._values[i]) return 1;
            }

            if (_values.Length < other._values.Length)
            {
                return other._values.Skip(_values.Length).Sum() == 0 ? 0 : -1;
            }

            if (_values.Length > other._values.Length)
            {
                return _values.Skip(other._values.Length).Sum() == 0 ? 0 : 1;
            }

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

        public static implicit operator NpmVersionNumber(string str)
        {
            return new NpmVersionNumber(str);
        }

        public static bool operator < (NpmVersionNumber f, NpmVersionNumber s)
        {
            return f.CompareTo(s) == -1;
        }

        public static bool operator > (NpmVersionNumber f, NpmVersionNumber s)
        {
            return f.CompareTo(s) == 1;
        }

        public static bool operator <= (NpmVersionNumber f, NpmVersionNumber s)
        {
            var c = f.CompareTo(s);
            return  c == -1 || c == 0;
        }

        public static bool operator >= (NpmVersionNumber f, NpmVersionNumber s)
        {
            var c = f.CompareTo(s);
            return c == 1 || c == 0;
        }


        public override string ToString()
        {
            return string.Join(".", _values);
        }

        public override int GetHashCode()
        {
            return _values.Sum();
        }
    }
}
