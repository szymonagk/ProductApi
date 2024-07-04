﻿using ProductApi.Entities;
using ProductApi.Models;

namespace ProductApi.Interfaces
{
    public interface IUserProductService
    {
        bool CheckIfFlagChanged(ProductIdAndUserFlagDTO productDTO, int userId);
        bool ShouldAddToTotalFlags(ProductIdAndUserFlagDTO productDTO, int userId);
        int AddOrUpdateUserProductAndReturnValueOfModifying(UserProduct userProduct);
        List<UserProduct> GetAllUserProductsByUserId(int userId);
    }
}
