using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Maddalena.Core.Npm.Model
{
    [JsonConverter(typeof(UserListConverter))]
    public class NpmUserList : List<NpmUser>
    {
    }
}
