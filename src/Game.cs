using System;

class Game
{
	// Private fields
	private Parser parser;
	private Player player;

	private Room shrineroom;
	private Room shrineroom1;

	// Constructor
	public Game()
	{
		parser = new Parser();
		player = new Player();
		CreateRooms();
	}

	// Initialise the Rooms (and the Items)
	private void CreateRooms()
	{
		// Create the rooms

		Room forest = new Room("you're alone in a forest and there seems to only be one path");
		Room shrine = new Room("You see a shrine in front of you, maybe you should look around");
		Room shrine1 = new Room("the monster passed by you, you can grab the key safetly now.");
		Room cabin = new Room("you're in a cozy cabin but you're freezing, maybe look around for some items.");
		Room hill = new Room("you went up the hill, but there's nothing here.");
		shrineroom = shrine;
		shrineroom1 = shrine1;


		// Initialise room exits
		forest.AddExit("north", shrine);
		// outside.AddExit("south", lab);
		// outside.AddExit("west", pub);
		// outside.AddExit("down", basement);
		// outside.AddExit("up", attic);

		// Create your Items here
		Item key = new Item(2, "a key!");
		Item wood = new Item(40, "a piece of wood! maybe it can be used as a weapon? or to make a fire?");
		Item lighter = new Item(5, "a lighter! it looks like you can try to start a fire with it.");
		Item cup = new Item(5, "a cup... what can you do with this?");
		// Item campfire = new Item(5, "a ");


		// And add them to the Rooms

		shrine.Chest.Put("key", key);
		shrine1.Chest.Put("key", key);
		cabin.Chest.Put("wood", wood);
		cabin.Chest.Put("lighter", lighter);
		cabin.Chest.Put("cup", cup);



		// Start game outside
		player.CurrentRoom = forest;


	}

	//  Main play routine. Loops until end of play.	
	public void Play()
	{
		PrintWelcome();

		// Enter the main command loop. Here we repeatedly read commands and
		// execute them until the player wants to quit.
		bool finished = false;
		while (!finished)
		{
			Command command = parser.GetCommand();
			finished = ProcessCommand(command);
			if (!player.IsAlive())
			{
				Console.WriteLine("You died :(, write 'quit' to exit the game");
				finished = true;
			}
		}
		Console.WriteLine("Press [Enter] to continue.");
		Console.ReadLine();
	}

	// Print out the opening message for the player.
	private void PrintWelcome()
	{
		Console.WriteLine();
		Console.WriteLine("Welcome to Zuul!");
		Console.WriteLine("You decided to go on an adventure in the forest, but there seems to be something lurking around.");
		Console.WriteLine("Maybe try to find out what it is...");
		Console.WriteLine("Type 'help' if you need help.");
		Console.WriteLine();
		Console.WriteLine(player.CurrentRoom.GetLongDescription());
	}

	// Given a command, process (that is: execute) the command.
	// If this command ends the game, it returns true.
	// Otherwise false is returned.
	private bool ProcessCommand(Command command)
	{
		bool wantToQuit = false;

		if (command.IsUnknown())
		{
			Console.WriteLine("I don't know what you mean...");
			return wantToQuit; // false
		}

		switch (command.CommandWord)
		{
			case "help":
				PrintHelp();
				break;
			case "go":
				GoRoom(command);
				break;
			case "quit":
				wantToQuit = true;
				break;
			case "look":
				PrintLook();
				break;
			case "status":
				PrintStatus();
				break;
			case "take":
				TakeItem(command);
				break;
			case "drop":
				DropItem(command);
				break;
			case "use":
				Use(command);
				break;

			case "hide":
				PrintHide(command);
				break;




		}

		return wantToQuit;
	}

	// ######################################
	// implementations of user commands:
	// ######################################

	// Print out some help information.
	// Here we print the mission and a list of the command words.
	private void PrintHelp()
	{
		Console.WriteLine("You are lost. You are alone.");
		Console.WriteLine("You wander around in a dark forest.");
		Console.WriteLine();
		// let the parser print the commands
		parser.PrintValidCommands();
	}


	private void PrintStatus()
	{
		Console.WriteLine("Your health is: " + player.health);
		Console.WriteLine("You are in: " + player.CurrentRoom.GetShortDescription());
		Console.WriteLine("You are carrying: " + player.backpack.ShowInventory() + "You have " + player.backpack.FreeWeight() + "kg free space left.");

	}
	// Try to go to one direction. If there is an exit, enter the new
	// room, otherwise print an error message.
	private void GoRoom(Command command)
	{
		if (!command.HasSecondWord())
		{
			// if there is no second word, we don't know where to go...
			Console.WriteLine("Go where?");
			return;
		}

		string direction = command.SecondWord;

		// Try to go to the next room.
		Room nextRoom = player.CurrentRoom.GetExit(direction);
		if (nextRoom == null)
		{
			Console.WriteLine("There is no door to " + direction + "!");
			return;
		}

		player.Damage(40);
		//player.Heal(25);

		player.CurrentRoom = nextRoom; // Update the player's current room
		Console.WriteLine(player.CurrentRoom.GetLongDescription());
	}
	///Look if no item "There's no items in this room.." if item string in deze kamer dan laat namen zien
private void PrintLook()
{
    Inventory chest = player.CurrentRoom.Chest;
    string items = chest.ShowInventory();

    if (items == "nothing")
    {
        Console.WriteLine("There are no items in this room...");
    }
    else if (player.CurrentRoom == shrineroom)
    {
        Console.WriteLine("You see the following items: " + items);
        Console.WriteLine("There's a monster guarding the key, what will you do?");
    }
    else if (player.CurrentRoom == shrineroom1)
    {
        Console.WriteLine("You see the following items: " + items);
        Console.WriteLine("You can grab the key safely now.");
    }
    else
    {
        Console.WriteLine("You see the following items: " + items);
    }
}

	private void TakeItem(Command command)
	{
		if (!command.HasSecondWord())
		{
			Console.WriteLine("Take what?");
			return;
		}

		string itemName = command.SecondWord;
		Item item = player.CurrentRoom.Chest.Get(itemName);

		if (item == null)
		{
			Console.WriteLine("There is no " + itemName + " in this room.");
			return;
		}

		if (player.CurrentRoom == shrineroom && itemName == "key")
		{
			Console.WriteLine("The monster attacks you as you grab the key!");
			player.Damage(player.health); // Player dies
			return;
		}

		if (player.backpack.Contains(itemName))
		{
			Console.WriteLine("You took the " + itemName);
		}
		else
		{
			Console.WriteLine("You can't carry that much.");
			player.CurrentRoom.Chest.Put(itemName, item);
		}
	}


	private void DropItem(Command command)
	{
		if (!command.HasSecondWord())

		{
			Console.WriteLine("Drop what?");
			return;
		}
		string itemName = command.SecondWord;
		Item item = player.backpack.Get(itemName);
		if (item != null)
		{
			player.CurrentRoom.Chest.Put(itemName, item);
			Console.WriteLine($"You dropped the {itemName}. ");
		}
		else
		{
			Console.WriteLine($"You don't have a {itemName} in your inventory.");
		}
	}
	private void PrintHide(Command command)
	{
		if (player.CurrentRoom == shrineroom)
		{
			Console.WriteLine("You hid from the monster and it passed by you.");
			player.CurrentRoom = shrineroom1;
		}
		else if (player.CurrentRoom != shrineroom)
		{
			Console.WriteLine("You can't hide here.");
		}

	}
	private void Use(Command command)
	{
		if (!command.HasSecondWord())
		{
			Console.WriteLine("Use what?");
			return;
		}
		string itemName = command.SecondWord;
		Item item = player.backpack.Get(itemName);
		if (item != null)
		{
			if (itemName == "potion")
			{
				player.Heal(25);
				Console.WriteLine("You used the potion and gained 25 health.");
			}

			if (itemName == "sword")
			{
				Console.WriteLine("It did nothing yet...");

			}

			else
			{
				Console.WriteLine("You can't use that item.");
			}
		}
		else
		{
			Console.WriteLine($"You don't have a {itemName} in your inventory.");
		}
	}

}



