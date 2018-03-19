using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Maddalena.ML.Adapters.AdventureWorks
{
    [DataContract]
    [Serializable]
    [StructLayout(LayoutKind.Auto, CharSet = CharSet.Auto)]
    public class SalesOrderDetail
    {
        private string _carriertrackingnumber;

        private decimal _linetotal;

        private DateTime _modifieddate;

        private short _orderqty;

        private int _productid;

        private Guid _rowguid;

        private int _salesorderdetailid;
        private int _salesorderid;

        private int _specialofferid;

        private decimal _unitprice;

        private decimal _unitpricediscount;

        [DataMember]
        public int SalesOrderID
        {
            get => _salesorderid;
            set => _salesorderid = value;
        }

        [DataMember]
        public int SalesOrderDetailID
        {
            get => _salesorderdetailid;
            set => _salesorderdetailid = value;
        }

        [DataMember]
        public string CarrierTrackingNumber
        {
            get => _carriertrackingnumber;
            set => _carriertrackingnumber = value;
        }

        [DataMember]
        public short OrderQty
        {
            get => _orderqty;
            set => _orderqty = value;
        }

        [DataMember]
        public int ProductID
        {
            get => _productid;
            set => _productid = value;
        }

        [DataMember]
        public int SpecialOfferID
        {
            get => _specialofferid;
            set => _specialofferid = value;
        }

        [DataMember]
        public decimal UnitPrice
        {
            get => _unitprice;
            set => _unitprice = value;
        }

        [DataMember]
        public decimal UnitPriceDiscount
        {
            get => _unitpricediscount;
            set => _unitpricediscount = value;
        }

        [DataMember]
        public decimal LineTotal
        {
            get => _linetotal;
            set => _linetotal = value;
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