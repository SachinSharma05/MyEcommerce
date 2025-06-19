using MyEcommerce.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEcommerce.Core.Interface
{
    public interface IAuthService
    {
        Task<UserEntity?> RegisterAsync(UserEntity user, string password);
        Task<UserEntity?> LoginAsync(string email, string password);
        Task<bool> ChangePasswordAsync(int userId, string currentPassword, string newPassword);
        Task<UserEntity?> GetProfileAsync(int userId);
        string GenerateToken(UserEntity user);
    }
}
