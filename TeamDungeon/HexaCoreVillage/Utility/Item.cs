namespace HexaCoreVillage.Utility
{
    internal class Item
    {
        string ItemName {  get; set; }
        ItemType Type {  get; set; }
        string Desc {  get; set; }
        int Price {  get; set; }
        int ItemOption {  get; set; }

        public Item()
        {
            ItemName = "로지텍 마우스";
            Type = ItemType.Mouse;
            Desc = "나쁘지 않은 마우스이다.";
            Price = 500;
            ItemOption = 5;
        }
    }

    enum ItemType
    {
        Keyboard,
        Mouse,
        Monitor,
        Notebook,
        HeadSet,
        EnergyDrink
    }

    class InvenItem
    {
        string ItemName { get; set; }
        bool isEquipment { get; set; }
        bool isBuying { get; set; }
        int Quantity {  get; set; }
        public InvenItem() { }
        public InvenItem(string _itemName, bool _isEquip, bool _isBuying, int _quantity)
        {
            ItemName= _itemName;
            isEquipment= _isEquip;
            isBuying= _isBuying;
            Quantity= _quantity;
        }
    }
}
