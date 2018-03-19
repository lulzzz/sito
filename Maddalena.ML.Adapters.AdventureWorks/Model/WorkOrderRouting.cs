using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Maddalena.ML.Adapters.AdventureWorks
{
    [DataContract]
    [Serializable]
    [StructLayout(LayoutKind.Auto, CharSet = CharSet.Auto)]
    public class WorkOrderRouting
    {
        private decimal _actualcost;

        private DateTime _actualenddate;

        private decimal _actualresourcehrs;

        private DateTime _actualstartdate;

        private short _locationid;

        private DateTime _modifieddate;

        private short _operationsequence;

        private decimal _plannedcost;

        private int _productid;

        private DateTime _scheduledenddate;

        private DateTime _scheduledstartdate;
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
        public short OperationSequence
        {
            get => _operationsequence;
            set => _operationsequence = value;
        }

        [DataMember]
        public short LocationID
        {
            get => _locationid;
            set => _locationid = value;
        }

        [DataMember]
        public DateTime ScheduledStartDate
        {
            get => _scheduledstartdate;
            set => _scheduledstartdate = value;
        }

        [DataMember]
        public DateTime ScheduledEndDate
        {
            get => _scheduledenddate;
            set => _scheduledenddate = value;
        }

        [DataMember]
        public DateTime ActualStartDate
        {
            get => _actualstartdate;
            set => _actualstartdate = value;
        }

        [DataMember]
        public DateTime ActualEndDate
        {
            get => _actualenddate;
            set => _actualenddate = value;
        }

        [DataMember]
        public decimal ActualResourceHrs
        {
            get => _actualresourcehrs;
            set => _actualresourcehrs = value;
        }

        [DataMember]
        public decimal PlannedCost
        {
            get => _plannedcost;
            set => _plannedcost = value;
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