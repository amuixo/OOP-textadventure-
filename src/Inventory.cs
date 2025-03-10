public class Inventory
{
    // fields
    private int maxWeight;
    private Dictionary<string, Item> items;

    // constructor
    public Inventory(int maxWeight)
    {
        this.maxWeight = maxWeight;
        this.items = new Dictionary<string, Item>();
    }

    // methods
    public bool Put(string itemName, Item item)
    {
             if (TotalWeight() + item.Weight <= maxWeight)
        {
            items[itemName] = item;
            return true;
        }
        return false;
    }

    public Item Get(string itemName)

        {
            items.TryGetValue(itemName, out Item item);
            items.Remove(itemName);
            return item;
        }


    public int TotalWeight()
    {
        int total = 0;
        foreach (var item in items)
        {
            total += item.Value.Weight;
        }
        return total;
    }

    public int FreeWeight()
    {
        // TODO implement:
        // compare MaxWeight and TotalWeight()
        return maxWeight - TotalWeight();
    }

//method show all items in inventory
    public string ShowInventory()
    {
        string str = " ";
        foreach (var item in items)
        {
            str += item.Value.Description + " ";
        }
        return str;
    }
}