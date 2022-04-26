using System;
using AutoMapper;
using FluentAssertions;
using Gambling.Backend.Common;
using Gambling.Backend.Dtos;
using Gambling.Backend.Entities;
using Gambling.Backend.Services;
using Moq;
using Xunit;

namespace Gambling.Backend.Tests.Services;

public class BetServicesTests
{
    [Fact]
    public void TestGetRandomNumber()
    {
        //Arrange
        var mapper = new Mock<IMapper>();
        var playersRepo = new Mock<IRepository<Player>>();
        var betServices = new BetServices(mapper.Object, playersRepo.Object);
        var min = 0; var max = 9;
        //Act
        var random = betServices.GetRandomNumber(min, max);
        
        //Assert
        random.Should().BeInRange(min, max);
    }
    [Fact]
    public void TestCheckPlayerBet_ShouldReturnWonResult()
    {
        //Arrange
        var mapper = new Mock<IMapper>();
        var playersRepo = new Mock<IRepository<Player>>();
        var betServices = new BetServices(mapper.Object, playersRepo.Object);
        var rand = 3; var points =100; var account = 10000;
        var bet = new CreateBetDto(Guid.NewGuid(), points, rand);
        //Act
        var result = betServices.CheckPlayerBet(rand, account, bet);  
        
        //Assert
        Assert.NotNull(result);
        Assert.Equal(Status.Won.ToString(), result.Status);
        Assert.Equal((points * 9)+ account, result.Account);
        Assert.Equal($"+{points * 9}", result.Points);
    }
    [Fact]
    public void TestCheckPlayerBet_ShouldReturnLoseResult()
    {
        //Arrange
        var mapper = new Mock<IMapper>();
        var playersRepo = new Mock<IRepository<Player>>();
        var betServices = new BetServices(mapper.Object, playersRepo.Object);
        var rand = 3; var points =100; var account = 10000;
        var bet = new CreateBetDto(Guid.NewGuid(), points, 7);
        //Act
        var result = betServices.CheckPlayerBet(rand, account, bet);  
        
        //Assert
        Assert.NotNull(result);
        Assert.Equal(Status.Lose.ToString(), result.Status);
        Assert.Equal(account-points, result.Account);
        Assert.Equal($"-{points}", result.Points);
    }
}