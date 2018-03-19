using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Maddalena.ML.Adapters.AdventureWorks
{
    [DataContract]
    [Serializable]
    [StructLayout(LayoutKind.Auto, CharSet = CharSet.Auto)]
    public class WorkOrder
    {
        private DateTime _duedate;

        private DateTime _enddate;

        private DateTime _modifieddate;

        private int _orderqty;

        private int _productid;

        private short _scrappedqty;

        private short _scrapreasonid;

        private DateTime _startdate;

        private int _stockedqty;
        private int _workorderid;

        [DataMember]
        public int WorkOrderID
        {
            get => _workorderid;
            set => _workorderid = value;
        }

        [DataMember]
        public int ProductID
        {
            get => _productid;
            set => _productid = value;
        }

        [DataMember]
        public int OrderQty
        {
            get => _orderqty;
            set => _orderqty = value;
        }

        [DataMember]
        public int StockedQty
        {
            get => _stockedqty;
            set => _stockedqty = value;
        }

        [DataMember]
        public short ScrappedQty
        {
            get => _scrappedqty;
            set => _scrappedqty = value;
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
        public DateTime DueDate
        {
            get => _duedate;
            set => _duedate = value;
        }

        [DataMember]
        public short ScrapReasonID
        {
            get => _scrapreasonid;
            set => _scrapreasonid = value;
        }

        [DataMember]
        public DateTime ModifiedDate
        {
            get => _modifieddate;
            set => _modifieddate = value;
        }
    }
}