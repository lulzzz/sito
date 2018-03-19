using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Maddalena.ML.Adapters.AdventureWorks
{
    [DataContract]
    [Serializable]
    [StructLayout(LayoutKind.Auto, CharSet = CharSet.Auto)]
    public class ProductDescription
    {
        private string _description;

        private DateTime _modifieddate;
        private int _productdescriptionid;

        private Guid _rowguid;

        [DataMember]
        public int ProductDescriptionID
        {
            get => _productdescriptionid;
            set => _productdescriptionid = value;
        }

        [DataMember]
        public string Description
        {
            get => _description;
            set => _description = value;
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