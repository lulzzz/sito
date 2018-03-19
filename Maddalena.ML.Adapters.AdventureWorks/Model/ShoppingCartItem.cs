using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Maddalena.ML.Adapters.AdventureWorks
{
    [DataContract]
    [Serializable]
    [StructLayout(LayoutKind.Auto, CharSet = CharSet.Auto)]
    public class ShoppingCartItem
    {
        private DateTime _datecreated;

        private DateTime _modifieddate;

        private int _productid;

        private int _quantity;

        private string _shoppingcartid;
        private int _shoppingcartitemid;

        [DataMember]
        public int ShoppingCartItemID
        {
            get => _shoppingcartitemid;
            set => _shoppingcartitemid = value;
        }

        [DataMember]
        public string ShoppingCartID
        {
            get => _shoppingcartid;
            set => _shoppingcartid = value;
        }

        [DataMember]
        public int Quantity
        {
            get => _quantity;
            set => _quantity = value;
        }

        [DataMember]
        public int ProductID
        {
            get => _productid;
            set => _productid = value;
        }

        [DataMember]
        public DateTime DateCreated
        {
            get => _datecreated;
            set => _datecreated = value;
        }

        [DataMember]
        public DateTime ModifiedDate
        {
            get => _modifieddate;
            set => _modifieddate = value;
        }
    }
}