namespace HexaCoreVillage.Utility
{
    internal class Item
    { 
        public string ItemName {  get; set; }
        public ItemType Type {  get; set; }
        public string Desc {  get; set; }
        public int Price {  get; set; }
        public int ItemOption {  get; set; }

        public Item()
        {
            ItemName = "로지텍 마우스";
            Type = ItemType.Mouse;
            Desc = "나쁘지 않은 마우스이다.";
            Price = 500;
            ItemOption = 5;
        }
    }

    public enum ItemType
    {
        Keyboard,
        Mouse,
        Monitor,
        Notebook,
        HeadSet,
        EnergyDrink
    }

    public class InventoryItem
    {
        public string ItemName { get; set; }
        public bool IsEquipment { get; set; }
        public bool IsBuying { get; set; }
        public int Quantity {  get; set; }
        public InventoryItem(string itemName, bool isEquip, bool isBuying, int quantity)
        {
            ItemName= itemName;
            IsEquipment= isEquip;
            IsBuying= isBuying;
            Quantity= quantity;
        }
    }
}
