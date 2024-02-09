﻿using Contracts;
using MassTransit;
using MongoDB.Entities;
using SearchService.Data;
using SearchService.Models;

namespace SearchService.Consumer;

public class AuctionFinishedConsumer:IConsumer<AuctionFinished>
{
    public async Task Consume(ConsumeContext<AuctionFinished> context)
    {
        Console.WriteLine("----->Consuming auction finished");
        var auction = await DB.Find<Item>().OneAsync(context.Message.AuctionId);
        if (context.Message.ItemSold)
        {
            auction.Winner = context.Message.Winner;
            auction.SoldAmount = (int)context.Message.Amount;
        }

        auction.Status = "Finished";
        await auction.SaveAsync();
    }
}