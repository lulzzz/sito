using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Maddalena.ML.Adapters.AdventureWorks
{
    [DataContract]
    [Serializable]
    [StructLayout(LayoutKind.Auto, CharSet = CharSet.Auto)]
    public class SalesPersonQuotaHistory
    {
        private int _businessentityid;

        private DateTime _modifieddate;

        private DateTime _quotadate;

        private Guid _rowguid;

        private decimal _salesquota;

        [DataMember]
        public int BusinessEntityID
        {
            get => _businessentityid;
            set => _businessentityid = value;
        }

        [DataMember]
        public DateTime QuotaDate
        {
            get => _quotadate;
            set => _quotadate = value;
        }

        [DataMember]
        public decimal SalesQuota
        {
            get => _salesquota;
            set => _salesquota = value;
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