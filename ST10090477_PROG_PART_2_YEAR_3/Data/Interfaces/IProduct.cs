using ST10090477_PROG_PART_2_YEAR_3.ViewModels;
using System.Security.Claims;

namespace ST10090477_PROG_PART_2_YEAR_3.Data.Interfaces
{
    public interface IProduct
    {
        Task<(bool success, string message)>CreateProduct(CreateProductVM createProductVM, ClaimsPrincipal user);

        Task<List<CreateProductVM>> GetAllProductByIDAsync(ClaimsPrincipal user);

        Task<List<CreateProductVM>> ViewSpecificUserProductAsync(string id);
    }
}
