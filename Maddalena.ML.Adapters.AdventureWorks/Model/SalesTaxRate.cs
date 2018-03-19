using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Maddalena.ML.Adapters.AdventureWorks
{
    [DataContract]
    [Serializable]
    [StructLayout(LayoutKind.Auto, CharSet = CharSet.Auto)]
    public class SalesTaxRate
    {
        private DateTime _modifieddate;

        private string _name;

        private Guid _rowguid;
        private int _salestaxrateid;

        private int _stateprovinceid;

        private decimal _taxrate;

        private byte _taxtype;

        [DataMember]
        public int SalesTaxRateID
        {
            get => _salestaxrateid;
            set => _salestaxrateid = value;
        }

        [DataMember]
        public int StateProvinceID
        {
            get => _stateprovinceid;
            set => _stateprovinceid = value;
        }

        [DataMember]
        public byte TaxType
        {
            get => _taxtype;
            set => _taxtype = value;
        }

        [DataMember]
        public decimal TaxRate
        {
            get => _taxrate;
            set => _taxrate = value;
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