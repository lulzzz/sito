using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Maddalena.ML.Adapters.AdventureWorks
{
    [DataContract]
    [Serializable]
    [StructLayout(LayoutKind.Auto, CharSet = CharSet.Auto)]
    public class SalesOrderHeaderSalesReason
    {
        private DateTime _modifieddate;
        private int _salesorderid;

        private int _salesreasonid;

        [DataMember]
        public int SalesOrderID
        {
            get => _salesorderid;
            set => _salesorderid = value;
        }

        [DataMember]
        public int SalesReasonID
        {
            get => _salesreasonid;
            set => _salesreasonid = value;
        }

        [DataMember]
        public DateTime ModifiedDate
        {
            get => _modifieddate;
            set => _modifieddate = value;
        }
    }
}