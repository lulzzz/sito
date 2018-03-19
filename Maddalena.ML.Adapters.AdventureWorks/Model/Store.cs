using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Maddalena.ML.Adapters.AdventureWorks
{
    [DataContract]
    [Serializable]
    [StructLayout(LayoutKind.Auto, CharSet = CharSet.Auto)]
    public class Store
    {
        private int _businessentityid;

        private string _demographics;

        private DateTime _modifieddate;

        private string _name;

        private Guid _rowguid;

        private int _salespersonid;

        [DataMember]
        public int BusinessEntityID
        {
            get => _businessentityid;
            set => _businessentityid = value;
        }

        [DataMember]
        public string Name
        {
            get => _name;
            set => _name = value;
        }

        [DataMember]
        public int SalesPersonID
        {
            get => _salespersonid;
            set => _salespersonid = value;
        }

        [DataMember]
        public string Demographics
        {
            get => _demographics;
            set => _demographics = value;
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