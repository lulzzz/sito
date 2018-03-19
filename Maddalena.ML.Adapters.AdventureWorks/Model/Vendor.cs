using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Maddalena.ML.Adapters.AdventureWorks
{
    [DataContract]
    [Serializable]
    [StructLayout(LayoutKind.Auto, CharSet = CharSet.Auto)]
    public class Vendor
    {
        private string _accountnumber;

        private bool _activeflag;
        private int _businessentityid;

        private byte _creditrating;

        private DateTime _modifieddate;

        private string _name;

        private bool _preferredvendorstatus;

        private string _purchasingwebserviceurl;

        [DataMember]
        public int BusinessEntityID
        {
            get => _businessentityid;
            set => _businessentityid = value;
        }

        [DataMember]
        public string AccountNumber
        {
            get => _accountnumber;
            set => _accountnumber = value;
        }

        [DataMember]
        public string Name
        {
            get => _name;
            set => _name = value;
        }

        [DataMember]
        public byte CreditRating
        {
            get => _creditrating;
            set => _creditrating = value;
        }

        [DataMember]
        public bool PreferredVendorStatus
        {
            get => _preferredvendorstatus;
            set => _preferredvendorstatus = value;
        }

        [DataMember]
        public bool ActiveFlag
        {
            get => _activeflag;
            set => _activeflag = value;
        }

        [DataMember]
        public string PurchasingWebServiceURL
        {
            get => _purchasingwebserviceurl;
            set => _purchasingwebserviceurl = value;
        }

        [DataMember]
        public DateTime ModifiedDate
        {
            get => _modifieddate;
            set => _modifieddate = value;
        }
    }
}