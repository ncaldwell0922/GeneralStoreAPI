using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GeneralStoreAPI.Data;
using GeneralStoreAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace GeneralStoreAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TransactionController : Controller
    {
        private GeneralStoreDBContext _db;
        public TransactionController(GeneralStoreDBContext db) {
            _db = db;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTransaction(TransactionEdit newTransaction)
        {
            Transaction transaction = new Transaction()
            {
                ProductId = newTransaction.ProductId,
                CustomerId = newTransaction.CustomerId,
                Quantity = newTransaction.Quantity
            };

            _db.Transactions.Add(transaction);
            await _db.SaveChangesAsync();
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTransactions()
        {
            var transactions = await _db.Transactions.ToListAsync();
            return Ok(transactions);
        }

        [HttpPut]
        [Route("{id}")]

        public async Task<IActionResult> UpdateTransaction([FromForm] TransactionEdit model, [FromRoute] int CustomerId)
        {
            var oldTransaction = await _db.Transactions.FindAsync(CustomerId);
            if (oldTransaction == null)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            if (!string.IsNullOrEmpty(model.ProductId))
            {
                oldTransaction.ProductId = model.ProductId;
            }
            if (!string.IsNullOrEmpty(model.CustomerId))
            {
                oldTransaction.CustomerId = model.CustomerId;
            }
            if (!string.IsNullOrEmpty(model.Quantity))
            {
                oldTransaction.Quantity = model.Quantity;
            }
            await _db.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteTransaction([FromRoute] int CustomerId)
        {
            var transaction = await _db.Transactions.FindAsync(CustomerId);
            if (transaction == null)
            {
                return NotFound();
            }
            _db.Transactions.Remove(transaction);
            await _db.SaveChangesAsync();
            return Ok();
        }
    }
}