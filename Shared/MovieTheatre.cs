// file data types

global using ConcessionMenuTuple = (string itemName, string itemDescription, decimal price);
global using ConcessionSaleTuple = (System.DateTime soldDateTime, string itemName, int quantitySold, decimal revenueCollected, int? preferredCustomerID);
global using MovieTuple = (string title, int runLengthMinutes, string advertisingMesssage, string leads);
global using ScheduleTuple = (int showingID, System.DateTime showingDateTime, decimal ticketPrice, int theaterRoom, string movieTitle);
global using PreferredCustomerTuple = (int preferredCustomerID, string name, string email, int ticketPoints, int concessionPoints);
global using SoldTicketTuple = (System.DateTime soldDateTime, int showingID, decimal revenueCharged, int? preferredCustomerNum);
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
    public static string MonthlyRevenueReport(DateOnly date)
    {
        // - T2 - (Monthly Revenue Report)
        // 	Produce a report, given a certain month, that shows the following data summations:
        // 	- Month
        // 	- - Monthly Ticket Revenue
        // 	- - Monthly Advertising Revenue
        // 	- - Monthly Concession Revenue
        // 	- Grand Total Revenue for the month
        // List<(DateTime showTime, string movieTitle, int numberTicketsSold, decimal sumCollected, int CountGivenAwayFreeToPreferredCustomers)> TicketReport = MonthlyTicketSalesRevenueReport(date);
        // List<(string title, int showings, decimal revenue)> AdvertisingReport = MonthlyAdvertisingRevenueReport(date);
        // List<(string name, int sold, decimal revenue, int givenAway)> ConcessionsReport = MonthlyConcessionReport(date);

        // decimal ticketRevenue = 0;
        // foreach (var ticket in TicketReport)
        // {
        //     ticketRevenue += ticket.sumCollected;
        // }
        // decimal adRevenue = 0;
        // foreach (var ad in AdvertisingReport)
        // {
        //     adRevenue += ad.revenue;
        // }
        // decimal concessionsRevenue = 0;
        // foreach (var c in ConcessionsReport)
        // {
        //     concessionsRevenue += c.revenue;
        // }
        // decimal grandTotal = ticketRevenue + adRevenue + concessionsRevenue;

        // return (TicketReport, AdvertisingReport, ConcessionsReport, grandTotal);
        return "";

    }



    //Scheduling
    public static void CreateShowing(int ID, DateTime date, decimal ticketPrice, int theaterRoom, string movieTitle)
    {

    }
    public static void NewMovieRegistration(string title, int runLengthMinutes, string advertisingMesssage, string leads)
    {
    }


    //Advertisments
    public static string MonthlyAdvertisingRevenueReport(DateOnly date)
    {
        return "";
    }
    public static string DailyAdvertisingRevenueReport(DateOnly date)
    {
        return "";
    }
    public static string AdvertismentLengthReport(DateOnly date)
    {
        return "";
    }
    public static void ScheduleAdForMovie(int showingID, string name)
    {
        
    }
    public static void RegisterNewAd(string name, string description, int lengthInSeconds, decimal chargePerPlay)
    {
        
    }

    public static string ConcessionReport3_AllReceipts()
    {
        return "";
    }
    public static string ConcessionReport4_RevenueTotalsForAllDays()
    {
        // report displays one row for each day
        // display the amount of money made in that day
        // return a nicely formatted, multi-line string
        return "";
    }


    public static string ConcessionReport5_ItemTotalsPerDay(DateOnly date)
    {
        // report is for a specific day
        // report how much money was sold for each type of items sold 
        // two purchases of "small drink" whould be combined into a single "small drink" row
        // return a nicely formatted, multi-line string
        return "";
    }
    public static void PurchaseMenuItem(string customerName, string itemWanted, int quantity, bool preferedCustomerPayWithPoints)
    {
        // create a ConcessionSaleTuple for the purchase, store it in the ConcessionSaleList
        // do not allow a concession to be sold if it is not in the ConcessionMenuList
    }


    //Tickets
    public static string MonthlyTicketSalesRevenueReport(DateOnly date)
    {
        return "";
    }
    public static string DailyTicketSalesRevenueReport(DateOnly date)
    {
        //T6 - (Daily Ticket Sales Revenue Report)
        //      Produce a report, for a given date, that shows the following data summations for each showingID:
        //  	- ShowTime,  MovieTitle, NumberTicketsSold, Sum$Collected, CountGivenAwayFreeToPreferredCustomers
        //  	(NumberTicketsSold should include the #s of free tickets.  Example 3 tickets paid for + 1 free ticket = 4 NumberTicketsSold)

        // 1) Create tuple to hold the sales report
        return "";
    }
    public static void PreferredCustomerRegistration(int ID, string name, string email)
    {
        //-T5 - (Preferred Customer Registration)
        //    A Customer can register as a preferred customer.
        //    PreferredCustomerTuple = (int preferredCustomerID, string name, string email, int ticketPoints, int concessionPoints);

        //1) ensure new preferred customer # doesn't already exist
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
       
        //2) get sum of soldTickets for showing

        //3) return availableSeats=Capacity-SumOfSoldTickets
        return -1;
    }
    public static int SumOfSoldTicketsForShowing(int showingID)
    {
        return -1;
    }
    public static int CountOfSoldTicketsForShowing(int showingID)
    {
        return -1;
    }
    public static void TicketPurchase(int showingID, decimal revenueAmt)
    {
        //REQ T1 - Customers can buy a ticket
    }
    public static void TicketPurchase(int showingID, decimal revenueAmt, int preferredID)
    {
        //REQ T1 - Customers can buy a ticket
        //ensure preferredID exists
    }

    public static decimal GetSalesTotal(int numberOfTickets, decimal ticketPrice)
    {
        // Requriements:
        // 1) expected to return the correct amount
        // 2) must include sales tax
        // 3) must be rounded to the nearest penny  (up , down as appropriate)

        return -1;
    }
    public static decimal GetAdvertisementRevenue(int watchers, decimal ratePerWatcher)
    {
        // Requriements:
        // 1) expected to return the correct amount
        // 2) must NOT include sales tax
        // 3) must be rounded to the nearest penny  (up , down as appropriate)

        return -1;
    }
    public static decimal GetTicketRevenueNoSalesTax(int numberOfTickets, decimal ticketPrice)
    {
        // Requriements:
        // 1) expected to return the correct amount
        // 2) must NOT include sales tax
        // 3) must be rounded to the nearest penny  (up , down as appropriate)
        return -1;
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
        return -1;
    }
    public static bool DoesMovieExist(string movieTitle, DateTime date, out ScheduleTuple movie)
    {
        // if the movie does exist, add it
        movie = (-1, DateTime.Now, -1, -1, null);
        return false;
    }

    // public static PreferredCustomerTuple? GetPreferredCustomer(string userName)
    // {
    //     foreach (var c in MovieTheater.PreferredCustomerList)
    //     {
    //         if (userName == c.name)
    //         {
    //             return c;
    //         }
    //     }
    //     return null;
    // }

    // public static ScheduleTuple? GetMovieSchedule(int movieID, DateTime time)
    // {
    //     foreach (var o in MovieTheater.ScheduleList)
    //     {
    //         if (o.showingID == movieID && o.showingDateTime == time) return o;
    //     }
    //     return null;
    // }

    // public static Dictionary<int, string> avavlibleMovies()
    // {
    //     Dictionary<int, string> totals = new();
    //     foreach (var item in MovieTheater.ScheduleList)
    //     {
    //         if (!totals.ContainsValue(item.movieTitle))
    //         {
    //             totals[item.showingID] = item.movieTitle;
    //         }

    //     }
    //     return totals;
    // }
    // public static Dictionary<int, DateTime> avavlibleMovieSchedules(string movieTitles)
    // {
    //     Dictionary<int, DateTime> totals = new();
    //     foreach (var item in MovieTheater.ScheduleList)
    //     {
    //         if (movieTitles == item.movieTitle)
    //         {
    //             totals[item.showingID] = item.showingDateTime;
    //         }

    //     }
    //     return totals;
    // }
    // public static Dictionary<int, DateTime> avavlibleMovieSchedules(int ID, List<string> movies)
    // {
    //     Dictionary<int, DateTime> totals = new();
    //     foreach (var item in MovieTheater.ScheduleList)
    //     {
    //         if (item.movieTitle == MovieTheater.ScheduleList[ID].movieTitle)
    //         {
    //             totals[item.showingID] = item.showingDateTime;
    //         }

    //     }
    //     return totals;
    // }

    // public static DateTime GetDateTimeForShowingID(int showingID)
    // {
    //     foreach (var o in MovieTheater.ScheduleList)
    //     {
    //         if (o.showingID == showingID) return o.showingDateTime;
    //     }
    //     throw new Exception();

    // }

    // public static void UpdatePrefferedCustomer(int ticketNumber, PreferredCustomerTuple? prefferedCustomer, bool usePoints)
    // {
    //     int freeTicketsGiven = ticketNumber;
    //     (int preferredCustomerID, string name, string email, int ticketPoints, int concessionPoints)
    //         newPrefferedCustomer;
    //     if (usePoints)
    //     {
    //         newPrefferedCustomer = (prefferedCustomer?.preferredCustomerID ?? 0,
    //                                                         prefferedCustomer?.name ?? "",
    //                                                         prefferedCustomer?.email ?? "",
    //                                                         prefferedCustomer?.ticketPoints - (10 * ticketNumber) + ticketNumber ?? 0,
    //                                                         prefferedCustomer?.concessionPoints ?? 0);
    //     }
    //     else
    //     {
    //         newPrefferedCustomer = (prefferedCustomer?.preferredCustomerID ?? 0,
    //                                                         prefferedCustomer?.name ?? "",
    //                                                         prefferedCustomer?.email ?? "",
    //                                                         prefferedCustomer?.ticketPoints + ticketNumber ?? 0,
    //                                                         prefferedCustomer?.concessionPoints ?? 0);
    //     }
    //     for (int i = 0; i < MovieTheater.PreferredCustomerList.Count; i++)
    //     {
    //         if (MovieTheater.PreferredCustomerList[i].preferredCustomerID == newPrefferedCustomer.preferredCustomerID)
    //             MovieTheater.PreferredCustomerList[i] = newPrefferedCustomer;
    //     }
    // }
}