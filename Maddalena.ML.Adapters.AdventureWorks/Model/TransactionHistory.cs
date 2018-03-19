using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Maddalena.ML.Adapters.AdventureWorks
{
    [DataContract]
    [Serializable]
    [StructLayout(LayoutKind.Auto, CharSet = CharSet.Auto)]
    public class TransactionHistory
    {
        private decimal _actualcost;

        private DateTime _modifieddate;

        private int _productid;

        private int _quantity;

        private int _referenceorderid;

        private int _referenceorderlineid;

        private DateTime _transactiondate;
        private int _transactionid;

        private string _transactiontype;

        [DataMember]
        public int TransactionID
        {
            get => _transactionid;
            set => _transactionid = value;
        }

        [DataMember]
        public int ProductID
        {
            get => _productid;
            set => _productid = value;
        }

        [DataMember]
        public int ReferenceOrderID
        {
            get => _referenceorderid;
            set => _referenceorderid = value;
        }

        [DataMember]
        public int ReferenceOrderLineID
        {
            get => _referenceorderlineid;
            set => _referenceorderlineid = value;
        }

        [DataMember]
        public DateTime TransactionDate
        {
            get => _transactiondate;
            set => _transactiondate = value;
        }

        [DataMember]
        public string TransactionType
        {
            get => _transactiontype;
            set => _transactiontype = value;
        }

        [DataMember]
        public int Quantity
        {
            get => _quantity;
            set => _quantity = value;
        }

        [DataMember]
        public decimal ActualCost
        {
            get => _actualcost;
            set => _actualcost = value;
        }

        [DataMember]
        public DateTime ModifiedDate
        {
            get => _modifieddate;
            set => _modifieddate = value;
        }
    }
}