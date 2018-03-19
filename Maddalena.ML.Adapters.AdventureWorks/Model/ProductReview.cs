using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Maddalena.ML.Adapters.AdventureWorks
{
    [DataContract]
    [Serializable]
    [StructLayout(LayoutKind.Auto, CharSet = CharSet.Auto)]
    public class ProductReview
    {
        private string _comments;

        private string _emailaddress;

        private DateTime _modifieddate;

        private int _productid;
        private int _productreviewid;

        private int _rating;

        private DateTime _reviewdate;

        private string _reviewername;

        [DataMember]
        public int ProductReviewID
        {
            get => _productreviewid;
            set => _productreviewid = value;
        }

        [DataMember]
        public int ProductID
        {
            get => _productid;
            set => _productid = value;
        }

        [DataMember]
        public string ReviewerName
        {
            get => _reviewername;
            set => _reviewername = value;
        }

        [DataMember]
        public DateTime ReviewDate
        {
            get => _reviewdate;
            set => _reviewdate = value;
        }

        [DataMember]
        public string EmailAddress
        {
            get => _emailaddress;
            set => _emailaddress = value;
        }

        [DataMember]
        public int Rating
        {
            get => _rating;
            set => _rating = value;
        }

        [DataMember]
        public string Comments
        {
            get => _comments;
            set => _comments = value;
        }

        [DataMember]
        public DateTime ModifiedDate
        {
            get => _modifieddate;
            set => _modifieddate = value;
        }
    }
}