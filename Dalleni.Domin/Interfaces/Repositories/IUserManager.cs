using Microsoft.AspNetCore.Identity;

namespace Dalleni.Domin.Interfaces.Repositories
{
    public interface IUserManager<TUser> where TUser : class
    {
        Task<TUser?> FindByIdAsync(Guid userId, bool trackChanges = false);
        Task<TUser?> FindByIdAsync(string userId, bool trackChanges = false);
        Task<TUser?> FindByEmailAsync(string email, bool trackChanges = false);
        Task<TUser?> FindByNameAsync(string userName, bool trackChanges = false);
        Task<IEnumerable<TUser>> GetAllUsersAsync();
        Task<IEnumerable<TUser>> GetAllAdminsAsync();
        Task<IdentityResult> CreateAsync(TUser user, string password);
        Task<IdentityResult> UpdateAsync(TUser user);
        Task<IdentityResult> DeleteAsync(TUser user);
        Task<bool> IsEmailUniqueAsync(string email);
        Task<bool> IsUserNameUniqueAsync(string userName);
        Task<bool> CheckPasswordAsync(TUser user, string password);
        Task<IdentityResult> RemovePasswordAsync(TUser user);
        Task<bool> HasPasswordAsync(TUser user);
        Task<IdentityResult> AddPasswordAsync(TUser user, string password);
        Task<IdentityResult> ChangePasswordAsync(TUser user, string currentPassword, string newPassword);
        Task<string> GeneratePasswordResetTokenAsync(TUser user);
        Task<IdentityResult> ResetPasswordAsync(TUser user, string token, string newPassword);
        Task<IList<string>> GetRolesAsync(TUser user);
        Task<IdentityResult> AddToRoleAsync(TUser user, string role);
        Task<IdentityResult> RemoveFromRoleAsync(TUser user, string role);
        Task<bool> IsInRoleAsync(TUser user, string role);
        Task<string> GenerateEmailConfirmationTokenAsync(TUser user);
        Task<IdentityResult> ConfirmEmailAsync(TUser user, string token);
        Task<bool> IsEmailConfirmedAsync(TUser user);
        Task<IdentityResult> SetLockoutEnabledAsync(TUser user, bool enabled);
        Task<bool> IsLockedOutAsync(TUser user);
        Task<DateTimeOffset?> GetLockoutEndDateAsync(TUser user);
        Task<IdentityResult> SetLockoutEndDateAsync(TUser user, DateTimeOffset? lockoutEnd);
        Task<int> GetAccessFailedCountAsync(TUser user);
        Task<IdentityResult> ResetAccessFailedCountAsync(TUser user);
        Task<IdentityResult> AccessFailedAsync(TUser user);
        Task<IdentityResult> UpdateSecurityStampAsync(TUser user);
    }
}
