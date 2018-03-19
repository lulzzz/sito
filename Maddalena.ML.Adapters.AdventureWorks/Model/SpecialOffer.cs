using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Maddalena.ML.Adapters.AdventureWorks
{
    [DataContract]
    [Serializable]
    [StructLayout(LayoutKind.Auto, CharSet = CharSet.Auto)]
    public class SpecialOffer
    {
        private string _category;

        private string _description;

        private decimal _discountpct;

        private DateTime _enddate;

        private int _maxqty;

        private int _minqty;

        private DateTime _modifieddate;

        private Guid _rowguid;
        private int _specialofferid;

        private DateTime _startdate;

        private string _type;

        [DataMember]
        public int SpecialOfferID
        {
            get => _specialofferid;
            set => _specialofferid = value;
        }

        [DataMember]
        public string Description
        {
            get => _description;
            set => _description = value;
        }

        [DataMember]
        public decimal DiscountPct
        {
            get => _discountpct;
            set => _discountpct = value;
        }

        [DataMember]
        public string Type
        {
            get => _type;
            set => _type = value;
        }

        [DataMember]
        public string Category
        {
            get => _category;
            set => _category = value;
        }

        [DataMember]
        public DateTime StartDate
        {
            get => _startdate;
            set => _startdate = value;
        }

        [DataMember]
        public DateTime EndDate
        {
            get => _enddate;
            set => _enddate = value;
        }

        [DataMember]
        public int MinQty
        {
            get => _minqty;
            set => _minqty = value;
        }

        [DataMember]
        public int MaxQty
        {
            get => _maxqty;
            set => _maxqty = value;
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