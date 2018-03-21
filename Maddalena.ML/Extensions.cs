using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson.IO;

namespace Maddalena.ML
{
    public static class Extensions
    {
        public static string Safe(this string str) => str ?? "";

        public static DateTime Safe(this DateTime? str) => str ?? DateTime.MinValue;

        public static bool Safe(this bool? str) => str ?? false;

        public static decimal Safe(this decimal? str) => str ?? 0;

        public static double Safe(this double? str) => str ?? 0;

        public static int Safe(this int? str) => str ?? 0;

        public static long Safe(this long? str) => str ?? 0;

        public static List<T> Safe<T>(this List<T> str) => str ?? new List<T>();

        public static List<T> Safe<K, T>(this List<K> str, Func<K, T> select) =>
            str?.Select(select)?.ToList() ?? new List<T>();

        public static DateTime SafeDate(this string str) => DateTime.TryParse(str, out DateTime dateTime) ? dateTime : DateTime.MinValue;
    }
}
