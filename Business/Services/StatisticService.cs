using AutoMapper;
using Business.Interfaces;
using Business.Models;
using Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace Business.Services
{
    public class StatisticService : IStatisticService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public StatisticService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductModel>> GetCustomersMostPopularProductsAsync(int productCount, int customerId)
        {
            var receipts = await _unitOfWork.ReceiptRepository.GetAllWithDetailsAsync();
            var products = receipts.Where(r => r.CustomerId == customerId)
                .SelectMany(r => r.ReceiptDetails)
                .OrderBy(rd => rd.Quantity)
                .Select(x => x.Product)
                .Take(productCount);

            return _mapper.Map<IEnumerable<ProductModel>>(products);               
        }

        public async Task<decimal> GetIncomeOfCategoryInPeriod(int categoryId, DateTime startDate, DateTime endDate)
        {
            var receipts = await _unitOfWork.ReceiptRepository.GetAllWithDetailsAsync();
            var receiptDetails = receipts
                .Where(r => r.OperationDate >= startDate && r.OperationDate <= endDate)
                .SelectMany(r => r.ReceiptDetails)
                .Where(rd => rd.Product.ProductCategoryId == categoryId);

            return receiptDetails.Sum(rd => rd.Quantity * rd.DiscountUnitPrice);
        }

        public async Task<IEnumerable<ProductModel>> GetMostPopularProductsAsync(int productCount)
        {
            var receiptDetails = await _unitOfWork.ReceiptDetailRepository.GetAllWithDetailsAsync();
            var products = receiptDetails
                .OrderByDescending(rd => rd.Quantity)
                .Select(rd => rd.Product)
                .Take(productCount);
            return _mapper.Map<IEnumerable<ProductModel>>(products);
        }

        public async Task<IEnumerable<CustomerActivityModel>> GetMostValuableCustomersAsync(int customerCount, DateTime startDate, DateTime endDate)
        {
            var receipts = await _unitOfWork.ReceiptRepository.GetAllWithDetailsAsync();

            var customers = receipts
                .GroupBy(r => r.Customer)
                .Select(a => new CustomerActivityModel
                {
                    CustomerId = a.Key.Id,
                    CustomerName = $"{a.Key.Person?.Name} {a.Key.Person?.Surname}",
                    ReceiptSum = receipts
                                    .Where(r => r.CustomerId == a.Key.Id)
                                    .Where(r => r.OperationDate >= startDate && r.OperationDate <= endDate)
                                    .SelectMany(r => r.ReceiptDetails)
                                    .Sum(rd => rd.Quantity * rd.DiscountUnitPrice)

                })
                .OrderByDescending(c => c.ReceiptSum)
                .Take(customerCount);

            return customers;
        }
    }
}
