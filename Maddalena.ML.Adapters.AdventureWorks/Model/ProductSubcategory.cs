using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Maddalena.ML.Adapters.AdventureWorks
{
    [DataContract]
    [Serializable]
    [StructLayout(LayoutKind.Auto, CharSet = CharSet.Auto)]
    public class ProductSubcategory
    {
        private DateTime _modifieddate;

        private string _name;

        private int _productcategoryid;
        private int _productsubcategoryid;

        private Guid _rowguid;

        [DataMember]
        public int ProductSubcategoryID
        {
            get => _productsubcategoryid;
            set => _productsubcategoryid = value;
        }

        [DataMember]
        public int ProductCategoryID
        {
            get => _productcategoryid;
            set => _productcategoryid = value;
        }

        [DataMember]
        public string Name
        {
            get => _name;
            set => _name = value;
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