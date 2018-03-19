using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Maddalena.ML.Adapters.AdventureWorks
{
    [DataContract]
    [Serializable]
    [StructLayout(LayoutKind.Auto, CharSet = CharSet.Auto)]
    public class PersonPhone
    {
        private int _businessentityid;

        private DateTime _modifieddate;

        private string _phonenumber;

        private int _phonenumbertypeid;

        [DataMember]
        public int BusinessEntityID
        {
            get => _businessentityid;
            set => _businessentityid = value;
        }

        [DataMember]
        public string PhoneNumber
        {
            get => _phonenumber;
            set => _phonenumber = value;
        }

        [DataMember]
        public int PhoneNumberTypeID
        {
            get => _phonenumbertypeid;
            set => _phonenumbertypeid = value;
        }

        [DataMember]
        public DateTime ModifiedDate
        {
            get => _modifieddate;
            set => _modifieddate = value;
        }
    }
}