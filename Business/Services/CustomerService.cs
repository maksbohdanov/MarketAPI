using AutoMapper;
using Business.Interfaces;
using Business.Models;
using Data.Interfaces;
using Business.Validation;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Data.Entities;

namespace Business.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CustomerService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CustomerModel>> GetAllAsync()
        {
            var customers = await _unitOfWork.CustomerRepository.GetAllWithDetailsAsync();
            return _mapper.Map<IEnumerable<CustomerModel>>(customers);
        }

        public async Task<CustomerModel> GetByIdAsync(int id)
        {
            var customer = await _unitOfWork.CustomerRepository.GetByIdWithDetailsAsync(id);
            return _mapper.Map<CustomerModel>(customer);
        }

        public async Task<IEnumerable<CustomerModel>> GetCustomersByProductIdAsync(int productId)
        {
            var customers = await _unitOfWork.CustomerRepository.GetAllWithDetailsAsync();
            
            var customersFiltered = customers.Where(c => c.Receipts
                .SelectMany(r => r.ReceiptDetails)
                .Any(rd => rd.ProductId == productId));

            return _mapper.Map<IEnumerable<CustomerModel>>(customersFiltered);

        }

        public async Task AddAsync(CustomerModel model)
        {
            if (model == null)
            {
                throw new MarketException("The model is null.");
            }
            if(string.IsNullOrEmpty(model.Name))
            {
                throw new MarketException("Field name is null.");
            }
            if (string.IsNullOrEmpty(model.Surname))
            {
                throw new MarketException("Field surname is null.");
            }
            if (model.BirthDate.Year > 2022 || model.BirthDate.Year < 1900)
            {
                throw new MarketException("Incorrect birth date.");
            }
            if (model.DiscountValue < 0)
            {
                throw new MarketException("Discount value cannot be lower than 0.");
            }
            var customer = _mapper.Map<Customer>(model);
            await _unitOfWork.CustomerRepository.AddAsync(customer);
            await _unitOfWork.SaveAsync();
        }     

        public async Task UpdateAsync(CustomerModel model)
        {
            if (model == null)
            {
                throw new MarketException("The model is null.");
            }
            if (string.IsNullOrEmpty(model.Name))
            {
                throw new MarketException("Field name is null.");
            }
            if (string.IsNullOrEmpty(model.Surname))
            {
                throw new MarketException("Field surname is null.");
            }
            if (model.BirthDate.Year > 2022 || model.BirthDate.Year < 1900)
            {
                throw new MarketException("Incorrect birth date.");
            }
            if (model.DiscountValue < 0)
            {
                throw new MarketException("Discount value cannot be lower than 0.");
            }
            var customer = _mapper.Map<Customer>(model);
            _unitOfWork.CustomerRepository.Update(customer);
            _unitOfWork.PersonRepository.Update(customer.Person);
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteAsync(int modelId)
        {
            await _unitOfWork.CustomerRepository.DeleteByIdAsync(modelId);
            await _unitOfWork.SaveAsync();
        }
    }
}
