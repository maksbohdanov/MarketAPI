using AutoMapper;
using Business.Interfaces;
using Business.Models;
using Business.Validation;
using Data.Entities;
using Data.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace Business.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task AddAsync(ProductModel model)
        {
            if (model == null)
            {
                throw new MarketException("The model is null.");
            }
            if (string.IsNullOrEmpty(model.ProductName))
            {
                throw new MarketException("Field product name is null.");
            }
            if (model.Price <= 0)
            {
                throw new MarketException("Price cannot be negative");
            }
            var product = _mapper.Map<Product>(model);
            await _unitOfWork.ProductRepository.AddAsync(product);
            await _unitOfWork.SaveAsync();
        }

        public async Task AddCategoryAsync(ProductCategoryModel categoryModel)
        {
            if (categoryModel == null)
            {
                throw new MarketException("The model is null.");
            }
            if (string.IsNullOrEmpty(categoryModel.CategoryName))
            {
                throw new MarketException("Field category name is null.");
            }
            var category = _mapper.Map<ProductCategory>(categoryModel);
            await _unitOfWork.ProductCategoryRepository.AddAsync(category);
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteAsync(int modelId)
        {
            await _unitOfWork.ProductRepository.DeleteByIdAsync(modelId);
            await _unitOfWork.SaveAsync();
        }

        public async Task<IEnumerable<ProductModel>> GetAllAsync()
        {
            var products = await _unitOfWork.ProductRepository.GetAllWithDetailsAsync();
            return _mapper.Map<IEnumerable<ProductModel>>(products);
        }

        public async Task<IEnumerable<ProductCategoryModel>> GetAllProductCategoriesAsync()
        {            
            var categories = await _unitOfWork.ProductCategoryRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<ProductCategoryModel>>(categories);
        }

        public async Task<IEnumerable<ProductModel>> GetByFilterAsync(FilterSearchModel filterSearch)
        {
            var products = await _unitOfWork.ProductRepository.GetAllWithDetailsAsync();
            var productsFiltered = products.Where(p =>
                    (filterSearch?.CategoryId ?? p.ProductCategoryId) == p.ProductCategoryId &&
                    (filterSearch?.MinPrice ?? p.Price) <= p.Price &&
                    (filterSearch?.MaxPrice ?? p.Price) >= p.Price);
            return _mapper.Map<IEnumerable<ProductModel>>(productsFiltered);
        }

        public async Task<ProductModel> GetByIdAsync(int id)
        {
            var product = await _unitOfWork.ProductRepository.GetByIdWithDetailsAsync(id);
            return _mapper.Map<ProductModel>(product);
        }

        public async Task RemoveCategoryAsync(int categoryId)
        {
            await _unitOfWork.ProductCategoryRepository.DeleteByIdAsync(categoryId);
            await _unitOfWork.SaveAsync();
        }

        public async Task UpdateAsync(ProductModel model)
        {
            if (model == null)
            {
                throw new MarketException("The model is null.");
            }
            if (string.IsNullOrEmpty(model.ProductName))
            {
                throw new MarketException("Field product name is null.");
            }
            var product = _mapper.Map<Product>(model);
            _unitOfWork.ProductRepository.Update(product);
            await _unitOfWork.SaveAsync();
        }

        public async Task UpdateCategoryAsync(ProductCategoryModel categoryModel)
        {
            if (categoryModel == null)
            {
                throw new MarketException("The model is null.");
            }
            if (string.IsNullOrEmpty(categoryModel.CategoryName))
            {
                throw new MarketException("Field category name is null.");
            }
            var category = _mapper.Map<ProductCategory>(categoryModel);
            _unitOfWork.ProductCategoryRepository.Update(category);
            await _unitOfWork.SaveAsync();
        }
    }
}
