class Player
{
	//fields
	private int health;


	// auto property
	public Room CurrentRoom { get; set; }

	//constructor
	public Player()
	{
		health = 100;
		CurrentRoom = null;
	}

	// methods
	public int Damage(int amount)
	{
		return this.health -= amount;


	} // player loses some health... 
	public int Heal(int amount)
	{

		return this.health += amount;
	}

	// player's health restores... 
	public bool IsAlive()
	{
		return health > 0;
	}  // checks whether the player is alive or not

}
