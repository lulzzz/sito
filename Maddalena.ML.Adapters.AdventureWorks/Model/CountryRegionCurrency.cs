using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Maddalena.ML.Adapters.AdventureWorks
{
    [DataContract]
    [Serializable]
    [StructLayout(LayoutKind.Auto, CharSet = CharSet.Auto)]
    public class CountryRegionCurrency
    {
        private string _countryregioncode;

        private string _currencycode;

        private DateTime _modifieddate;

        [DataMember]
        public string CountryRegionCode
        {
            get => _countryregioncode;
            set => _countryregioncode = value;
        }

        [DataMember]
        public string CurrencyCode
        {
            get => _currencycode;
            set => _currencycode = value;
        }

        [DataMember]
        public DateTime ModifiedDate
        {
            get => _modifieddate;
            set => _modifieddate = value;
        }
    }
}