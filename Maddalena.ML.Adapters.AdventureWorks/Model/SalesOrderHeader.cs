using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Maddalena.ML.Adapters.AdventureWorks
{
    [DataContract]
    [Serializable]
    [StructLayout(LayoutKind.Auto, CharSet = CharSet.Auto)]
    public class SalesOrderHeader
    {
        private string _accountnumber;

        private int _billtoaddressid;

        private string _comment;

        private string _creditcardapprovalcode;

        private int _creditcardid;

        private int _currencyrateid;

        private int _customerid;

        private DateTime _duedate;

        private decimal _freight;

        private DateTime _modifieddate;

        private bool _onlineorderflag;

        private DateTime _orderdate;

        private string _purchaseordernumber;

        private byte _revisionnumber;

        private Guid _rowguid;
        private int _salesorderid;

        private string _salesordernumber;

        private int _salespersonid;

        private DateTime _shipdate;

        private int _shipmethodid;

        private int _shiptoaddressid;

        private byte _status;

        private decimal _subtotal;

        private decimal _taxamt;

        private int _territoryid;

        private decimal _totaldue;

        [DataMember]
        public int SalesOrderID
        {
            get => _salesorderid;
            set => _salesorderid = value;
        }

        [DataMember]
        public byte RevisionNumber
        {
            get => _revisionnumber;
            set => _revisionnumber = value;
        }

        [DataMember]
        public DateTime OrderDate
        {
            get => _orderdate;
            set => _orderdate = value;
        }

        [DataMember]
        public DateTime DueDate
        {
            get => _duedate;
            set => _duedate = value;
        }

        [DataMember]
        public DateTime ShipDate
        {
            get => _shipdate;
            set => _shipdate = value;
        }

        [DataMember]
        public byte Status
        {
            get => _status;
            set => _status = value;
        }

        [DataMember]
        public bool OnlineOrderFlag
        {
            get => _onlineorderflag;
            set => _onlineorderflag = value;
        }

        [DataMember]
        public string SalesOrderNumber
        {
            get => _salesordernumber;
            set => _salesordernumber = value;
        }

        [DataMember]
        public string PurchaseOrderNumber
        {
            get => _purchaseordernumber;
            set => _purchaseordernumber = value;
        }

        [DataMember]
        public string AccountNumber
        {
            get => _accountnumber;
            set => _accountnumber = value;
        }

        [DataMember]
        public int CustomerID
        {
            get => _customerid;
            set => _customerid = value;
        }

        [DataMember]
        public int SalesPersonID
        {
            get => _salespersonid;
            set => _salespersonid = value;
        }

        [DataMember]
        public int TerritoryID
        {
            get => _territoryid;
            set => _territoryid = value;
        }

        [DataMember]
        public int BillToAddressID
        {
            get => _billtoaddressid;
            set => _billtoaddressid = value;
        }

        [DataMember]
        public int ShipToAddressID
        {
            get => _shiptoaddressid;
            set => _shiptoaddressid = value;
        }

        [DataMember]
        public int ShipMethodID
        {
            get => _shipmethodid;
            set => _shipmethodid = value;
        }

        [DataMember]
        public int CreditCardID
        {
            get => _creditcardid;
            set => _creditcardid = value;
        }

        [DataMember]
        public string CreditCardApprovalCode
        {
            get => _creditcardapprovalcode;
            set => _creditcardapprovalcode = value;
        }

        [DataMember]
        public int CurrencyRateID
        {
            get => _currencyrateid;
            set => _currencyrateid = value;
        }

        [DataMember]
        public decimal SubTotal
        {
            get => _subtotal;
            set => _subtotal = value;
        }

        [DataMember]
        public decimal TaxAmt
        {
            get => _taxamt;
            set => _taxamt = value;
        }

        [DataMember]
        public decimal Freight
        {
            get => _freight;
            set => _freight = value;
        }

        [DataMember]
        public decimal TotalDue
        {
            get => _totaldue;
            set => _totaldue = value;
        }

        [DataMember]
        public string Comment
        {
            get => _comment;
            set => _comment = value;
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