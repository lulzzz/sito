using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Maddalena.ML.Adapters.AdventureWorks
{
    [DataContract]
    [Serializable]
    [StructLayout(LayoutKind.Auto, CharSet = CharSet.Auto)]
    public class DatabaseLog
    {
        private int _databaselogid;

        private string _databaseuser;

        private string _event;

        private string _object;

        private DateTime _posttime;

        private string _schema;

        private string _tsql;

        private string _xmlevent;

        [DataMember]
        public int DatabaseLogID
        {
            get => _databaselogid;
            set => _databaselogid = value;
        }

        [DataMember]
        public DateTime PostTime
        {
            get => _posttime;
            set => _posttime = value;
        }

        [DataMember]
        public string DatabaseUser
        {
            get => _databaseuser;
            set => _databaseuser = value;
        }

        [DataMember]
        public string Event
        {
            get => _event;
            set => _event = value;
        }

        [DataMember]
        public string Schema
        {
            get => _schema;
            set => _schema = value;
        }

        [DataMember]
        public string Object
        {
            get => _object;
            set => _object = value;
        }

        [DataMember]
        public string TSQL
        {
            get => _tsql;
            set => _tsql = value;
        }

        [DataMember]
        public string XmlEvent
        {
            get => _xmlevent;
            set => _xmlevent = value;
        }
    }
}