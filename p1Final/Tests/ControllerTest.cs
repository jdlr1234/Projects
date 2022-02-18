using System;
using System.Linq;
using System.Text;
using Moq;
using Xunit;
using BL;
using Models;
using WebAPI.Controllers;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Tests;
public class ControllerTest
{
    [Fact]
    public void StoreControllerListShouldReturnListOfStores()
    {
        var mockBL = new Mock<IBL>();
        mockBL.Setup(x => x.GetAllStores()).Returns(
            new List<Store>
            {
                new Store
                {
                    Id = 1,
                    Name = "Test One",
                    City = "City One",
                    State = "State One",
                },
                new Store
                {
                    Id = 2,
                    Name = "Test Two",
                    City = "City Two",
                    State = "State Two",
                }
            }
        );
        var stoCntrllr = new StoreController(mockBL.Object);
        var result = stoCntrllr.Get();

        Assert.NotNull(result);
        Assert.IsType<List<Store>>(result);
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public void ProductControllerListShouldReturnListOfProducts()
    {
        var mockBL = new Mock<IBL>();
        mockBL.Setup(x => x.GetAllProducts()).Returns(
            new List<Product>
            {
                new Product
                {
                    Id = 1,
                    ProductName = "Test One",
                    Price = 11,
                    Brand = "Brand2",
                    Inventory = 12
                },
                new Product
                {
                    Id = 11,
                    ProductName = "Test Two",
                    Price = 22,
                    Brand = "Brand1",
                    Inventory = 22
                }
            }
        );
        var prodCntrllr = new ProductController(mockBL.Object);
        var result = prodCntrllr.Get();

        Assert.NotNull(result);
        Assert.IsType<List<Product>>(result);
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public void ControllerTestShouldWork()
    {
        bool b = true;
        Assert.True(b);
    }
}
