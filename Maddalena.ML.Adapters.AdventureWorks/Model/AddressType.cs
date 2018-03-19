using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Maddalena.ML.Adapters.AdventureWorks
{
    [DataContract]
    [Serializable]
    [StructLayout(LayoutKind.Auto, CharSet = CharSet.Auto)]
    public class AddressType
    {
        private int _addresstypeid;

        private DateTime _modifieddate;

        private string _name;

        private Guid _rowguid;

        [DataMember]
        public int AddressTypeID
        {
            get => _addresstypeid;
            set => _addresstypeid = value;
        }

        [DataMember]
        public string Name
        {
            get => _name;
            set => _name = value;
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