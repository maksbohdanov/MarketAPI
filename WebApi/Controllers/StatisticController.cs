using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Business.Interfaces;
using Business.Models;
using Business.Validation;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatisticsController : ControllerBase
    {
        private readonly IStatisticService _statisticService;
        public StatisticsController(IStatisticService statisticService)
        {
            _statisticService = statisticService;
        }

        //GET/api/statistic/popularProducts?productCount=2
        [HttpGet("popularProducts")]
        public async Task<ActionResult<IEnumerable<CustomerModel>>> GetMostPopularProducts([FromQuery] int productCount)
        {
            var products = await _statisticService.GetMostPopularProductsAsync(productCount);
            if(products == null)
            {
                return NotFound();
            }
            return Ok(products);
        }

        //GET/api/statisic/customer/{id}/{productCount} -
        [HttpGet("customer/{id}/{productCount}")]
        public async Task<ActionResult<CustomerModel>> GetNumberOfProducts(int id, int productCount)
        {
            var products = await _statisticService.GetCustomersMostPopularProductsAsync(productCount, id);
            if (products == null)
            {
                return NotFound();
            }
            return Ok(products);
        }

        //GET/api/statistic/activity/{customerCount}?startDate= 2020-7-21&endDate= 2020-7-22
        [HttpGet("activity/{customerCount}")]
        public async Task<ActionResult<CustomerActivityModel>> GetMostActiveCustomers(int customerCount, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var customers = await _statisticService.GetMostValuableCustomersAsync(customerCount , startDate, endDate);
            if (customers == null)
            {
                return NotFound();
            }
            return Ok(customers);
        }

        //GET/api/statistic/income/{categoryId}?startDate= 2020-7-21&endDate= 2020-7-22
        [HttpGet("income/{categoryId}")]
        public async Task<ActionResult<CustomerActivityModel>> GetIncomeInPeriod(int categoryId, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var income = await _statisticService.GetIncomeOfCategoryInPeriod(categoryId, startDate, endDate);
            
            return Ok(income);
        }

    }
}
