using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Maddalena.ML.Adapters.AdventureWorks
{
    [DataContract]
    [Serializable]
    [StructLayout(LayoutKind.Auto, CharSet = CharSet.Auto)]
    public class ProductPhoto
    {
        private byte[] _largephoto;

        private string _largephotofilename;

        private DateTime _modifieddate;
        private int _productphotoid;

        private byte[] _thumbnailphoto;

        private string _thumbnailphotofilename;

        [DataMember]
        public int ProductPhotoID
        {
            get => _productphotoid;
            set => _productphotoid = value;
        }

        [DataMember]
        public byte[] ThumbNailPhoto
        {
            get => _thumbnailphoto;
            set => _thumbnailphoto = value;
        }

        [DataMember]
        public string ThumbnailPhotoFileName
        {
            get => _thumbnailphotofilename;
            set => _thumbnailphotofilename = value;
        }

        [DataMember]
        public byte[] LargePhoto
        {
            get => _largephoto;
            set => _largephoto = value;
        }

        [DataMember]
        public string LargePhotoFileName
        {
            get => _largephotofilename;
            set => _largephotofilename = value;
        }

        [DataMember]
        public DateTime ModifiedDate
        {
            get => _modifieddate;
            set => _modifieddate = value;
        }
    }
}