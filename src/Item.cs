public class Item
{
    // fields
public int Weight { get; }
public string Name { get; }
public string Description { get; }
// constructor
public Item (int weight, string description)
{
    Weight = weight;
    Description = description;
    Name = description.Split(',')[0];
}
}
