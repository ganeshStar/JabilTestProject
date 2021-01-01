using JabilScreeningTest.Entities;
using JabilScreeningTest.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace JabilScreeningTest.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly ApplicationDbContext appDbContext;
        public InventoryController(ApplicationDbContext _appDbContext)
        {
            appDbContext = _appDbContext;
        }

        /// <summary>
        /// Get All InventoryItems
        /// </summary>
        /// <returns></returns>
        [HttpGet("getAllProducts")]
        public async Task<IActionResult> GetAllProducts()
        {
            List<InventoryModel> model = new List<InventoryModel>();
            try
            {
                model = await (from item in appDbContext.Inventories.OrderByDescending(ee=>ee.CreatedDate)
                               select new InventoryModel
                               {
                                   Id = item.Id,
                                   Name = item.Name,
                                   Price = item.Price,
                                   Quantity = item.Quantity,
                                   CreatedDate = item.CreatedDate.ToString("MM/dd/yyyy hh:mm tt")
                               }).ToListAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(model);
        }

        /// <summary>
        /// Create A Inventory Record
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("createProduct")]
        public async Task<IActionResult> CreateProduct([FromBody] InventoryModel model)
        {
            int res;
            try
            {
                var result = new Inventory() { Name = model.Name, Price = model.Price, Quantity = model.Quantity, CreatedDate = DateTime.UtcNow, UpdatesDate = DateTime.UtcNow, IsDeleted = false };
                appDbContext.Inventories.Add(result);
                await appDbContext.SaveChangesAsync();
                res = result.Id;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(res);
        }

        /// <summary>
        /// Get a product to edit and update
        /// </summary>
        /// <returns></returns>
        [HttpGet("getProduct")]
        public async Task<IActionResult> getProduct([FromQuery][Required] int id)
        {
            InventoryModel model = new InventoryModel();
            try
            {
                model = await (from item in appDbContext.Inventories
                               where (item.Id == id)
                               select new InventoryModel
                               {
                                   Id = item.Id,
                                   Name = item.Name,
                                   Price = item.Price,
                                   Quantity = item.Quantity,
                                   CreatedDate = item.CreatedDate.ToString("MM/dd/yyyy hh:mm tt")
                               }).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(model);
        }


        /// <summary>
        /// Update A Inventory Record
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("updateProduct")]
        public async Task<IActionResult> UpdateProduct([FromBody] InventoryModel model)
        {
            bool res = false;
            try
            {
                if (model.Id == 0)
                {
                    return BadRequest();
                }
                var inventoryItems = await appDbContext.Inventories.Where(ee => ee.Id == model.Id && ee.IsDeleted == false).FirstOrDefaultAsync();
                if (inventoryItems != null)
                {
                    inventoryItems.Name = model.Name;
                    inventoryItems.Price = model.Price;
                    inventoryItems.Quantity = model.Quantity;
                    inventoryItems.UpdatesDate = DateTime.UtcNow;
                    appDbContext.Inventories.Update(inventoryItems);
                    await appDbContext.SaveChangesAsync();
                    res = true;
                    return Ok(res);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(res);
        }
    }
}
