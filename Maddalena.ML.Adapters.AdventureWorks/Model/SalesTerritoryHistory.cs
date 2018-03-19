using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Maddalena.ML.Adapters.AdventureWorks
{
    [DataContract]
    [Serializable]
    [StructLayout(LayoutKind.Auto, CharSet = CharSet.Auto)]
    public class SalesTerritoryHistory
    {
        private int _businessentityid;

        private DateTime _enddate;

        private DateTime _modifieddate;

        private Guid _rowguid;

        private DateTime _startdate;

        private int _territoryid;

        [DataMember]
        public int BusinessEntityID
        {
            get => _businessentityid;
            set => _businessentityid = value;
        }

        [DataMember]
        public int TerritoryID
        {
            get => _territoryid;
            set => _territoryid = value;
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