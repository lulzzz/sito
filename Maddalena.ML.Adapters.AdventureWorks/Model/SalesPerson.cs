using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Maddalena.ML.Adapters.AdventureWorks
{
    [DataContract]
    [Serializable]
    [StructLayout(LayoutKind.Auto, CharSet = CharSet.Auto)]
    public class SalesPerson
    {
        private decimal _bonus;
        private int _businessentityid;

        private decimal _commissionpct;

        private DateTime _modifieddate;

        private Guid _rowguid;

        private decimal _saleslastyear;

        private decimal _salesquota;

        private decimal _salesytd;

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
        public decimal SalesQuota
        {
            get => _salesquota;
            set => _salesquota = value;
        }

        [DataMember]
        public decimal Bonus
        {
            get => _bonus;
            set => _bonus = value;
        }

        [DataMember]
        public decimal CommissionPct
        {
            get => _commissionpct;
            set => _commissionpct = value;
        }

        [DataMember]
        public decimal SalesYTD
        {
            get => _salesytd;
            set => _salesytd = value;
        }

        [DataMember]
        public decimal SalesLastYear
        {
            get => _saleslastyear;
            set => _saleslastyear = value;
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