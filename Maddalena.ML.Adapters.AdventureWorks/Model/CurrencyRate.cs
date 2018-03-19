using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Maddalena.ML.Adapters.AdventureWorks
{
    [DataContract]
    [Serializable]
    [StructLayout(LayoutKind.Auto, CharSet = CharSet.Auto)]
    public class CurrencyRate
    {
        private decimal _averagerate;

        private DateTime _currencyratedate;
        private int _currencyrateid;

        private decimal _endofdayrate;

        private string _fromcurrencycode;

        private DateTime _modifieddate;

        private string _tocurrencycode;

        [DataMember]
        public int CurrencyRateID
        {
            get => _currencyrateid;
            set => _currencyrateid = value;
        }

        [DataMember]
        public DateTime CurrencyRateDate
        {
            get => _currencyratedate;
            set => _currencyratedate = value;
        }

        [DataMember]
        public string FromCurrencyCode
        {
            get => _fromcurrencycode;
            set => _fromcurrencycode = value;
        }

        [DataMember]
        public string ToCurrencyCode
        {
            get => _tocurrencycode;
            set => _tocurrencycode = value;
        }

        [DataMember]
        public decimal AverageRate
        {
            get => _averagerate;
            set => _averagerate = value;
        }

        [DataMember]
        public decimal EndOfDayRate
        {
            get => _endofdayrate;
            set => _endofdayrate = value;
        }

        [DataMember]
        public DateTime ModifiedDate
        {
            get => _modifieddate;
            set => _modifieddate = value;
        }
    }
}