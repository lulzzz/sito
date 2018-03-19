using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Maddalena.ML.Adapters.AdventureWorks
{
    [DataContract]
    [Serializable]
    [StructLayout(LayoutKind.Auto, CharSet = CharSet.Auto)]
    public class Department
    {
        private short _departmentid;

        private string _groupname;

        private DateTime _modifieddate;

        private string _name;

        [DataMember]
        public short DepartmentID
        {
            get => _departmentid;
            set => _departmentid = value;
        }

        [DataMember]
        public string Name
        {
            get => _name;
            set => _name = value;
        }

        [DataMember]
        public string GroupName
        {
            get => _groupname;
            set => _groupname = value;
        }

        [DataMember]
        public DateTime ModifiedDate
        {
            get => _modifieddate;
            set => _modifieddate = value;
        }
    }
}