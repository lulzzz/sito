using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Maddalena.ML.Adapters.AdventureWorks
{
    [DataContract]
    [Serializable]
    [StructLayout(LayoutKind.Auto, CharSet = CharSet.Auto)]
    public class ProductModel
    {
        private string _catalogdescription;

        private string _instructions;

        private DateTime _modifieddate;

        private string _name;
        private int _productmodelid;

        private Guid _rowguid;

        [DataMember]
        public int ProductModelID
        {
            get => _productmodelid;
            set => _productmodelid = value;
        }

        [DataMember]
        public string Name
        {
            get => _name;
            set => _name = value;
        }

        [DataMember]
        public string CatalogDescription
        {
            get => _catalogdescription;
            set => _catalogdescription = value;
        }

        [DataMember]
        public string Instructions
        {
            get => _instructions;
            set => _instructions = value;
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