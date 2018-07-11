﻿using DutchTreat.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DutchTreat.Data
{
    public class DutchRepository : IDutchRepository
    {
        private readonly DutchContext _ctx;
        private readonly ILogger<DutchRepository> _logger;

        public DutchRepository(DutchContext ctx, ILogger<DutchRepository> logger)
        {
            _ctx = ctx;
            _logger = logger;
        }

        public IEnumerable<Order> GetAllOrders(bool includeItems)
        {
            if (includeItems)
            {
                return _ctx.Orders
                               .Include(o => o.Items)
                               .ThenInclude(i => i.Product)
                               .ToList();
            }
            else
            {
                return _ctx.Orders.ToList();
            }
        }

        public Order GetOrderById(string username, int id)
        {
            return _ctx.Orders
                       .Include(o => o.Items)
                       .ThenInclude(i => i.Product)
                       .Where(o => o.Id == id && o.User.UserName == username)
                       .FirstOrDefault();
        }

        public IEnumerable<Product> GetAllProducts()
        {
            try
            {
                _logger.LogInformation("Get All Product Was Called");
                return _ctx.Products
                    .OrderBy(p => p.Title)
                    .ToList();
            }
            catch(Exception ex)
            {
                _logger.LogError($"Failed to get all products: {ex}");
                return null;
            }
           
        }

        public IEnumerable<Product> GetProductByCategory(string category)
        {
            try
            {
                return _ctx.Products
                      .OrderBy(p => p.Category == category)
                      .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get products by catergory: {ex}");
                return null;
            }
        }

        public bool SaveAll()
        {
            return _ctx.SaveChanges() > 0;
        }

        public void AddEntity(object model)
        {
            _ctx.Add(model);
        }

        public IEnumerable<Order> GetAllOrdersByUser(string username, bool includeItems)
        {
            if (includeItems)
            {
                return _ctx.Orders
                               .Where(o=> o.User.UserName == username)
                               .Include(o => o.Items)
                               .ThenInclude(i => i.Product)
                               .ToList();
            }
            else
            {
                return _ctx.Orders
                    .Where(o => o.User.UserName == username)
                    .ToList();
            }
        }
    }
}
