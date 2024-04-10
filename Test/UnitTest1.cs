using Shared;
namespace Test;

public class UnitTest1
{

    //START TESTS - FinalProject 1

    [Fact]
    public void CanLoadConcessionMenuDataFromFile()
    {
        MovieTheater.ReadDataInFromAllFiles();
        Assert.NotEmpty(MovieTheater.ConcessionMenuList); //IF file file was read in OK, then List will not be empty
    }


    [Fact]
    public void CanLoadConcessionSalesDataFromFile()
    {
        MovieTheater.ReadDataInFromAllFiles();
        Assert.NotEmpty(MovieTheater.ConcessionSaleList); //IF file file was read in OK, then List will not be empty
    }

    //PurchaseMenuItem
    [Fact]
    public void ConcessionPurchaseItem_NormalCustomer()
    {
        MovieTheater.ReadDataInFromAllFiles();
        MovieTheater.ConcessionSaleList = new();
        MovieTheater.PurchaseMenuItem("Bob", "Large Soda", 5, false);
        Assert.Single(MovieTheater.ConcessionSaleList);
    }
    //PurchaseMenuItem
    [Fact]
    public void ConcessionPurchaseItemDoesNotExist_NormalCustomer()
    {
        MovieTheater.ReadDataInFromAllFiles();
        MovieTheater.ConcessionSaleList = new();
        MovieTheater.ConcessionMenuList = new(); //Large Soda will not exist any more
        try
        {
            MovieTheater.PurchaseMenuItem("Bob", "Large Soda", 5, false);
        }
        catch (Exception ex)
        {
            Assert.True(true); //We expect an exception to be caught
            return;
        }
        Assert.Fail("Expected an exception thrown since menuItem does not exist");
    }
    //PurchaseMenuItem
    [Fact]
    public void ConcessionPurchaseItemQuantityPrice_Check()
    {
        MovieTheater.ReadDataInFromAllFiles();
        MovieTheater.ConcessionSaleList = new();
        //ACT
        MovieTheater.PurchaseMenuItem("Bob", "Large Soda", 4, false);
        //ASSERT
        Assert.Equal(4, MovieTheater.ConcessionSaleList[0].quantitySold);
        Assert.Equal(4 * 5.00M, MovieTheater.ConcessionSaleList[0].revenueCollected);
    }

    [Fact]
    public void ConcessionDailyReportNoConcessionsPurchased()
    {
        MovieTheater.ReadDataInFromAllFiles();
        MovieTheater.ConcessionSaleList = new();
        var dailyReport = MovieTheater.ConcessionReport5_ItemTotalsPerDay(DateOnly.Parse("1/1/2001"));
        Assert.Empty(dailyReport);
    }

    [Fact]
    public void ConcessionDailyReportOption3_CanDisplaySales()
    {
        MovieTheater.ReadDataInFromAllFiles();
        DateTime date = DateTime.Today;
        MovieTheater.ConcessionSaleList = [(date, "Large Soda", 5, 50m, null)];
        var dailyReport = MovieTheater.ConcessionReport5_ItemTotalsPerDay(DateOnly.FromDateTime(date));
        Assert.Single(dailyReport);
    }



    //END TESTS - FinalProject 1






}