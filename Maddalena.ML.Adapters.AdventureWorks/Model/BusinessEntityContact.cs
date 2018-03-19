using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Maddalena.ML.Adapters.AdventureWorks
{
    [DataContract]
    [Serializable]
    [StructLayout(LayoutKind.Auto, CharSet = CharSet.Auto)]
    public class BusinessEntityContact
    {
        private int _businessentityid;

        private int _contacttypeid;

        private DateTime _modifieddate;

        private int _personid;

        private Guid _rowguid;

        [DataMember]
        public int BusinessEntityID
        {
            get => _businessentityid;
            set => _businessentityid = value;
        }

        [DataMember]
        public int PersonID
        {
            get => _personid;
            set => _personid = value;
        }

        [DataMember]
        public int ContactTypeID
        {
            get => _contacttypeid;
            set => _contacttypeid = value;
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