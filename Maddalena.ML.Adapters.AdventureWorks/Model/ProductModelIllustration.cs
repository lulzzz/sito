using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Maddalena.ML.Adapters.AdventureWorks
{
    [DataContract]
    [Serializable]
    [StructLayout(LayoutKind.Auto, CharSet = CharSet.Auto)]
    public class ProductModelIllustration
    {
        private int _illustrationid;

        private DateTime _modifieddate;
        private int _productmodelid;

        [DataMember]
        public int ProductModelID
        {
            get => _productmodelid;
            set => _productmodelid = value;
        }

        [DataMember]
        public int IllustrationID
        {
            get => _illustrationid;
            set => _illustrationid = value;
        }

        [DataMember]
        public DateTime ModifiedDate
        {
            get => _modifieddate;
            set => _modifieddate = value;
        }
    }
}