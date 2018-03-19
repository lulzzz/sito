using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Maddalena.ML.Adapters.AdventureWorks
{
    [DataContract]
    [Serializable]
    [StructLayout(LayoutKind.Auto, CharSet = CharSet.Auto)]
    public class Illustration
    {
        private string _diagram;
        private int _illustrationid;

        private DateTime _modifieddate;

        [DataMember]
        public int IllustrationID
        {
            get => _illustrationid;
            set => _illustrationid = value;
        }

        [DataMember]
        public string Diagram
        {
            get => _diagram;
            set => _diagram = value;
        }

        [DataMember]
        public DateTime ModifiedDate
        {
            get => _modifieddate;
            set => _modifieddate = value;
        }
    }
}