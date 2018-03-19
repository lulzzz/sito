using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Maddalena.ML.Adapters.AdventureWorks
{
    [DataContract]
    [Serializable]
    [StructLayout(LayoutKind.Auto, CharSet = CharSet.Auto)]
    public class UnitMeasure
    {
        private DateTime _modifieddate;

        private string _name;
        private string _unitmeasurecode;

        [DataMember]
        public string UnitMeasureCode
        {
            get => _unitmeasurecode;
            set => _unitmeasurecode = value;
        }

        [DataMember]
        public string Name
        {
            get => _name;
            set => _name = value;
        }

        [DataMember]
        public DateTime ModifiedDate
        {
            get => _modifieddate;
            set => _modifieddate = value;
        }
    }
}