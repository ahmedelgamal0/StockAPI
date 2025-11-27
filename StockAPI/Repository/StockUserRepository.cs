using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository;

public class StockUserRepository : IStockUserRepository
{
    private readonly ApplicationDBContext _context;
    public StockUserRepository(ApplicationDBContext context)
    {
        _context = context;
    }

    public async Task<StockUser> CreateAsync(StockUser stockUser)
    {
        await _context.StockUsers.AddAsync(stockUser);
        await _context.SaveChangesAsync();
        return stockUser;
    }

    public async Task<List<Stock>> GetStockUser(AppUser user)
    {
        return await _context.StockUsers.Where(u => u.AppUserId == user.Id)
        .Select(stock => new Stock
        {
            Id = stock.StockId,
            Symbol = stock.Stock.Symbol,
            CompanyName = stock.Stock.CompanyName,
            Purchase = stock.Stock.Purchase,
            LastDiv = stock.Stock.LastDiv,
            Industry = stock.Stock.Industry,
            MarketCap = stock.Stock.MarketCap

        }).ToListAsync();
    }
}
