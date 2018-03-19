using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Maddalena.ML.Adapters.AdventureWorks
{
    [DataContract]
    [Serializable]
    [StructLayout(LayoutKind.Auto, CharSet = CharSet.Auto)]
    public class SalesReason
    {
        private DateTime _modifieddate;

        private string _name;

        private string _reasontype;
        private int _salesreasonid;

        [DataMember]
        public int SalesReasonID
        {
            get => _salesreasonid;
            set => _salesreasonid = value;
        }

        [DataMember]
        public string Name
        {
            get => _name;
            set => _name = value;
        }

        [DataMember]
        public string ReasonType
        {
            get => _reasontype;
            set => _reasontype = value;
        }

        [DataMember]
        public DateTime ModifiedDate
        {
            get => _modifieddate;
            set => _modifieddate = value;
        }
    }
}