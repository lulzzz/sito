using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Maddalena.ML.Adapters.AdventureWorks
{
    [DataContract]
    [Serializable]
    [StructLayout(LayoutKind.Auto, CharSet = CharSet.Auto)]
    public class PurchaseOrderHeader
    {
        private int _employeeid;

        private decimal _freight;

        private DateTime _modifieddate;

        private DateTime _orderdate;
        private int _purchaseorderid;

        private byte _revisionnumber;

        private DateTime _shipdate;

        private int _shipmethodid;

        private byte _status;

        private decimal _subtotal;

        private decimal _taxamt;

        private decimal _totaldue;

        private int _vendorid;

        [DataMember]
        public int PurchaseOrderID
        {
            get => _purchaseorderid;
            set => _purchaseorderid = value;
        }

        [DataMember]
        public byte RevisionNumber
        {
            get => _revisionnumber;
            set => _revisionnumber = value;
        }

        [DataMember]
        public byte Status
        {
            get => _status;
            set => _status = value;
        }

        [DataMember]
        public int EmployeeID
        {
            get => _employeeid;
            set => _employeeid = value;
        }

        [DataMember]
        public int VendorID
        {
            get => _vendorid;
            set => _vendorid = value;
        }

        [DataMember]
        public int ShipMethodID
        {
            get => _shipmethodid;
            set => _shipmethodid = value;
        }

        [DataMember]
        public DateTime OrderDate
        {
            get => _orderdate;
            set => _orderdate = value;
        }

        [DataMember]
        public DateTime ShipDate
        {
            get => _shipdate;
            set => _shipdate = value;
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
        public DateTime ModifiedDate
        {
            get => _modifieddate;
            set => _modifieddate = value;
        }
    }
}