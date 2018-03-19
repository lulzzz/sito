using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Maddalena.ML.Adapters.AdventureWorks
{
    [DataContract]
    [Serializable]
    [StructLayout(LayoutKind.Auto, CharSet = CharSet.Auto)]
    public class ProductListPriceHistory
    {
        private DateTime _enddate;

        private decimal _listprice;

        private DateTime _modifieddate;
        private int _productid;

        private DateTime _startdate;

        [DataMember]
        public int ProductID
        {
            get => _productid;
            set => _productid = value;
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
        public decimal ListPrice
        {
            get => _listprice;
            set => _listprice = value;
        }

        [DataMember]
        public DateTime ModifiedDate
        {
            get => _modifieddate;
            set => _modifieddate = value;
        }
    }
}