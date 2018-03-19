using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Maddalena.ML.Adapters.AdventureWorks
{
    [DataContract]
    [Serializable]
    [StructLayout(LayoutKind.Auto, CharSet = CharSet.Auto)]
    public class EmployeePayHistory
    {
        private int _businessentityid;

        private DateTime _modifieddate;

        private byte _payfrequency;

        private decimal _rate;

        private DateTime _ratechangedate;

        [DataMember]
        public int BusinessEntityID
        {
            get => _businessentityid;
            set => _businessentityid = value;
        }

        [DataMember]
        public DateTime RateChangeDate
        {
            get => _ratechangedate;
            set => _ratechangedate = value;
        }

        [DataMember]
        public decimal Rate
        {
            get => _rate;
            set => _rate = value;
        }

        [DataMember]
        public byte PayFrequency
        {
            get => _payfrequency;
            set => _payfrequency = value;
        }

        [DataMember]
        public DateTime ModifiedDate
        {
            get => _modifieddate;
            set => _modifieddate = value;
        }
    }
}