
using Shared;

internal class Program
{
  private static void Main()
  {
    Shared.MovieTheater.ReadDataInFromAllFiles();
    MainMenu();
    Shared.MovieTheater.WriteDataToAllFiles();

  }
  public static void MainMenu()
  {
    while (true)
    {
      int choice = PrintMainMenu();
      if (choice == 1)//Ticket Window
      {
        // TicketWindowMenu();
      }
      else if (choice == 2)//Concession Stand
      {
        ConcessionMenu();
      }
      else if (choice == 3)//Advertisment Controls
      {
        // AdvertismentMenu();
      }
      else if (choice == 4)//Scheduling Controls
      {
        // SchedulingControlsMenu();
      }
      else if (choice == 5)//Theaterwide Controls
      {
        // TheaterWideMenu();
      }
      else if (choice == 6)// Save and Exit
      {
        return;
      }
    }
  }
  public static void ConcessionMenu()
  {
    while (true)
    {
      int choice = PrintConcessionMenu();
      if (choice == 1)//Print Menu
      {
        Console.Clear();
        Console.WriteLine("****Concessions Menu****");
        Console.WriteLine($"{"NAME",-30} {"DESCRIPTION",-30} {"PRICE",-10:C2}");
        foreach (var mi in MovieTheater.ConcessionMenuList)
        {
          Console.WriteLine($"{mi.itemName,-30} {mi.itemDescription,-30} {mi.price,-10:C2}");
        }
        Console.WriteLine("\nPress any key to continue.");
        Console.ReadKey(true);
      }
      if (choice == 2)//Purchase Concession Item
      {
        Console.Clear();
        Console.WriteLine("***Purchase Concessions***");
        // display items and walk user through purchase
        PressKeyToContinue("\nTransaction Complete.\nPress any key to continue.");
      }
      if (choice == 3)//Receipts from All Sales
      {
        Console.Clear();
        Console.WriteLine(MovieTheater.ConcessionReport3_AllReceipts());
        PressKeyToContinue("Hit any key to move on");
      }
      if (choice == 4)//Revenue Totals For All Days
      {
        Console.Clear();
        PressKeyToContinue("Hit any key to move on");
      }
      if (choice == 5)//Display Item Revenue For A Given Day
      {
        Console.Clear();
        PressKeyToContinue("Hit any key to move on");
      }
      if (choice == 6)//return to main menu
      {
        return;
      }
    }
  }
  public static int PrintConcessionMenu()
  {
    while (true)
    {
      Console.Clear();
      Console.WriteLine("***Concession Menu***");
      string menu1 = "1-Display Items On Menu\n" +
                      "2-Purchase Item\n" +
                      "3-Display Receipts from All Sales\n" +
                      "4-Display Revenue Totals For All Days\n" +
                      "5-Display Item Revenue For A Given Day\n" +
                      "6-Return to Main Menu\n" +
                      "What would you like to do? ";
      return getIntWillLoop(menu1, 1, 6);
    }
  }

  public static int PrintMainMenu()
  {
    while (true)
    {
      Console.Clear();
      Console.WriteLine("***Main Menu***");
      string menu1 = "1-Ticket Window\n2-Concession Stand\n3-Advertisment Controls\n4-Scheduling Controls\n5-Theaterwide Controls\n6-Save and Exit\nWhat would you like to do? ";
      return getIntWillLoop(menu1, 1, 6);
    }
  }

  public static DateOnly getDateOnlyWillLoop(string prompt)
  {
    // todo
    return new DateOnly();
  }
  public static int getIntWillLoop(string prompt, int min, int max)
  {
    while (true)
    {
      Console.WriteLine(prompt);
      int number;
      if (int.TryParse(Console.ReadLine(), out number))
      {
        if (number >= min && number <= max)
        {
          return number;
        }
      }
      Console.Write("Invalid.  Please enter valid number: ");
    }
  }
  public static decimal getDecimalWillLoop(string prompt, int min, int max)
  {
    // todo
    return -1m;
  }
  public static bool GetBoolWillLoop(string prompt)
  {
    while (true)
    {
      Console.WriteLine(prompt);
      string input = Console.ReadLine().ToUpper();
      if (input == "YES" || input == "Y" || input.ToLower() == "true" || input.ToLower() == "t") return true;
      else if (input == "NO" || input == "N" || input.ToLower() == "false" || input.ToLower() == "f") return false;
      Console.Write("Invalid.  Please enter a valid True/False/Yes/No answer");
    }
  }


  public static void PressKeyToContinue(string prompt)
  {
    while (Console.KeyAvailable)
    {
      Console.ReadKey(true);
    }
    Console.WriteLine(prompt);
    Console.ReadKey(true);
  }
}