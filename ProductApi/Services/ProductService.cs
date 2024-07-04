using ProductApi.Entities;
using ProductApi.Interfaces;
using ProductApi.Models;
using ProductApi.Exceptions;
using AutoMapper;

namespace ProductApi.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IUserProductService _userProductService;
        private readonly IMapper _mapper;

        public ProductService(IProductRepository productRepository, IUserProductService userProductService, IMapper mapper)
        {
            _productRepository = productRepository;
            _userProductService = userProductService;
            _mapper = mapper;
        }

        public List<Product> GetAll()
        {
            return _productRepository.GetAll();
        }

        public List<ProductWithUserFlagDTO> GetAllProductsWithUserFlag(int userId)
        {
            var products = _productRepository.GetAll();
            var userProducts = _userProductService.GetAllUserProductsByUserId(userId);
            List<ProductWithUserFlagDTO> productsWithUserFlag = new List<ProductWithUserFlagDTO>();

            foreach (var product in products)
            {
                var productWithUserFlagDTO = _mapper.Map<ProductWithUserFlagDTO>(product);
                var userProduct = userProducts.Where(up => up.UserId == userId && up.ProductId == product.Id).FirstOrDefault();
                if (userProduct == null)
                    productWithUserFlagDTO.IsFlaggedByUser = false;
                else
                    productWithUserFlagDTO.IsFlaggedByUser = userProduct.IsFlagged;

                productsWithUserFlag.Add(productWithUserFlagDTO);
            }
            return productsWithUserFlag;
        }

        public Product GetById(int id)
        {
            var product = _productRepository.GetById(id);
            if (product == null)
                throw new BadRequestException("Product not found!");

            return product;
        }

        public string AddOrUpdateFlaggedCountAndReturnMessage(ProductIdAndUserFlagDTO productDTO, int userId)
        {
            var product = _productRepository.GetById(productDTO.ProductId);
            if (product == null)
                throw new Exception("Product not found");

            var userProduct = new UserProduct()
            {
                UserId = userId,
                ProductId = productDTO.ProductId,
                IsFlagged = productDTO.IsFlagged
            };

            switch (_userProductService.AddOrUpdateUserProductAndReturnValueOfModifying(userProduct))
            {
                case 0:
                    return "No changes made!";
                case 1:
                    IncreaseTimesFlagged(product);
                    break;
                case -1:
                    DecreaseTimesFlagged(product);
                    break;
            }
            return "Changes saved!";
        }

        private void IncreaseTimesFlagged(Product product)
        {
            product.TimesFlagged++;
            _productRepository.Update(product);
        }

        private void DecreaseTimesFlagged(Product product)
        {
            product.TimesFlagged--;
            _productRepository.Update(product);
        }
    }
}
