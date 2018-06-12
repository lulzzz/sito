using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ServerSideAnalytics
{
    public class MatchExpression<T>
    {
        public List<Regex> Regexes { get; set; }

        public Action<Match, T> Action { get; set; }
    }
}
