using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Maddalena.ML.Adapters.AdventureWorks
{
    [DataContract]
    [Serializable]
    [StructLayout(LayoutKind.Auto, CharSet = CharSet.Auto)]
    public class Password
    {
        private int _businessentityid;

        private DateTime _modifieddate;

        private string _passwordhash;

        private string _passwordsalt;

        private Guid _rowguid;

        [DataMember]
        public int BusinessEntityID
        {
            get => _businessentityid;
            set => _businessentityid = value;
        }

        [DataMember]
        public string PasswordHash
        {
            get => _passwordhash;
            set => _passwordhash = value;
        }

        [DataMember]
        public string PasswordSalt
        {
            get => _passwordsalt;
            set => _passwordsalt = value;
        }

        [DataMember]
        public Guid rowguid
        {
            get => _rowguid;
            set => _rowguid = value;
        }

        [DataMember]
        public DateTime ModifiedDate
        {
            get => _modifieddate;
            set => _modifieddate = value;
        }
    }
}