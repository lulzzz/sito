using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Maddalena.ML.Adapters.AdventureWorks
{
    [DataContract]
    [Serializable]
    [StructLayout(LayoutKind.Auto, CharSet = CharSet.Auto)]
    public class PhoneNumberType
    {
        private DateTime _modifieddate;

        private string _name;
        private int _phonenumbertypeid;

        [DataMember]
        public int PhoneNumberTypeID
        {
            get => _phonenumbertypeid;
            set => _phonenumbertypeid = value;
        }

        [DataMember]
        public string Name
        {
            get => _name;
            set => _name = value;
        }

        [DataMember]
        public DateTime ModifiedDate
        {
            get => _modifieddate;
            set => _modifieddate = value;
        }
    }
}