using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Maddalena.ML.Adapters.AdventureWorks
{
    [DataContract]
    [Serializable]
    [StructLayout(LayoutKind.Auto, CharSet = CharSet.Auto)]
    public class Employee
    {
        private DateTime _birthdate;
        private int _businessentityid;

        private bool _currentflag;

        private string _gender;

        private DateTime _hiredate;

        private string _jobtitle;

        private string _loginid;

        private string _maritalstatus;

        private DateTime _modifieddate;

        private string _nationalidnumber;

        private short _organizationlevel;

        private Guid _rowguid;

        private bool _salariedflag;

        private short _sickleavehours;

        private short _vacationhours;

        [DataMember]
        public int BusinessEntityID
        {
            get => _businessentityid;
            set => _businessentityid = value;
        }

        [DataMember]
        public string NationalIDNumber
        {
            get => _nationalidnumber;
            set => _nationalidnumber = value;
        }

        [DataMember]
        public string LoginID
        {
            get => _loginid;
            set => _loginid = value;
        }

        [DataMember]
        public short OrganizationLevel
        {
            get => _organizationlevel;
            set => _organizationlevel = value;
        }

        [DataMember]
        public string JobTitle
        {
            get => _jobtitle;
            set => _jobtitle = value;
        }

        [DataMember]
        public DateTime BirthDate
        {
            get => _birthdate;
            set => _birthdate = value;
        }

        [DataMember]
        public string MaritalStatus
        {
            get => _maritalstatus;
            set => _maritalstatus = value;
        }

        [DataMember]
        public string Gender
        {
            get => _gender;
            set => _gender = value;
        }

        [DataMember]
        public DateTime HireDate
        {
            get => _hiredate;
            set => _hiredate = value;
        }

        [DataMember]
        public bool SalariedFlag
        {
            get => _salariedflag;
            set => _salariedflag = value;
        }

        [DataMember]
        public short VacationHours
        {
            get => _vacationhours;
            set => _vacationhours = value;
        }

        [DataMember]
        public short SickLeaveHours
        {
            get => _sickleavehours;
            set => _sickleavehours = value;
        }

        [DataMember]
        public bool CurrentFlag
        {
            get => _currentflag;
            set => _currentflag = value;
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