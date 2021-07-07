using Microsoft.EntityFrameworkCore;
using posuno.Api.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace posuno.Api.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;

        public SeedDb(DataContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();
            await CheckUserAsync();
            await CheckCustomerAsync();
            await CheckProductsAsync();
        }

        private async Task CheckUserAsync()
        {
            if (!_context.Users.Any())
            {
                _context.Users.Add(new User { Email = "pedro@yopmail.com", FirstName = "Pedro", LastName = "Velasquez", Password = "1234567" });
                _context.Users.Add(new User { Email = "fresia@yopmail.com", FirstName = "Fresia", LastName = "Pino", Password = "1234567" });
                await _context.SaveChangesAsync();

            }
        }

        private async Task CheckCustomerAsync()
        {
            if (!_context.Customers.Any())
            {
                User user = await _context.Users.FirstOrDefaultAsync();
                for (int i = 1; i <= 50; i++)
                {
                    _context.Customers.Add(new Customer { FirstName = $"Cliente {i}", LastName = $"Apellido {i}", Phonenumber = "3005128950", Address = "Calle 19 # 81 b ",Email = $"Cliente{i}@yopmail.com", IsActive = true, User = user });
                }
                await _context.SaveChangesAsync();
            }
        }

        private async Task CheckProductsAsync()
        {
            if (!_context.Products.Any())
            {
                Random random = new Random();
                User user = await _context.Users.FirstOrDefaultAsync();
                for (int i = 1; i <= 200; i++)
                {
                    _context.Products.Add(new Product { Name = $"Producto {i}", Description = $"Producto {i}", Price = random.Next(5, 1000), Stock = random.Next(0, 500), IsActive = true, User = user });
                }
                await _context.SaveChangesAsync();
            }
        }
    }
}
