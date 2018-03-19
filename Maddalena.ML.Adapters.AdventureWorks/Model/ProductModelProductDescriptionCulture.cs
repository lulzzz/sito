using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Maddalena.ML.Adapters.AdventureWorks
{
    [DataContract]
    [Serializable]
    [StructLayout(LayoutKind.Auto, CharSet = CharSet.Auto)]
    public class ProductModelProductDescriptionCulture
    {
        private string _cultureid;

        private DateTime _modifieddate;

        private int _productdescriptionid;
        private int _productmodelid;

        [DataMember]
        public int ProductModelID
        {
            get => _productmodelid;
            set => _productmodelid = value;
        }

        [DataMember]
        public int ProductDescriptionID
        {
            get => _productdescriptionid;
            set => _productdescriptionid = value;
        }

        [DataMember]
        public string CultureID
        {
            get => _cultureid;
            set => _cultureid = value;
        }

        [DataMember]
        public DateTime ModifiedDate
        {
            get => _modifieddate;
            set => _modifieddate = value;
        }
    }
}