using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Maddalena.ML.Adapters.AdventureWorks
{
    [DataContract]
    [Serializable]
    [StructLayout(LayoutKind.Auto, CharSet = CharSet.Auto)]
    public class SalesTerritory
    {
        private decimal _costlastyear;

        private decimal _costytd;

        private string _countryregioncode;

        private string _group;

        private DateTime _modifieddate;

        private string _name;

        private Guid _rowguid;

        private decimal _saleslastyear;

        private decimal _salesytd;
        private int _territoryid;

        [DataMember]
        public int TerritoryID
        {
            get => _territoryid;
            set => _territoryid = value;
        }

        [DataMember]
        public string Name
        {
            get => _name;
            set => _name = value;
        }

        [DataMember]
        public string CountryRegionCode
        {
            get => _countryregioncode;
            set => _countryregioncode = value;
        }

        [DataMember]
        public string Group
        {
            get => _group;
            set => _group = value;
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
        public decimal CostYTD
        {
            get => _costytd;
            set => _costytd = value;
        }

        [DataMember]
        public decimal CostLastYear
        {
            get => _costlastyear;
            set => _costlastyear = value;
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