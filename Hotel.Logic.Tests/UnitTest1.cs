using Hotel.Data;

namespace Hotel.Logic.Tests;

public class UnitTest1
{
    string roomPricesFileRoute = Hotel.Data.ReadFiles.FindFile("Roomprices.txt");

    [Fact]
    public void CanReserve()
    {
        Assert.True(Logic.CanReserveRoom(101, new DateOnly(2024, 01, 11)));
    }

    [Fact]
    public void CannotReserve()
    {
        Assert.False(Logic.CanReserveRoom(101, new DateOnly(2024, 01, 10)));
    }

    [Fact]
    public void CanCreateCustumer()
    {
        // Add customer to the customer list
        Customer newCustomer = Logic.CreateCustomer();
        // Find the customer in the list
        bool isCustomerInTheList = false;
        foreach (var customer in Logic.customers)
        {
            if (customer.Equals(newCustomer))
            {
                isCustomerInTheList = true;
                return;
            }
        }
        // Throw error if the customer is not found
        Assert.True(isCustomerInTheList);
    }
}
