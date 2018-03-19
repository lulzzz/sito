using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Maddalena.ML.Adapters.AdventureWorks
{
    [DataContract]
    [Serializable]
    [StructLayout(LayoutKind.Auto, CharSet = CharSet.Auto)]
    public class Document
    {
        private int _changenumber;

        private byte[] _document;
        private short _documentlevel;

        private string _documentsummary;

        private string _fileextension;

        private string _filename;

        private bool _folderflag;

        private DateTime _modifieddate;

        private int _owner;

        private string _revision;

        private Guid _rowguid;

        private byte _status;

        private string _title;

        [DataMember]
        public short DocumentLevel
        {
            get => _documentlevel;
            set => _documentlevel = value;
        }

        [DataMember]
        public string Title
        {
            get => _title;
            set => _title = value;
        }

        [DataMember]
        public int Owner
        {
            get => _owner;
            set => _owner = value;
        }

        [DataMember]
        public bool FolderFlag
        {
            get => _folderflag;
            set => _folderflag = value;
        }

        [DataMember]
        public string FileName
        {
            get => _filename;
            set => _filename = value;
        }

        [DataMember]
        public string FileExtension
        {
            get => _fileextension;
            set => _fileextension = value;
        }

        [DataMember]
        public string Revision
        {
            get => _revision;
            set => _revision = value;
        }

        [DataMember]
        public int ChangeNumber
        {
            get => _changenumber;
            set => _changenumber = value;
        }

        [DataMember]
        public byte Status
        {
            get => _status;
            set => _status = value;
        }

        [DataMember]
        public string DocumentSummary
        {
            get => _documentsummary;
            set => _documentsummary = value;
        }

        [DataMember]
        public byte[] DocumentBody
        {
            get => _document;
            set => _document = value;
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