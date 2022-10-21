using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Interfaces;
using Business.Models;
using Business.Validation;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReceiptsController : ControllerBase
    {
        private readonly IReceiptService _receiptService;
        public ReceiptsController(IReceiptService receiptService)
        {
            _receiptService = receiptService;
        }

        //GET: api/receipts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReceiptModel>>> GetReceipts()
        {
            var receipts = await _receiptService.GetAllAsync();
            if (receipts == null)
            {
                return NotFound();
            }
            return Ok(receipts);
        }

        //GET: api/receipts/1
        [HttpGet("{id}")]
        public async Task<ActionResult<ReceiptModel>> GetReceiptById(int id)
        {
            var receipt = await _receiptService.GetByIdAsync(id);
            if (receipt == null)
            {
                return NotFound();
            }
            return Ok(receipt);
        }

        //GET: api/receipts/1/details
        [HttpGet("{id}/details")]
        public async Task<ActionResult<ReceiptModel>> GetReceiptDetails(int id)
        {
            var receipt = await _receiptService.GetReceiptDetailsAsync(id);
            if (receipt == null)
            {
                return NotFound();
            }
            return Ok(receipt);
        }

        //GET: api/receipts/1/sum
        [HttpGet("{id}/sum")]
        public async Task<ActionResult<decimal>> GetReceiptSum(int id)
        {
            var receipt = await _receiptService.GetReceiptDetailsAsync(id);
            if (receipt == null)
            {
                return NotFound();
            }           
            
            return Ok(receipt.Sum(x => x.DiscountUnitPrice * x.Quantity));
        }

        //GET: api/receipts/period?startDate=2021-12-1&endDate=2020-12-31
        [HttpGet("period")]
        public async Task<ActionResult<decimal>> GetReceiptByPeriod([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var receipt = await _receiptService.GetReceiptsByPeriodAsync(startDate, endDate);
            if (receipt == null)
            {
                return NotFound();
            }

            return Ok(receipt);
        }


        // POST: api/receipts
        [HttpPost]
        public async Task<ActionResult> AddReceipt([FromBody] ReceiptModel value)
        {
            try
            {
                await _receiptService.AddAsync(value);

                return CreatedAtAction(nameof(AddReceipt), new { id = value.Id }, value);
            }
            catch (MarketException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/receipts/1
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateProduct(int id, [FromBody] ReceiptModel value)
        {
            try
            {
                await _receiptService.UpdateAsync(value);
                return Ok();
            }
            catch (MarketException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //PUT/api/receipts/{id}/products/add/{productId}/{quantity}
        [HttpPut("{id}/products/add/{productId}/{quantity}")]
        public async Task<ActionResult> AddProduct(int id, int productId, int quantity)
        {
            try
            {
                await _receiptService.AddProductAsync(productId, id, quantity);
                return Ok();
            }
            catch (MarketException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //PUT/api/receipts/{id}/products/remove/{productId}/{quantity}
        [HttpPut("{id}/products/remove/{productId}/{quantity}")]
        public async Task<ActionResult> RemoveProduct(int id, int productId, int quantity)
        {
            try
            {
                await _receiptService.RemoveProductAsync(productId, id, quantity);
                return Ok();
            }
            catch (MarketException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //PUT/api/receipts/{id}/checkout – change a receipt check out value to true
        [HttpPut("{id}/checkout")]
        public async Task<ActionResult> CheckOutProduct(int id)
        {
            try
            {
                await _receiptService.CheckOutAsync(id);
                return Ok();
            }
            catch (MarketException ex)
            {
                return BadRequest(ex.Message);
            }
        }



        // DELETE: api/receipts/1
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            await _receiptService.DeleteAsync(id);
            return Ok();
        }




        ////PUT/api/receipts/categories{id}
        //[HttpPut("categories/{id}")]
        //public async Task<ActionResult> UpdatCategory(int id, [FromBody] ProductCategoryModel value)
        //{
        //    try
        //    {
        //        await _receiptService.UpdateCategoryAsync(value);
        //        return Ok();
        //    }
        //    catch (MarketException ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}

      
    }
}
