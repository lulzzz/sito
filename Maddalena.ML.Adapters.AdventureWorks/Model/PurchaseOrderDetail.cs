using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Maddalena.ML.Adapters.AdventureWorks
{
    [DataContract]
    [Serializable]
    [StructLayout(LayoutKind.Auto, CharSet = CharSet.Auto)]
    public class PurchaseOrderDetail
    {
        private DateTime _duedate;

        private decimal _linetotal;

        private DateTime _modifieddate;

        private short _orderqty;

        private int _productid;

        private int _purchaseorderdetailid;
        private int _purchaseorderid;

        private decimal _receivedqty;

        private decimal _rejectedqty;

        private decimal _stockedqty;

        private decimal _unitprice;

        [DataMember]
        public int PurchaseOrderID
        {
            get => _purchaseorderid;
            set => _purchaseorderid = value;
        }

        [DataMember]
        public int PurchaseOrderDetailID
        {
            get => _purchaseorderdetailid;
            set => _purchaseorderdetailid = value;
        }

        [DataMember]
        public DateTime DueDate
        {
            get => _duedate;
            set => _duedate = value;
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
        public decimal UnitPrice
        {
            get => _unitprice;
            set => _unitprice = value;
        }

        [DataMember]
        public decimal LineTotal
        {
            get => _linetotal;
            set => _linetotal = value;
        }

        [DataMember]
        public decimal ReceivedQty
        {
            get => _receivedqty;
            set => _receivedqty = value;
        }

        [DataMember]
        public decimal RejectedQty
        {
            get => _rejectedqty;
            set => _rejectedqty = value;
        }

        [DataMember]
        public decimal StockedQty
        {
            get => _stockedqty;
            set => _stockedqty = value;
        }

        [DataMember]
        public DateTime ModifiedDate
        {
            get => _modifieddate;
            set => _modifieddate = value;
        }
    }
}