using System.Text.Json.Serialization;

namespace HexaCoreVillage.Utility;

public class Item
{
    [JsonPropertyName("ItemName")] public string? ItemName {  get; set; }
    [JsonPropertyName("Type")] public ItemType Type {  get; set; }
    [JsonPropertyName("Desc")] public string? Desc {  get; set; }
    [JsonPropertyName("Price")] public int Price {  get; set; }
    [JsonPropertyName("ItemOption")] public int ItemOption {  get; set; }
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
