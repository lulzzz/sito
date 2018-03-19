using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Maddalena.ML.Adapters.AdventureWorks
{
    [DataContract]
    [Serializable]
    [StructLayout(LayoutKind.Auto, CharSet = CharSet.Auto)]
    public class CreditCard
    {
        private string _cardnumber;

        private string _cardtype;
        private int _creditcardid;

        private byte _expmonth;

        private short _expyear;

        private DateTime _modifieddate;

        [DataMember]
        public int CreditCardID
        {
            get => _creditcardid;
            set => _creditcardid = value;
        }

        [DataMember]
        public string CardType
        {
            get => _cardtype;
            set => _cardtype = value;
        }

        [DataMember]
        public string CardNumber
        {
            get => _cardnumber;
            set => _cardnumber = value;
        }

        [DataMember]
        public byte ExpMonth
        {
            get => _expmonth;
            set => _expmonth = value;
        }

        [DataMember]
        public short ExpYear
        {
            get => _expyear;
            set => _expyear = value;
        }

        [DataMember]
        public DateTime ModifiedDate
        {
            get => _modifieddate;
            set => _modifieddate = value;
        }
    }
}