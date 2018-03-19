using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Maddalena.ML.Adapters.AdventureWorks
{
    [DataContract]
    [Serializable]
    [StructLayout(LayoutKind.Auto, CharSet = CharSet.Auto)]
    public class JobCandidate
    {
        private int _businessentityid;
        private int _jobcandidateid;

        private DateTime _modifieddate;

        private string _resume;

        [DataMember]
        public int JobCandidateID
        {
            get => _jobcandidateid;
            set => _jobcandidateid = value;
        }

        [DataMember]
        public int BusinessEntityID
        {
            get => _businessentityid;
            set => _businessentityid = value;
        }

        [DataMember]
        public string Resume
        {
            get => _resume;
            set => _resume = value;
        }

        [DataMember]
        public DateTime ModifiedDate
        {
            get => _modifieddate;
            set => _modifieddate = value;
        }
    }
}