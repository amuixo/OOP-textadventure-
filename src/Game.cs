using System;

class Game
{
	// Private fields
	private Parser parser;
	private Player player;
	private Room currentRoom;

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

		Room outside = new Room("outside the main entrance of the university");
		Room theatre = new Room("in a lecture theatre");
		Room pub = new Room("in the campus pub");
		Room lab = new Room("in a computing lab");
		Room office = new Room("in the computing admin office");
		Room basement = new Room("in the basement");
		Room attic = new Room("in the attic");


		// Initialise room exits
		outside.AddExit("east", theatre);
		outside.AddExit("south", lab);
		outside.AddExit("west", pub);
		outside.AddExit("down", basement);
		outside.AddExit("up", attic);

		theatre.AddExit("west", outside);

		pub.AddExit("east", outside);

		lab.AddExit("north", outside);
		lab.AddExit("east", office);

		office.AddExit("west", lab);
		attic.AddExit("down", outside);
		attic.AddExit("up", outside);

		// Create your Items here
		Item sword = new Item(2, "a sharp and pointy sword, nice!");
		Item potion = new Item(4, "a potion that seems to heal you, nice!");

		// And add them to the Rooms

		outside.Chest.Put("sword", sword);
		outside.Chest.Put("potion", potion);


		// Start game outside
		player.CurrentRoom = outside;


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
		}
		Console.WriteLine("Thank you for playing.");
		Console.WriteLine("Press [Enter] to continue.");
		Console.ReadLine();
	}

	// Print out the opening message for the player.
	private void PrintWelcome()
	{
		Console.WriteLine();
		Console.WriteLine("Welcome to Zuul!");
		Console.WriteLine("Zuul is a new, incredibly boring adventure game.");
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
		Console.WriteLine("You wander around at the university.");
		Console.WriteLine();
		// let the parser print the commands
		parser.PrintValidCommands();
	}


	private void PrintStatus()
	{
		Console.WriteLine("Your health is: " + player.health);
		Console.WriteLine("You are in: " + player.CurrentRoom.GetShortDescription());
		Console.WriteLine("You are carrying: " + player.backpack.ShowInventory());
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

		player.Damage(10);
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

		if (player.backpack.Put(itemName, item))
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
}


