using ProductApi.Interfaces;
using ProductApi.Exceptions;
using ProductApi.Models;
using ProductApi.Entities;

namespace ProductApi.Services
{
    public class UserProductService : IUserProductService
    {
        private readonly IUserProductRepository _userProductRepository;

        public UserProductService(IUserProductRepository userProductRepository)
        {
            _userProductRepository = userProductRepository;
        }

        public bool CheckIfFlagChanged(ProductIdAndUserFlagDTO productDTO, int userId)
        {
            var userProduct = _userProductRepository.GetByProductIdAndUserId(productDTO.ProductId, userId);
            if (userProduct == null)
                throw new BadRequestException("UserProduct not found!");

            if (userProduct.IsFlagged != productDTO.IsFlagged)
                return true;

            return false;
        }

        public bool ShouldAddToTotalFlags(ProductIdAndUserFlagDTO productDTO, int userId)
        {
            var userProduct = _userProductRepository.GetByProductIdAndUserId(productDTO.ProductId, userId);
            if (userProduct == null)
                throw new BadRequestException("UserProduct not found!");

            if (productDTO.IsFlagged)
            {
                _userProductRepository.Add(userProduct);
                return true;
            }

            _userProductRepository.Delete(userProduct);
            return false;
        }

        public int AddOrUpdateUserProductAndReturnValueOfModifying(UserProduct userProduct)
        {
            int valueOfModifying = 1;
            var oldUserProduct = _userProductRepository.GetByProductIdAndUserId(userProduct.ProductId, userProduct.UserId);
            if(oldUserProduct != null)
            {
                if (oldUserProduct.IsFlagged != userProduct.IsFlagged)
                {
                    _userProductRepository.Delete(oldUserProduct);
                    if (!userProduct.IsFlagged)
                        valueOfModifying = -1;
                }
                else
                    return 0;
            }
            else if (oldUserProduct == null && userProduct.IsFlagged == false)
                return 0;

            _userProductRepository.Add(userProduct);
            return valueOfModifying;
        }

        public List<UserProduct> GetAllUserProductsByUserId(int userId)
        {
            return _userProductRepository.GetAllByUserId(userId);
        }
    }
}
