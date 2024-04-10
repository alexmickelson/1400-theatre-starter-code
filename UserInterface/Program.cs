
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
        int cChoice = getIntWillLoop("What concession would you like? ", 1, MovieTheater.ConcessionMenuList.Count);
        int quantity = getIntWillLoop("How many would you like? ", 0, int.MaxValue);
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
              usePoints = GetBoolWillLoop("Would you like to use your points to buy concessions? ");
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
        Console.WriteLine(MovieTheater.ConcessionReport3_AllReceipts());
        PressKeyToContinue("Hit any key to move on");
      }
      // {
      //   Console.Clear();
      //   Console.WriteLine("***Concession Daily Report***");
      //   List<DateOnly> avalibleDates = new();
      //   foreach (var s in MovieTheater.ConcessionSaleList)
      //   {
      //     DateOnly temp = DateOnly.Parse($"{s.soldDateTime.Month}/{s.soldDateTime.Day}/{s.soldDateTime.Year}");
      //     if (!avalibleDates.Contains(temp)) avalibleDates.Add(temp);
      //   }
      //   for (int i = 0; i < avalibleDates.Count; i++)
      //   {
      //     Console.WriteLine($"{i + 1,3}. {avalibleDates[i]}");
      //   }
      //   int index = getIntWillLoop("What date would you like to see? ", 0, avalibleDates.Count);
      //   var dailyReportList = MovieTheater.ConcessionReport5_ItemTotalsPerDay(avalibleDates[index - 1]);
      //   Console.Clear();
      //   Console.WriteLine($"{"Name",-20}{"Revenue",-20}{"Sold Items",-20}{"Given Items",-20}");
      //   foreach (var c in dailyReportList)
      //   {
      //     Console.WriteLine($"{c.name,-20}{c.revenue,-20:C2}{c.sold,-20}{c.givenAway,-20}");
      //   }
      //   PressKeyToContinue("\nPress any key to continue.");
      // }
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
      string menu1 =  "1-Display Items On Menu\n"+
                      "2-Purchase Item\n"+
                      "3-Display Receipts from All Sales\n"+
                      "4-Display Revenue Totals For All Days\n"+
                      "5-Display Item Revenue For A Given Day\n"+
                      "6-Return to Main Menu\n"+
                      "What would you like to do? ";
      return getIntWillLoop(menu1, 1,6);
    }
  }

  public static int PrintMainMenu()
  {
    while (true)
    {
      Console.Clear();
      Console.WriteLine("***Main Menu***");
      string menu1 = "1-Ticket Window\n2-Concession Stand\n3-Advertisment Controls\n4-Scheduling Controls\n5-Theaterwide Controls\n6-Save and Exit\nWhat would you like to do? ";
      return getIntWillLoop(menu1, 1,6);
    }
  }



    public static int getValidTheaterRoomWillLoop(string prompt)
    {
        Console.Write(prompt);
        int input;
        if (int.TryParse(Console.ReadLine(), out input))
        {
            foreach (var t in MovieTheater.TheaterRoomCapacity)
            {
                if (t.Key == input)
                {
                    return input;
                }
            }
        }
        Console.WriteLine("Invalid Entry.");
        return getValidTheaterRoomWillLoop(prompt);
    }
    public static DateOnly getDateOnlyWillLoop(string prompt)
    {
        while (true)
        {
            Console.WriteLine(prompt);
            DateOnly date;
            if (DateOnly.TryParse(Console.ReadLine(), out date))
            {
                return date;
            }
            Console.Write("Invalid. Date format must be mm/dd/yyyy: ");
        }
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
        while (true)
        {
            Console.WriteLine(prompt);
            decimal number;
            if (decimal.TryParse(Console.ReadLine(), out number))
            {
                if (number >= min && number <= max)
                {
                    return number;
                }
            }
            Console.Write("Invalid.  Please enter valid number: ");
        }
    }
    public static bool GetBoolWillLoop(string prompt)
    {
        while (true)
        {
            Console.WriteLine(prompt);
            string input = Console.ReadLine().ToUpper();
            if (input == "YES" || input == "Y" || input.ToLower()=="true" || input.ToLower()=="t") return true;
            else if (input == "NO" || input == "N" || input.ToLower()=="false" || input.ToLower()=="f") return false;
            Console.Write("Invalid.  Please enter a valid True/False/Yes/No answer");
        }
    }

    private static int getValidKeyWillLoop(string prompt, Dictionary<int, string> dictonary)
    {
        while (true)
        {
            Console.WriteLine(prompt);
            int number;
            if (int.TryParse(Console.ReadLine(), out number))
            {
                if (dictonary.ContainsKey(number))
                {
                    return number;
                }
            }
            Console.Write("Invalid.  Key not found. ");
        }
    }
    private static int getValidKeyWillLoop(string prompt, Dictionary<int, DateOnly> dictonary)
    {
        while (true)
        {
            Console.WriteLine(prompt);
            int number;
            if (int.TryParse(Console.ReadLine(), out number))
            {
                if (dictonary.ContainsKey(number))
                {
                    return number;
                }
            }
            Console.Write("Invalid.  Key not found. ");
        }
    }
    private static int getValidKeyWillLoop(string prompt, Dictionary<int, DateTime> dictonary)
    {
        while (true)
        {
            Console.WriteLine(prompt);
            int number;
            if (int.TryParse(Console.ReadLine(), out number))
            {
                if (dictonary.ContainsKey(number))
                {
                    return number;
                }
            }
            Console.Write("Invalid.  Key not found. ");
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