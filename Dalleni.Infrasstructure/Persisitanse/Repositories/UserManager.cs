using Dalleni.Domin.Interfaces.Repositories;
using Dalleni.Domin.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Dalleni.Infrastructure.Persisitanse.Repositories
{
    public class UserManager : IUserManager<ApplicationUser>
    {
        private readonly Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> _userManager;

        public UserManager(Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        public async Task<ApplicationUser?> FindByIdAsync(Guid userId, bool trackChanges = false)
            => trackChanges
                ? await _userManager.Users.SingleOrDefaultAsync(u => u.Id == userId)
                : await _userManager.Users.AsNoTracking().SingleOrDefaultAsync(u => u.Id == userId);

        public Task<ApplicationUser?> FindByIdAsync(string userId, bool trackChanges = false)
            => Guid.TryParse(userId, out var parsedId)
                ? FindByIdAsync(parsedId, trackChanges)
                : Task.FromResult<ApplicationUser?>(null);

        public async Task<ApplicationUser?> FindByEmailAsync(string email, bool trackChanges = false)
            => trackChanges
                ? await _userManager.Users.SingleOrDefaultAsync(u => u.Email == email)
                : await _userManager.Users.AsNoTracking().SingleOrDefaultAsync(u => u.Email == email);

        public async Task<ApplicationUser?> FindByNameAsync(string userName, bool trackChanges = false)
            => trackChanges
                ? await _userManager.Users.SingleOrDefaultAsync(u => u.UserName == userName)
                : await _userManager.Users.AsNoTracking().SingleOrDefaultAsync(u => u.UserName == userName);

        public async Task<IEnumerable<ApplicationUser>> GetAllUsersAsync()
            => await _userManager.Users.ToListAsync();

        public async Task<IEnumerable<ApplicationUser>> GetAllAdminsAsync()
            => await _userManager.GetUsersInRoleAsync("Admin");

        public Task<IdentityResult> CreateAsync(ApplicationUser user, string password)
            => _userManager.CreateAsync(user, password);

        public Task<IdentityResult> UpdateAsync(ApplicationUser user)
            => _userManager.UpdateAsync(user);

        public Task<IdentityResult> DeleteAsync(ApplicationUser user)
            => _userManager.DeleteAsync(user);

        public async Task<bool> IsEmailUniqueAsync(string email)
            => await _userManager.FindByEmailAsync(email) is null;

        public async Task<bool> IsUserNameUniqueAsync(string userName)
            => await _userManager.FindByNameAsync(userName) is null;

        public Task<bool> CheckPasswordAsync(ApplicationUser user, string password)
            => _userManager.CheckPasswordAsync(user, password);

        public Task<IdentityResult> RemovePasswordAsync(ApplicationUser user)
            => _userManager.RemovePasswordAsync(user);

        public Task<bool> HasPasswordAsync(ApplicationUser user)
            => _userManager.HasPasswordAsync(user);

        public Task<IdentityResult> AddPasswordAsync(ApplicationUser user, string password)
            => _userManager.AddPasswordAsync(user, password);

        public Task<IdentityResult> ChangePasswordAsync(ApplicationUser user, string currentPassword, string newPassword)
            => _userManager.ChangePasswordAsync(user, currentPassword, newPassword);

        public Task<string> GeneratePasswordResetTokenAsync(ApplicationUser user)
            => _userManager.GeneratePasswordResetTokenAsync(user);

        public Task<IdentityResult> ResetPasswordAsync(ApplicationUser user, string token, string newPassword)
            => _userManager.ResetPasswordAsync(user, token, newPassword);

        public Task<IList<string>> GetRolesAsync(ApplicationUser user)
            => _userManager.GetRolesAsync(user);

        public Task<IdentityResult> AddToRoleAsync(ApplicationUser user, string role)
            => _userManager.AddToRoleAsync(user, role);

        public Task<IdentityResult> RemoveFromRoleAsync(ApplicationUser user, string role)
            => _userManager.RemoveFromRoleAsync(user, role);

        public Task<bool> IsInRoleAsync(ApplicationUser user, string role)
            => _userManager.IsInRoleAsync(user, role);

        public Task<string> GenerateEmailConfirmationTokenAsync(ApplicationUser user)
            => _userManager.GenerateEmailConfirmationTokenAsync(user);

        public Task<IdentityResult> ConfirmEmailAsync(ApplicationUser user, string token)
            => _userManager.ConfirmEmailAsync(user, token);

        public Task<bool> IsEmailConfirmedAsync(ApplicationUser user)
            => _userManager.IsEmailConfirmedAsync(user);

        public Task<IdentityResult> SetLockoutEnabledAsync(ApplicationUser user, bool enabled)
            => _userManager.SetLockoutEnabledAsync(user, enabled);

        public Task<bool> IsLockedOutAsync(ApplicationUser user)
            => _userManager.IsLockedOutAsync(user);

        public Task<DateTimeOffset?> GetLockoutEndDateAsync(ApplicationUser user)
            => _userManager.GetLockoutEndDateAsync(user);

        public Task<IdentityResult> SetLockoutEndDateAsync(ApplicationUser user, DateTimeOffset? lockoutEnd)
            => _userManager.SetLockoutEndDateAsync(user, lockoutEnd);

        public Task<int> GetAccessFailedCountAsync(ApplicationUser user)
            => _userManager.GetAccessFailedCountAsync(user);

        public Task<IdentityResult> ResetAccessFailedCountAsync(ApplicationUser user)
            => _userManager.ResetAccessFailedCountAsync(user);

        public Task<IdentityResult> AccessFailedAsync(ApplicationUser user)
            => _userManager.AccessFailedAsync(user);

        public Task<IdentityResult> UpdateSecurityStampAsync(ApplicationUser user)
            => _userManager.UpdateSecurityStampAsync(user);
    }
}
