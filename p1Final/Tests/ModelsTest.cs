using Xunit;
using Models;
using CustomExceptions;
using System.Collections.Generic;
namespace Tests;

//Arrange
//Act
//Assert
public class ModelsTest
{
    [Fact]
    public void ManagerShouldMakeStore(){
        
        Store testStore = new Store();
        Assert.NotNull(testStore);
    }
    [Fact]
    public void StoreShouldSetValidData(){
        Store testStore = new Store();
        int id = 123;
        string name = "Test Name";
        string city = "Test City";
        string state = "Test State";

        testStore.Name = name;
        testStore.City = city;
        testStore.State= state;

        Assert.Equal(name, testStore.Name);
        Assert.Equal(city, testStore.City);
        Assert.Equal(state, testStore.State);

    }
    //test if exception is thrown when input not valid
    [Fact]

    public void StoreShouldNotSetInvalidName() {
        Store testStore = new Store();
        //going to follow the constraint of characters we have set in models
        string nonRegexName = "$%&*((&";
        Assert.Throws<InputInvalidException>(() => testStore.Name = nonRegexName);
    }

    //same as above
    [Theory]
    [InlineData("N@me")]
    [InlineData("        ")]
    [InlineData("buyer456%")]
    public void OnlyNumbersAndLetterForStoreName(string input)
    {
        Store testStore = new Store();

        Assert.Throws<InputInvalidException>(() => testStore.Name = input);
    }

    [Fact]
    public void StoreShouldHaveCustomToStringMethod()
    {
        Store testStore = new Store
        {
            Id = 123,
            Name = "Test Store",
            City = "Test City",
            State = "Test State"
        };
        string expectedOutput = "Reference Id: 123 \nName: Test Store \nLocation: Test City, Test State \n";

        Assert.Equal(expectedOutput, testStore.ToString());
    }

        //same as above
    [Theory]
    [InlineData("N@me")]
    [InlineData("        ")]
    [InlineData("buyer456%")]
    public void OnlyNumbersAndLetterForProductName(string input)
    {
        Store testStore = new Store();

        Assert.Throws<InputInvalidException>(() => testStore.Name = input);
    }
    
    [Fact]
    public void StoresShouldBeAbleToBeSetOrders()
    {
        Store testStore = new Store();
        List<Order> testOrders = new List<Order>();
        testStore.Orders = testOrders;

        Assert.NotNull(testStore.Orders);
        Assert.Equal(0, testStore.Orders.Count);
    }


}