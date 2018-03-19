using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Maddalena.ML.Adapters.AdventureWorks
{
    [DataContract]
    [Serializable]
    [StructLayout(LayoutKind.Auto, CharSet = CharSet.Auto)]
    public class StateProvince
    {
        private string _countryregioncode;

        private bool _isonlystateprovinceflag;

        private DateTime _modifieddate;

        private string _name;

        private Guid _rowguid;

        private string _stateprovincecode;
        private int _stateprovinceid;

        private int _territoryid;

        [DataMember]
        public int StateProvinceID
        {
            get => _stateprovinceid;
            set => _stateprovinceid = value;
        }

        [DataMember]
        public string StateProvinceCode
        {
            get => _stateprovincecode;
            set => _stateprovincecode = value;
        }

        [DataMember]
        public string CountryRegionCode
        {
            get => _countryregioncode;
            set => _countryregioncode = value;
        }

        [DataMember]
        public bool IsOnlyStateProvinceFlag
        {
            get => _isonlystateprovinceflag;
            set => _isonlystateprovinceflag = value;
        }

        [DataMember]
        public string Name
        {
            get => _name;
            set => _name = value;
        }

        [DataMember]
        public int TerritoryID
        {
            get => _territoryid;
            set => _territoryid = value;
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