using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;

namespace api.Interfaces;

public interface IStockUserRepository
{
    Task<List<Stock>> GetStockUser(AppUser user);
    Task<StockUser> CreateAsync(StockUser stockUser);
}
