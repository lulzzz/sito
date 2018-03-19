using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Maddalena.ML.Adapters.AdventureWorks
{
    [DataContract]
    [Serializable]
    [StructLayout(LayoutKind.Auto, CharSet = CharSet.Auto)]
    public class ProductVendor
    {
        private int _averageleadtime;

        private int _businessentityid;

        private decimal _lastreceiptcost;

        private DateTime _lastreceiptdate;

        private int _maxorderqty;

        private int _minorderqty;

        private DateTime _modifieddate;

        private int _onorderqty;
        private int _productid;

        private decimal _standardprice;

        private string _unitmeasurecode;

        [DataMember]
        public int ProductID
        {
            get => _productid;
            set => _productid = value;
        }

        [DataMember]
        public int BusinessEntityID
        {
            get => _businessentityid;
            set => _businessentityid = value;
        }

        [DataMember]
        public int AverageLeadTime
        {
            get => _averageleadtime;
            set => _averageleadtime = value;
        }

        [DataMember]
        public decimal StandardPrice
        {
            get => _standardprice;
            set => _standardprice = value;
        }

        [DataMember]
        public decimal LastReceiptCost
        {
            get => _lastreceiptcost;
            set => _lastreceiptcost = value;
        }

        [DataMember]
        public DateTime LastReceiptDate
        {
            get => _lastreceiptdate;
            set => _lastreceiptdate = value;
        }

        [DataMember]
        public int MinOrderQty
        {
            get => _minorderqty;
            set => _minorderqty = value;
        }

        [DataMember]
        public int MaxOrderQty
        {
            get => _maxorderqty;
            set => _maxorderqty = value;
        }

        [DataMember]
        public int OnOrderQty
        {
            get => _onorderqty;
            set => _onorderqty = value;
        }

        [DataMember]
        public string UnitMeasureCode
        {
            get => _unitmeasurecode;
            set => _unitmeasurecode = value;
        }

        [DataMember]
        public DateTime ModifiedDate
        {
            get => _modifieddate;
            set => _modifieddate = value;
        }
    }
}