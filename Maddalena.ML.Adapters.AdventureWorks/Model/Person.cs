using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Maddalena.ML.Adapters.AdventureWorks
{
    [DataContract]
    [Serializable]
    [StructLayout(LayoutKind.Auto, CharSet = CharSet.Auto)]
    public class Person
    {
        private string _additionalcontactinfo;
        private int _businessentityid;

        private string _demographics;

        private int _emailpromotion;

        private string _firstname;

        private string _lastname;

        private string _middlename;

        private DateTime _modifieddate;

        private bool _namestyle;

        private string _persontype;

        private Guid _rowguid;

        private string _suffix;

        private string _title;

        [DataMember]
        public int BusinessEntityID
        {
            get => _businessentityid;
            set => _businessentityid = value;
        }

        [DataMember]
        public string PersonType
        {
            get => _persontype;
            set => _persontype = value;
        }

        [DataMember]
        public bool NameStyle
        {
            get => _namestyle;
            set => _namestyle = value;
        }

        [DataMember]
        public string Title
        {
            get => _title;
            set => _title = value;
        }

        [DataMember]
        public string FirstName
        {
            get => _firstname;
            set => _firstname = value;
        }

        [DataMember]
        public string MiddleName
        {
            get => _middlename;
            set => _middlename = value;
        }

        [DataMember]
        public string LastName
        {
            get => _lastname;
            set => _lastname = value;
        }

        [DataMember]
        public string Suffix
        {
            get => _suffix;
            set => _suffix = value;
        }

        [DataMember]
        public int EmailPromotion
        {
            get => _emailpromotion;
            set => _emailpromotion = value;
        }

        [DataMember]
        public string AdditionalContactInfo
        {
            get => _additionalcontactinfo;
            set => _additionalcontactinfo = value;
        }

        [DataMember]
        public string Demographics
        {
            get => _demographics;
            set => _demographics = value;
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