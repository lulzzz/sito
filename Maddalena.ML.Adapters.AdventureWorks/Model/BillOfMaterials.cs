using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Maddalena.ML.Adapters.AdventureWorks
{
    [DataContract]
    [Serializable]
    [StructLayout(LayoutKind.Auto, CharSet = CharSet.Auto)]
    public class BillOfMaterials
    {
        private int _billofmaterialsid;

        private short _bomlevel;

        private int _componentid;

        private DateTime _enddate;

        private DateTime _modifieddate;

        private decimal _perassemblyqty;

        private int _productassemblyid;

        private DateTime _startdate;

        private string _unitmeasurecode;

        [DataMember]
        public int BillOfMaterialsID
        {
            get => _billofmaterialsid;
            set => _billofmaterialsid = value;
        }

        [DataMember]
        public int ProductAssemblyID
        {
            get => _productassemblyid;
            set => _productassemblyid = value;
        }

        [DataMember]
        public int ComponentID
        {
            get => _componentid;
            set => _componentid = value;
        }

        [DataMember]
        public DateTime StartDate
        {
            get => _startdate;
            set => _startdate = value;
        }

        [DataMember]
        public DateTime EndDate
        {
            get => _enddate;
            set => _enddate = value;
        }

        [DataMember]
        public string UnitMeasureCode
        {
            get => _unitmeasurecode;
            set => _unitmeasurecode = value;
        }

        [DataMember]
        public short BOMLevel
        {
            get => _bomlevel;
            set => _bomlevel = value;
        }

        [DataMember]
        public decimal PerAssemblyQty
        {
            get => _perassemblyqty;
            set => _perassemblyqty = value;
        }

        [DataMember]
        public DateTime ModifiedDate
        {
            get => _modifieddate;
            set => _modifieddate = value;
        }
    }
}