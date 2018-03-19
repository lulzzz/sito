using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Maddalena.ML.Adapters.AdventureWorks
{
    [DataContract]
    [Serializable]
    [StructLayout(LayoutKind.Auto, CharSet = CharSet.Auto)]
    public class Shift
    {
        private TimeSpan _endtime;

        private DateTime _modifieddate;

        private string _name;
        private byte _shiftid;

        private TimeSpan _starttime;

        [DataMember]
        public byte ShiftID
        {
            get => _shiftid;
            set => _shiftid = value;
        }

        [DataMember]
        public string Name
        {
            get => _name;
            set => _name = value;
        }

        [DataMember]
        public TimeSpan StartTime
        {
            get => _starttime;
            set => _starttime = value;
        }

        [DataMember]
        public TimeSpan EndTime
        {
            get => _endtime;
            set => _endtime = value;
        }

        [DataMember]
        public DateTime ModifiedDate
        {
            get => _modifieddate;
            set => _modifieddate = value;
        }
    }
}