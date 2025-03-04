class Player
{
    // Fields
    public int health { get; set; }
    public Room CurrentRoom { get; set; }
    public Inventory backpack;

    // Constructor
    public Player()
    {
        health = 100;
        CurrentRoom = null;
        backpack = new Inventory(25); // 25kg is pretty heavy to carry around all day.
    }

    // Methods
    public int Damage(int amount)
    {
        return this.health -= amount;
    } // player loses some health... 

    public int Heal(int amount)
    {
        return this.health += amount;
    } // player's health restores... 

    public bool IsAlive()
    {
        return health > 0;
    } // checks whether the player is alive or not

    public bool TakeFromChest(string itemName)
    {
        // TODO implement:
        // Remove the Item from the Room
        // Put it in your backpack.
        // Inspect returned values.
        // If the item doesn't fit your backpack, put it back in the chest.
        // Communicate to the user what's happening.
        // Return true/false for success/failure
        return false;
    }

    public bool DropToChest(string itemName)
    {
        // TODO implement:
        // Remove Item from your inventory.
        // Add the Item to the Room
        // Inspect returned values
        // Communicate to the user what's happening
        // Return true/false for success/failure
        return false;
    }
}