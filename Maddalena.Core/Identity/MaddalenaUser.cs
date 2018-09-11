using System;
using System.Collections.Generic;
using System.Text;
using Maddalena.Core.Identity.Model;

namespace Maddalena.Core.Identity
{
    public class MaddalenaUser  : MongoUser
    {
        public string DisplayName { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }
    }
}
