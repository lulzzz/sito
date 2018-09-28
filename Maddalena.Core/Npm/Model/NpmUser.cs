using System.Text.RegularExpressions;

namespace Maddalena.Core.Npm.Model
{
    public class NpmUser
    {
        public string Name { get; set; }
        public string Email { get; set; }

        public static implicit operator NpmUser(string str)
        {
            var user = new NpmUser();
            Regex emailRegex = new Regex(@"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", RegexOptions.IgnoreCase);

            MatchCollection emailMatches = emailRegex.Matches(str);
            foreach (Match emailMatch in emailMatches)
            {
                user.Name = str.Substring(0, emailMatch.Index);
                user.Email = emailMatch.Value;
                break;
            }
            return user;
        }
    }
}