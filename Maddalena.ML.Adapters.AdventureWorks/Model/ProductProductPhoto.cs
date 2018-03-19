using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Maddalena.ML.Adapters.AdventureWorks
{
    [DataContract]
    [Serializable]
    [StructLayout(LayoutKind.Auto, CharSet = CharSet.Auto)]
    public class ProductProductPhoto
    {
        private DateTime _modifieddate;

        private bool _primary;
        private int _productid;

        private int _productphotoid;

        [DataMember]
        public int ProductID
        {
            get => _productid;
            set => _productid = value;
        }

        [DataMember]
        public int ProductPhotoID
        {
            get => _productphotoid;
            set => _productphotoid = value;
        }

        [DataMember]
        public bool Primary
        {
            get => _primary;
            set => _primary = value;
        }

        [DataMember]
        public DateTime ModifiedDate
        {
            get => _modifieddate;
            set => _modifieddate = value;
        }
    }
}