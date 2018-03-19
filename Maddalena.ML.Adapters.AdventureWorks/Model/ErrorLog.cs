using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Maddalena.ML.Adapters.AdventureWorks
{
    [DataContract]
    [Serializable]
    [StructLayout(LayoutKind.Auto, CharSet = CharSet.Auto)]
    public class ErrorLog
    {
        private int _errorline;
        private int _errorlogid;

        private string _errormessage;

        private int _errornumber;

        private string _errorprocedure;

        private int _errorseverity;

        private int _errorstate;

        private DateTime _errortime;

        private string _username;

        [DataMember]
        public int ErrorLogID
        {
            get => _errorlogid;
            set => _errorlogid = value;
        }

        [DataMember]
        public DateTime ErrorTime
        {
            get => _errortime;
            set => _errortime = value;
        }

        [DataMember]
        public string UserName
        {
            get => _username;
            set => _username = value;
        }

        [DataMember]
        public int ErrorNumber
        {
            get => _errornumber;
            set => _errornumber = value;
        }

        [DataMember]
        public int ErrorSeverity
        {
            get => _errorseverity;
            set => _errorseverity = value;
        }

        [DataMember]
        public int ErrorState
        {
            get => _errorstate;
            set => _errorstate = value;
        }

        [DataMember]
        public string ErrorProcedure
        {
            get => _errorprocedure;
            set => _errorprocedure = value;
        }

        [DataMember]
        public int ErrorLine
        {
            get => _errorline;
            set => _errorline = value;
        }

        [DataMember]
        public string ErrorMessage
        {
            get => _errormessage;
            set => _errormessage = value;
        }
    }
}