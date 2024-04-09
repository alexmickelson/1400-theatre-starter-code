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


  public static List<ConcessionsReportTuple> ConcessionDailyReport(DateOnly dateOnly)
  {
    return new();
  }

  public static void PurchaseMenuItem(string name, string itemName, int quantity, bool usePoints)
  {
  }

}