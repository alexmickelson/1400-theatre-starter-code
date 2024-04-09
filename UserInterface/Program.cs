
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
        for (int i = 0; i < MovieTheater.ConcessionMenuList.Count; i++)
        {
          Console.WriteLine($"{i + 1,3}. {MovieTheater.ConcessionMenuList[i].itemName}");
        }
        int cChoice = getInt("What concession would you like? ", 1, MovieTheater.ConcessionMenuList.Count);
        int quantity = getInt("How many would you like? ", 0, int.MaxValue);
        Console.Write("What is your name? ");
        string name = Console.ReadLine();
        bool pCos = false;
        bool usePoints = false;
        for (int i = 0; i < MovieTheater.PreferredCustomerList.Count; i++)
        {
          if (name == MovieTheater.PreferredCustomerList[i].name) pCos = true;
          if (pCos)
          {
            Console.WriteLine($"You have {MovieTheater.PreferredCustomerList[i].concessionPoints} concession points avalible.");
            if (MovieTheater.PreferredCustomerList[i].concessionPoints > 10)
              usePoints = GetBool("Would you like to use your points to buy concessions? ");
            var tempCus = MovieTheater.PreferredCustomerList[i];
            MovieTheater.PreferredCustomerList[i] = (tempCus.preferredCustomerID, tempCus.name, tempCus.email, tempCus.ticketPoints, tempCus.concessionPoints + quantity);
            pCos = false;
          }
        }
        Console.WriteLine($"Your total is {MovieTheater.ConcessionMenuList[cChoice - 1].price * quantity,3:C2}.");
        MovieTheater.PurchaseMenuItem(name, MovieTheater.ConcessionMenuList[cChoice - 1].itemName, quantity, usePoints);
        PressKeyToContinue("\nTransaction Complete.\nPress any key to continue.");
      }
      if (choice == 3)//Daily Report
      {
        Console.Clear();
        Console.WriteLine("***Concession Daily Report***");
        List<DateOnly> avalibleDates = new();
        foreach (var s in MovieTheater.ConcessionSaleList)
        {
          DateOnly temp = DateOnly.Parse($"{s.soldDateTime.Month}/{s.soldDateTime.Day}/{s.soldDateTime.Year}");
          if (!avalibleDates.Contains(temp)) avalibleDates.Add(temp);
        }
        for (int i = 0; i < avalibleDates.Count; i++)
        {
          Console.WriteLine($"{i + 1,3}. {avalibleDates[i]}");
        }
        int index = getInt("What date would you like to see? ", 0, avalibleDates.Count);
        var dailyReportList = MovieTheater.ConcessionDailyReport(avalibleDates[index - 1]);
        Console.Clear();
        Console.WriteLine($"{"Name",-20}{"Revenue",-20}{"Sold Items",-20}{"Given Items",-20}");
        foreach (var c in dailyReportList)
        {
          Console.WriteLine($"{c.name,-20}{c.revenue,-20:C2}{c.sold,-20}{c.givenAway,-20}");
        }
        PressKeyToContinue("\nPress any key to continue.");
      }
      if (choice == 4)//return to main menu
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
      string menu1 = "1-Menu\n2-Purchase Item\n3-Daily Report\n4-Return to Main Menu\nWhat would you like to do? ";
      bool userEntryIsOK = GetUserChoiceBool(menu1, 5, 1, out int input);
      if (userEntryIsOK) return input;
    }
  }

  public static int PrintMainMenu()
  {
    while (true)
    {
      Console.Clear();
      Console.WriteLine("***Main Menu***");
      string menu1 = "1-Ticket Window\n2-Concession Stand\n3-Advertisment Controls\n4-Scheduling Controls\n5-Theaterwide Controls\n6-Save and Exit\nWhat would you like to do? ";
      bool userEntryIsOK = GetUserChoiceBool(menu1, 6, 1, out int input);
      if (userEntryIsOK) return input;
    }
  }

  public static bool GetUserChoiceBool(string prompt, int max, int min, out int input)
  {
    input = 0;
    int number;
    Console.Write(prompt);
    if (int.TryParse(Console.ReadLine(), out number))
    {
      if (number <= max && number >= min)
      {
        input = number;
        return true;
      }
    }
    return false;
  }

  public static int getInt(string prompt, int min, int max)
  {
    Console.Write(prompt);
    while (true)
    {
      int number;
      if (int.TryParse(Console.ReadLine(), out number))
      {
        if (number >= min && number <= max)
        {
          return number;
        }
      }
      Console.Write("Please enter valid number: ");
    }
  }
  public static bool GetBool(string prompt)
  {
    while (true)
    {
      Console.Write(prompt);
      string input = Console.ReadLine().ToUpper();
      if (input == "YES" || input == "Y") return true;
      else if (input == "NO" || input == "N") return false;
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