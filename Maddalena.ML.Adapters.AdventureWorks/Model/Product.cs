using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Maddalena.ML.Adapters.AdventureWorks
{
    [DataContract]
    [Serializable]
    [StructLayout(LayoutKind.Auto, CharSet = CharSet.Auto)]
    public class Product
    {
        private string _class;

        private string _color;

        private int _daystomanufacture;

        private DateTime _discontinueddate;

        private bool _finishedgoodsflag;

        private decimal _listprice;

        private bool _makeflag;

        private DateTime _modifieddate;

        private string _name;
        private int _productid;

        private string _productline;

        private int _productmodelid;

        private string _productnumber;

        private int _productsubcategoryid;

        private short _reorderpoint;

        private Guid _rowguid;

        private short _safetystocklevel;

        private DateTime _sellenddate;

        private DateTime _sellstartdate;

        private string _size;

        private string _sizeunitmeasurecode;

        private decimal _standardcost;

        private string _style;

        private decimal _weight;

        private string _weightunitmeasurecode;

        [DataMember]
        public int ProductID
        {
            get => _productid;
            set => _productid = value;
        }

        [DataMember]
        public string Name
        {
            get => _name;
            set => _name = value;
        }

        [DataMember]
        public string ProductNumber
        {
            get => _productnumber;
            set => _productnumber = value;
        }

        [DataMember]
        public bool MakeFlag
        {
            get => _makeflag;
            set => _makeflag = value;
        }

        [DataMember]
        public bool FinishedGoodsFlag
        {
            get => _finishedgoodsflag;
            set => _finishedgoodsflag = value;
        }

        [DataMember]
        public string Color
        {
            get => _color;
            set => _color = value;
        }

        [DataMember]
        public short SafetyStockLevel
        {
            get => _safetystocklevel;
            set => _safetystocklevel = value;
        }

        [DataMember]
        public short ReorderPoint
        {
            get => _reorderpoint;
            set => _reorderpoint = value;
        }

        [DataMember]
        public decimal StandardCost
        {
            get => _standardcost;
            set => _standardcost = value;
        }

        [DataMember]
        public decimal ListPrice
        {
            get => _listprice;
            set => _listprice = value;
        }

        [DataMember]
        public string Size
        {
            get => _size;
            set => _size = value;
        }

        [DataMember]
        public string SizeUnitMeasureCode
        {
            get => _sizeunitmeasurecode;
            set => _sizeunitmeasurecode = value;
        }

        [DataMember]
        public string WeightUnitMeasureCode
        {
            get => _weightunitmeasurecode;
            set => _weightunitmeasurecode = value;
        }

        [DataMember]
        public decimal Weight
        {
            get => _weight;
            set => _weight = value;
        }

        [DataMember]
        public int DaysToManufacture
        {
            get => _daystomanufacture;
            set => _daystomanufacture = value;
        }

        [DataMember]
        public string ProductLine
        {
            get => _productline;
            set => _productline = value;
        }

        [DataMember]
        public string Class
        {
            get => _class;
            set => _class = value;
        }

        [DataMember]
        public string Style
        {
            get => _style;
            set => _style = value;
        }

        [DataMember]
        public int ProductSubcategoryID
        {
            get => _productsubcategoryid;
            set => _productsubcategoryid = value;
        }

        [DataMember]
        public int ProductModelID
        {
            get => _productmodelid;
            set => _productmodelid = value;
        }

        [DataMember]
        public DateTime SellStartDate
        {
            get => _sellstartdate;
            set => _sellstartdate = value;
        }

        [DataMember]
        public DateTime SellEndDate
        {
            get => _sellenddate;
            set => _sellenddate = value;
        }

        [DataMember]
        public DateTime DiscontinuedDate
        {
            get => _discontinueddate;
            set => _discontinueddate = value;
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