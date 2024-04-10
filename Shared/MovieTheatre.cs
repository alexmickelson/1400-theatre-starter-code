// file data types

global using MovieTuple = (string title, int runLengthMinutes, string advertisingMesssage, string leads);
global using ScheduleTuple = (int showingID, System.DateTime showingDateTime, decimal ticketPrice, int theaterRoom, string movieTitle);
global using PreferredCustomerTuple = (int preferredCustomerID, string name, string email, int ticketPoints, int concessionPoints);
global using SoldTicketTuple = (System.DateTime soldDateTime, int showingID, decimal revenueCharged, int? preferredCustomerNum);
global using ConcessionMenuTuple = (string itemName, string itemDescription, decimal price);
global using ConcessionSaleTuple = (System.DateTime soldDateTime, string itemName, int quantitySold, decimal revenueCollected, int? preferredCustomerID);
global using ScheduledAdsTuple = (int scheduleShowingID, string advertisementName);
global using AdvertisementTuple = (string name, string description, int lengthInSeconds, decimal chargePerPlay);
global using DailyScheduleTuple = (string MovieTitle, System.DateTime showTime);


// report data types

global using ConcessionsReportTuple = (string name, int sold, decimal revenue, int givenAway);

namespace Shared;

public static class MovieTheater
{
    public static readonly decimal salesTaxRate = 0.06512m;
    public static List<MovieTuple> MovieList = new();// List<(string,int,string,string)>();
    public static Dictionary<int, int> TheaterRoomCapacity = new();
    public static List<ScheduleTuple> ScheduleList = new();
    public static List<SoldTicketTuple> SoldTicketList = new();
    public static List<PreferredCustomerTuple> PreferredCustomerList = new();
    public static List<ConcessionMenuTuple> ConcessionMenuList = new();
    public static List<ConcessionSaleTuple> ConcessionSaleList = new();
    public static List<AdvertisementTuple> AdvertisementList = new();
    public static List<ScheduledAdsTuple> ScheduledAdsList = new();

    public static void ReadDataInFromAllFiles()
    {
        //This needs to call DataStorage.FileAccess and have it 
        //     load the Lists/Dictionaries from the correct datafiles
        //     example: MovieList = DataStorage.FileAccess.ReadMovies();
        //     (do that for RoomCapacity, Schedule, SoldTickets, PreferredCustomers, Advertisements,Scheduled Ads, Concessions, ConcessionSales
        MovieList = DataStorage.FileAccess.ReadMovies();
        TheaterRoomCapacity = DataStorage.FileAccess.ReadTheaterRoomData();
        ScheduleList = DataStorage.FileAccess.ReadScheduleData();
        PreferredCustomerList = DataStorage.FileAccess.ReadPreferredCustomerData();
        SoldTicketList = DataStorage.FileAccess.ReadSoldTicketData();
        ConcessionMenuList = DataStorage.FileAccess.ReadConcessionMenuData();
        ConcessionSaleList = DataStorage.FileAccess.ReadConcessionSalesData();
        AdvertisementList = DataStorage.FileAccess.ReadAdvertisementData();
        ScheduledAdsList = DataStorage.FileAccess.ReadAdvertisementScheduleData();
    }
    public static void WriteDataToAllFiles()
    {
        //This needs to take each DataStructure passed in from SHARED and write it to the correct datafile.
        //     Example:  DataStorage.FileAccess.WriteMovies(MovieList);
        //     (do that for all lists/dictionaries)
        DataStorage.FileAccess.WriteMovies(MovieList);
        DataStorage.FileAccess.WriteTheaterRoomData(TheaterRoomCapacity);
        DataStorage.FileAccess.WriteScheduleData(ScheduleList);
        DataStorage.FileAccess.WritePreferredCustomerData(PreferredCustomerList);
        DataStorage.FileAccess.WriteSoldTicketData(SoldTicketList);
        DataStorage.FileAccess.WriteConcessionMenuData(ConcessionMenuList);
        DataStorage.FileAccess.WriteConcessionSalesData(ConcessionSaleList);
        DataStorage.FileAccess.WriteAdvertisementData(AdvertisementList);
        DataStorage.FileAccess.WriteAdvertisementScheduleData(ScheduledAdsList);
    }


    //TheaterWide Controls
    public static (List<(DateTime showTime, string movieTitle, int numberTicketsSold, decimal sumCollected, int CountGivenAwayFreeToPreferredCustomers)> TicketReport,
     List<(string title, int showings, decimal revenue)> AdvertisingReport, List<(string name, int sold, decimal revenue, int givenAway)> ConcessionsReport,
     decimal GrandTotal) MonthlyRevenueReport(DateOnly date)
    {
        // - T2 - (Monthly Revenue Report)
        // 	Produce a report, given a certain month, that shows the following data summations:
        // 	- Month
        // 	- - Monthly Ticket Revenue
        // 	- - Monthly Advertising Revenue
        // 	- - Monthly Concession Revenue
        // 	- Grand Total Revenue for the month
        List<(DateTime showTime, string movieTitle, int numberTicketsSold, decimal sumCollected, int CountGivenAwayFreeToPreferredCustomers)> TicketReport = MonthlyTicketSalesRevenueReport(date);
        List<(string title, int showings, decimal revenue)> AdvertisingReport = MonthlyAdvertisingRevenueReport(date);
        List<(string name, int sold, decimal revenue, int givenAway)> ConcessionsReport = MonthlyConcessionReport(date);

        decimal ticketRevenue = 0;
        foreach (var ticket in TicketReport)
        {
            ticketRevenue += ticket.sumCollected;
        }
        decimal adRevenue = 0;
        foreach (var ad in AdvertisingReport)
        {
            adRevenue += ad.revenue;
        }
        decimal concessionsRevenue = 0;
        foreach (var c in ConcessionsReport)
        {
            concessionsRevenue += c.revenue;
        }
        decimal grandTotal = ticketRevenue + adRevenue + concessionsRevenue;

        return (TicketReport, AdvertisingReport, ConcessionsReport, grandTotal);

    }



    //Scheduling
    public static void CreateShowingID(int ID, DateTime date, decimal ticketPrice, int theaterRoom, string movieTitle)
    {
        foreach (var s in ScheduleList)
        {
            if (s.showingID == ID)
            {
                throw new ArgumentException("Showing ID already in use.");
            }
        }
        ScheduleList.Add((ID, date, ticketPrice, theaterRoom, movieTitle));
    }
    public static void NewMovieRegistration(string title, int runLengthMinutes, string advertisingMesssage, string leads)
    {
        // - S1 - (New Movie)
        // 	Administrator can add a new movie into the system
        foreach (var m in MovieList)
        {
            if (m.title == title)
            {
                throw new ArgumentException("Title Already in Use");
            }
        }
        MovieList.Add((title, runLengthMinutes, advertisingMesssage, leads));
    }


    //Advertisments
    public static List<(string title, int showings, decimal revenue)> MonthlyAdvertisingRevenueReport(DateOnly date)
    {
        // - A5 - (Monthly Advertising Revenue Report)
        // 	Produce a report, given a certain month, that shows the following data summations for each commercial shown: (each commercial should have its own line, and only be listed once)
        // 	- Title & Description
        // 	- Total # of showings
        // 	- Total $ of revenue
        Dictionary<string, (string title, int showings, decimal revenue)> revenueReportDic = new();
        List<int> avalibleIDsForDate = new();
        foreach (var movie in ScheduleList)
        {
            if (DateOnly.FromDateTime(movie.showingDateTime).Month == date.Month)
                avalibleIDsForDate.Add(movie.showingID);
        }
        foreach (var s in ScheduledAdsList)
        {
            foreach (var a in AdvertisementList)
            {
                if (avalibleIDsForDate.Contains(s.scheduleShowingID) && a.name == s.advertisementName)
                {
                    if (revenueReportDic.ContainsKey(s.advertisementName))
                    {
                        revenueReportDic[s.advertisementName] = (
                                revenueReportDic[s.advertisementName].title,
                                revenueReportDic[s.advertisementName].showings + 1,
                                revenueReportDic[s.advertisementName].revenue + a.chargePerPlay);
                    }
                    else
                    {
                        revenueReportDic[s.advertisementName] = (s.advertisementName, 1, a.chargePerPlay);
                    }
                }
            }
        }
        return revenueReportDic.Values.ToList();
    }
    public static List<(string title, int showings, decimal revenue)> DailyAdvertisingRevenueReport(DateOnly date)
    {
        // - A4 - (Daily Advertising Revenue Report)
        // 	Produce a report, given a certain day, that shows the following data summations:
        // 	- AdvertisementTitle, # of Showings, Total$Revenue
        Dictionary<string, (string title, int showings, decimal revenue)> revenueReportDic = new();
        List<int> avalibleIDsForDate = new();
        foreach (var movie in ScheduleList)
        {
            if (DateOnly.FromDateTime(movie.showingDateTime) == date)
                avalibleIDsForDate.Add(movie.showingID);
        }
        foreach (var s in ScheduledAdsList)
        {
            foreach (var a in AdvertisementList)
            {
                if (avalibleIDsForDate.Contains(s.scheduleShowingID))
                {
                    if (revenueReportDic.ContainsKey(s.advertisementName))
                    {
                        revenueReportDic[s.advertisementName] = (
                                revenueReportDic[s.advertisementName].title,
                                revenueReportDic[s.advertisementName].showings + 1,
                                revenueReportDic[s.advertisementName].revenue + a.chargePerPlay);
                    }
                    else
                    {
                        revenueReportDic[s.advertisementName] = (s.advertisementName, 1, a.chargePerPlay);
                    }
                }
            }
        }
        return revenueReportDic.Values.ToList();

    }
    public static List<(int showingID, int totalAds, int seconds, decimal revenue)> AdvertismentLengthReport(DateOnly date)
    {
        // - A3 - (Daily Showing & Advertisement Length Report)
        // 	Produce a report, given a certain date, that shows the following data per showingID
        // 	- # of commercials total
        // 	- Sum of Minutes/Seconds 
        // 	- $ earned from Advertisements for that showing
        List<int> avalibleIDsForDate = new();
        foreach (var movie in ScheduleList)
        {
            if (DateOnly.FromDateTime(movie.showingDateTime) == date)
                avalibleIDsForDate.Add(movie.showingID);
        }

        Dictionary<int, (int showingID, int totalAds, int seconds, decimal revenue)> result = new();
        foreach (var ad in ScheduledAdsList)
        {
            foreach (var adItem in AdvertisementList)
            {
                if (adItem.name == ad.advertisementName)
                {
                    if (avalibleIDsForDate.Contains(ad.scheduleShowingID))
                    {
                        if (result.ContainsKey(ad.scheduleShowingID))
                        {
                            result[ad.scheduleShowingID] = (
                                result[ad.scheduleShowingID].showingID,
                                result[ad.scheduleShowingID].totalAds + 1,
                                result[ad.scheduleShowingID].seconds + adItem.lengthInSeconds,
                                result[ad.scheduleShowingID].revenue + adItem.chargePerPlay);
                        }
                        else
                        {
                            result[ad.scheduleShowingID] = (ad.scheduleShowingID, 1, adItem.lengthInSeconds, adItem.chargePerPlay);
                        }
                    }
                }
            }
        }

        return result.Values.ToList();
    }
    public static void ScheduleAdForMovie(int showingID, string name)
    {
        //	- A2 - (Schedule Advertisements for a ShowingID (schedule time & movie))
        foreach (var ad in AdvertisementList)
        {
            if (ad.name == name)
            {
                ScheduledAdsList.Add((showingID, name));
                return;
            }
        }
    }
    public static void RegisterNewAd(string name, string description, int lengthInSeconds, decimal chargePerPlay)
    {
        // - A1 - (Register NEW advertisement(s) and their rates)
        bool alreadyExists = false;
        foreach (var x in AdvertisementList)
        {
            if (x.name == name)
                alreadyExists = true;
        }
        if (alreadyExists)
            throw new ArgumentException($"Advertisment name {name} already in use.  Can't create new ad.");
        AdvertisementList.Add((name, description, lengthInSeconds, chargePerPlay));
    }


    //Concessions
    public static List<(string name, int sold, decimal revenue, int givenAway)> MonthlyConcessionReport(DateOnly date)
    {
        Dictionary<string, (string name, int sumQuantity, decimal sumOfRevenue, int countOfFree)> totals = new();
        foreach (var item in ConcessionSaleList)
        {
            if (DateOnly.FromDateTime(item.soldDateTime).Month == date.Month)
            {
                int freeitems = 0;
                if (item.revenueCollected == 0)
                    freeitems = item.quantitySold;

                if (totals.ContainsKey(item.itemName))
                {
                    totals[item.itemName] = (item.itemName,
                        totals[item.itemName].sumQuantity + item.quantitySold,
                        totals[item.itemName].sumOfRevenue + item.revenueCollected,
                        totals[item.itemName].countOfFree + freeitems);
                }
                else
                {
                    totals[item.itemName] = (item.itemName, item.quantitySold, item.revenueCollected, freeitems);
                }
            }
        }
        return totals.Values.ToList();
    }
    
    public static string ConcessionReport3_AllReceipts()
    {
      string output = $"{"Sold Date Time",20} {"Item Name",20} {"Qty",5} {"Revenue",12} {"C_ID",5}\n";
      foreach (var item in ConcessionSaleList){
        output += $"{item.soldDateTime,20} {item.itemName,20} {item.quantitySold,5} {item.revenueCollected,12} {item.preferredCustomerID,5}\n";
      }
      return output;
    }


        public static string ConcessionReport5_ItemTotalsPerDay(DateOnly date)
    {
        // - C3 - (Daily Report)
        // Creates a report sumarizing a day's concession activity, one line for each item that was sold:
        // - Item Name, Qty Sold, SumRevenueCollected, CountGivenAwayFreeToPreferredCustomers
        Dictionary<string, (string name, int sumQuantity, decimal sumOfRevenue, int countOfFree)> totals = new();
        foreach (var item in ConcessionSaleList)
        {
            if (DateOnly.FromDateTime(item.soldDateTime) == date)
            {
                int freeitems = 0;
                if (item.revenueCollected == 0)
                    freeitems = item.quantitySold;

                if (totals.ContainsKey(item.itemName))
                {
                    totals[item.itemName] = (item.itemName,
                        totals[item.itemName].sumQuantity + item.quantitySold,
                        totals[item.itemName].sumOfRevenue + item.revenueCollected,
                        totals[item.itemName].countOfFree + freeitems);
                }
                else
                {
                    totals[item.itemName] = (item.itemName, item.quantitySold, item.revenueCollected, freeitems);
                }
            }
        }
        //Ready to build output!
        string output = "";
        foreach (var item in totals)
        {
          output += $"{item.Key}";
        }
        return output;
    }
    public static void PurchaseMenuItem(string customerName, string itemWanted, int quantity, bool preferedCustomerPayWithPoints)
    {
        // - C2 - (Purchase Menu Item)
        // Customer can buy X quantity of an item and pay for it
        // Preferred Customer can buy X quantity of an item and pay for it
        // Preferred Customer can buy an item with Reward points (if balance is sufficent)
        if (preferedCustomerPayWithPoints)
        {
            foreach (var c in PreferredCustomerList)
            {
                if (c.name == customerName)
                    ConcessionSaleList.Add((DateTime.Now, itemWanted, quantity, 0, c.preferredCustomerID));
            }
        }
        else
        {
            foreach (var i in ConcessionMenuList)
            {
                if (i.itemName == itemWanted)
                    ConcessionSaleList.Add((DateTime.Now, itemWanted, quantity, i.price * quantity, null));
            }
        }
    }


    //Tickets
     public static List<(DateTime showTime, string movieTitle, int numberTicketsSold, decimal sumCollected, int CountGivenAwayFreeToPreferredCustomers)> MonthlyTicketSalesRevenueReport(DateOnly date)
    {
        List<(DateTime showTime, string movieTitle, int numberTicketsSold, decimal sumCollected, int CountGivenAwayFreeToPreferredCustomers)> salesRevenueReport = new();

        decimal totalRevenue = 0;
        foreach (var show in ScheduleList)
        {
            if (DateOnly.FromDateTime(show.showingDateTime).Month == date.Month)
            {
                (int Sold, int Given) Tickets = SumOfSoldTicketsForShowing(show.showingID);
                int sold = Tickets.Sold;
                int given = Tickets.Given;
                if (sold != 0)
                {
                    totalRevenue += GetTicketRevenueNoSalesTax(sold, show.ticketPrice);
                    salesRevenueReport.Add((show.showingDateTime, show.movieTitle, sold, totalRevenue, given));
                }
            }

        }
        return salesRevenueReport;
    }
    public static List<(DateTime showTime, string movieTitle, int numberTicketsSold, decimal sumCollected, int CountGivenAwayFreeToPreferredCustomers)> DailyTicketSalesRevenueReport(DateOnly date)
    {
        //T6 - (Daily Ticket Sales Revenue Report)
        //      Produce a report, for a given date, that shows the following data summations for each showingID:
        //  	- ShowTime,  MovieTitle, NumberTicketsSold, Sum$Collected, CountGivenAwayFreeToPreferredCustomers
        //  	(NumberTicketsSold should include the #s of free tickets.  Example 3 tickets paid for + 1 free ticket = 4 NumberTicketsSold)

        // 1) Create tuple to hold the sales report
        List<(DateTime showTime, string movieTitle, int numberTicketsSold, decimal sumCollected, int CountGivenAwayFreeToPreferredCustomers)> salesRevenueReport = new();

        foreach (var show in ScheduleList)
        {
            if (DateOnly.FromDateTime(show.showingDateTime) == date)
            {
                (int Sold, int Given) Tickets = SumOfSoldTicketsForShowing(show.showingID);
                int sold = Tickets.Sold;
                int given = Tickets.Given;
                if (sold != 0)
                {
                    decimal totalRevenue = GetTicketRevenueNoSalesTax(sold, show.ticketPrice);
                    salesRevenueReport.Add((show.showingDateTime, show.movieTitle, sold, totalRevenue, given));
                }
            }

        }
        return salesRevenueReport;
    }
    public static void PreferredCustomerRegistration(int ID, string name, string email)
    {
        //-T5 - (Preferred Customer Registration)
        //    A Customer can register as a preferred customer.
        //    PreferredCustomerTuple = (int preferredCustomerID, string name, string email, int ticketPoints, int concessionPoints);

        //1) ensure new preferred customer # doesn't already exist
        bool alreadyExists = false;
        foreach (var x in PreferredCustomerList)
        {
            if (x.preferredCustomerID == ID)
                alreadyExists = true;
        }
        if (alreadyExists)
            throw new ArgumentException($"Preferred Customer ID {ID} already in use.  Can't create new Preferred Customer.");
        PreferredCustomerList.Add((ID, name, email, 0, 0));
    }
    public static List<DailyScheduleTuple> GetDailySchedule_Basic(System.DateOnly requestedDate)
    {
        //- T3 - (Daily Movies & Showtime Report) 
        //        When Customers select a date, they can see what movies are playing at which show times for that date.

        List<DailyScheduleTuple> result = new List<DailyScheduleTuple>();
        //1) Go through list, find all showings for this date
        foreach (var x in ScheduleList)
        {
            if (DateOnly.FromDateTime(x.showingDateTime) == requestedDate)
            {
                //2) add to local list
                result.Add((x.movieTitle, x.showingDateTime));
            }
        }

        //3) return local list
        return result;
    }
    public static int HowManySeatsAreAvailableForShowingID(int showingID)
    {
        //REQ T2 - (Seat Availability)
        //Customers(providing a schedule) can check ticket availability(is a show sold out already? How many seats are left?)

        //1) get room capacity (for room assigned to this showing)
        int totalCapacity = 0;
        foreach (var x in ScheduleList)
        {
            if (x.showingID == showingID)
            {
                totalCapacity = TheaterRoomCapacity[x.theaterRoom];
                break;
            }
        }

        //2) get sum of soldTickets for showing
        (int sold, int given) tickets = SumOfSoldTicketsForShowing(showingID);

        //3) return availableSeats=Capacity-SumOfSoldTickets
        return totalCapacity - (tickets.sold + tickets.given);
    }
    public static (int Sold, int Given) SumOfSoldTicketsForShowing(int showingID)
    {
        int sumSoldTickets = 0;
        int sumGivenTickets = 0;
        foreach (var x in SoldTicketList)
        {
            if (x.showingID == showingID)
            {
                if (x.revenueCharged == 0)
                {
                    sumGivenTickets++;
                    continue;
                }
                sumSoldTickets++;
            }
        }
        return (sumSoldTickets, sumGivenTickets);
    }
    public static void TicketPurchase(int showingID, decimal revenueAmt)
    {
        //REQ T1 - Customers can buy a ticket
        SoldTicketList.Add((System.DateTime.Now, showingID, revenueAmt, null));
    }
    public static void TicketPurchase(int showingID, decimal revenueAmt, int preferredID)
    {
        //REQ T1 - Customers can buy a ticket
        //ensure preferredID exists
        bool valid = false;
        for (int i = 0; i < PreferredCustomerList.Count; i++)
        {
            if (PreferredCustomerList[i].preferredCustomerID == preferredID)
            {
                valid = true;
                //Add 1 point to ticketPoint balance
                // Can't edidt the record directly since it is in a list.
                //      PreferredCustomerList[i].ticketPoints += 1; //won't work
                // Instead use the 'with' notation
                PreferredCustomerList[i] = (PreferredCustomerList[i]) with { ticketPoints = PreferredCustomerList[i].ticketPoints + 1 };
                break;
            }
        }
        if (valid)
        {
            SoldTicketList.Add((System.DateTime.Now, showingID, revenueAmt, preferredID));
        }
        else
            throw new ArgumentException($"Invalid PreferredCustomerID. {preferredID} not found.");
    }


    public static decimal GetSalesTotal(int numberOfTickets, decimal ticketPrice)
    {
        // Requriements:
        // 1) expected to return the correct amount
        // 2) must include sales tax
        // 3) must be rounded to the nearest penny  (up , down as appropriate)

        return Math.Round((numberOfTickets * ticketPrice) * (1 + salesTaxRate), 2);
    }
    public static decimal GetAdvertisementRevenue(int watchers, decimal ratePerWatcher)
    {
        // Requriements:
        // 1) expected to return the correct amount
        // 2) must NOT include sales tax
        // 3) must be rounded to the nearest penny  (up , down as appropriate)

        return Math.Round(watchers * ratePerWatcher, 2);
    }
    public static decimal GetTicketRevenueNoSalesTax(int numberOfTickets, decimal ticketPrice)
    {
        // Requriements:
        // 1) expected to return the correct amount
        // 2) must NOT include sales tax
        // 3) must be rounded to the nearest penny  (up , down as appropriate)
        return Math.Round(numberOfTickets * ticketPrice, 2);
    }
    public static decimal GetTotalRevenuePerShowing(int numberOfTickets, decimal ticketPrice, decimal ratePerWatcher)
    {
        // Requriements:
        // 1) expected to return the correct amount
        // 2) Per Showing:
        //      Advertisement Revenue (no sales tax)   +  TicketRevenue (no sales tax)
        // 3) must be rounded to the nearest penny  (up , down as appropriate)

        //PreferredCustomer pc = new (5,"Heber","heber@there.com",0,0);
        //pc = pc with {ticketPoints=500};

        //return 0m;
        return Math.Round(GetAdvertisementRevenue(numberOfTickets, ratePerWatcher) + GetTicketRevenueNoSalesTax(numberOfTickets, ticketPrice), 2);
    }
    public static bool DoesMovieExist(string movieTitle, DateTime date, out ScheduleTuple movie)
    {
        foreach (var s in ScheduleList)
        {
            if (s.movieTitle == movieTitle && s.showingDateTime == date)
            {
                movie = s;
                return true;
            }
        }
        movie = (-1, DateTime.Now, -1, -1, null);
        return false;
    }

}

public class MovieTheaterFunctions
{
    public static PreferredCustomerTuple? GetPreferredCustomer(string userName)
    {
        foreach (var c in MovieTheater.PreferredCustomerList)
        {
            if (userName == c.name)
            {
                return c;
            }
        }
        return null;
    }

    public static ScheduleTuple? GetMovieSchedule(int movieID, DateTime time)
    {
        foreach (var o in MovieTheater.ScheduleList)
        {
            if (o.showingID == movieID && o.showingDateTime == time) return o;
        }
        return null;
    }

    public static Dictionary<int, string> avavlibleMovies()
    {
        Dictionary<int, string> totals = new();
        foreach (var item in MovieTheater.ScheduleList)
        {
            if (!totals.ContainsValue(item.movieTitle))
            {
                totals[item.showingID] = item.movieTitle;
            }

        }
        return totals;
    }
    public static Dictionary<int, DateTime> avavlibleMovieSchedules(string movieTitles)
    {
        Dictionary<int, DateTime> totals = new();
        foreach (var item in MovieTheater.ScheduleList)
        {
            if (movieTitles == item.movieTitle)
            {
                totals[item.showingID] = item.showingDateTime;
            }

        }
        return totals;
    }
    public static Dictionary<int, DateTime> avavlibleMovieSchedules(int ID, List<string> movies)
    {
        Dictionary<int, DateTime> totals = new();
        foreach (var item in MovieTheater.ScheduleList)
        {
            if (item.movieTitle == MovieTheater.ScheduleList[ID].movieTitle)
            {
                totals[item.showingID] = item.showingDateTime;
            }

        }
        return totals;
    }

    public static DateTime GetDateTimeForShowingID(int showingID)
    {
        foreach (var o in MovieTheater.ScheduleList)
        {
            if (o.showingID == showingID) return o.showingDateTime;
        }
        throw new Exception();

    }

    public static void UpdatePrefferedCustomer(int ticketNumber, PreferredCustomerTuple? prefferedCustomer, bool usePoints)
    {
        int freeTicketsGiven = ticketNumber;
        (int preferredCustomerID, string name, string email, int ticketPoints, int concessionPoints)
            newPrefferedCustomer;
        if (usePoints)
        {
            newPrefferedCustomer = (prefferedCustomer?.preferredCustomerID ?? 0,
                                                            prefferedCustomer?.name ?? "",
                                                            prefferedCustomer?.email ?? "",
                                                            prefferedCustomer?.ticketPoints - (10 * ticketNumber) + ticketNumber ?? 0,
                                                            prefferedCustomer?.concessionPoints ?? 0);
        }
        else
        {
            newPrefferedCustomer = (prefferedCustomer?.preferredCustomerID ?? 0,
                                                            prefferedCustomer?.name ?? "",
                                                            prefferedCustomer?.email ?? "",
                                                            prefferedCustomer?.ticketPoints + ticketNumber ?? 0,
                                                            prefferedCustomer?.concessionPoints ?? 0);
        }
        for (int i = 0; i < MovieTheater.PreferredCustomerList.Count; i++)
        {
            if (MovieTheater.PreferredCustomerList[i].preferredCustomerID == newPrefferedCustomer.preferredCustomerID)
                MovieTheater.PreferredCustomerList[i] = newPrefferedCustomer;
        }
    }
}