using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Maddalena.ML.Adapters.AdventureWorks
{
    [DataContract]
    [Serializable]
    [StructLayout(LayoutKind.Auto, CharSet = CharSet.Auto)]
    public class EmployeeDepartmentHistory
    {
        private int _businessentityid;

        private short _departmentid;

        private DateTime _enddate;

        private DateTime _modifieddate;

        private byte _shiftid;

        private DateTime _startdate;

        [DataMember]
        public int BusinessEntityID
        {
            get => _businessentityid;
            set => _businessentityid = value;
        }

        [DataMember]
        public short DepartmentID
        {
            get => _departmentid;
            set => _departmentid = value;
        }

        [DataMember]
        public byte ShiftID
        {
            get => _shiftid;
            set => _shiftid = value;
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
        public DateTime ModifiedDate
        {
            get => _modifieddate;
            set => _modifieddate = value;
        }
    }
}