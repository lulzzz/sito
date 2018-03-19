using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Maddalena.ML.Adapters.AdventureWorks
{
    [DataContract]
    [Serializable]
    [StructLayout(LayoutKind.Auto, CharSet = CharSet.Auto)]
    public class ProductInventory
    {
        private byte _bin;

        private short _locationid;

        private DateTime _modifieddate;
        private int _productid;

        private short _quantity;

        private Guid _rowguid;

        private string _shelf;

        [DataMember]
        public int ProductID
        {
            get => _productid;
            set => _productid = value;
        }

        [DataMember]
        public short LocationID
        {
            get => _locationid;
            set => _locationid = value;
        }

        [DataMember]
        public string Shelf
        {
            get => _shelf;
            set => _shelf = value;
        }

        [DataMember]
        public byte Bin
        {
            get => _bin;
            set => _bin = value;
        }

        [DataMember]
        public short Quantity
        {
            get => _quantity;
            set => _quantity = value;
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