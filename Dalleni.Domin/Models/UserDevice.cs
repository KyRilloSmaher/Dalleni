using Dalleni.Domin.Enums;
using Dalleni.Domin.Models.Base;

namespace Dalleni.Domin.Models
{
    public class UserDevice: DomainEntity
    {
        private UserDevice()
        {
        }

        private UserDevice(
            Guid userId,
            string deviceToken,
            DevicePlatform platform)
        {
            Id = Guid.NewGuid();

            UserId = userId;
            DeviceToken = deviceToken;
            Platform = platform;

            IsActive = true;

            CreatedAt = DateTime.UtcNow;
            LastUsedAt = DateTime.UtcNow;
        }

        public Guid Id { get; private set; }

        /// <summary>
        /// The user who owns this device.
        /// </summary>
        public Guid UserId { get; private set; }

        /// <summary>
        /// The push notification token associated with this device.
        /// </summary>
        public string DeviceToken { get; private set; } = null!;

        /// <summary>
        /// The platform on which the device is registered.
        /// </summary>
        public DevicePlatform Platform { get; private set; }

        public bool IsActive { get; private set; }


        public DateTime LastUsedAt { get; private set; }

        public static UserDevice Create(
            Guid userId,
            string deviceToken,
            DevicePlatform platform)
        {
            Validate(
                userId,
                deviceToken);

            return new UserDevice(
                userId,
                deviceToken,
                platform);
        }

        public void UpdateToken(string deviceToken)
        {
            if (string.IsNullOrWhiteSpace(deviceToken))
            {
                throw new ArgumentException(
                    "Device token is required.",
                    nameof(deviceToken));
            }

            DeviceToken = deviceToken;
            LastUsedAt = DateTime.UtcNow;
            IsActive = true;
        }

        public void UpdateLastUsed()
        {
            LastUsedAt = DateTime.UtcNow;
        }

        public void MarkAsActive()
        {
            if (IsActive)
            {
                return;
            }

            IsActive = true;
            LastUsedAt = DateTime.UtcNow;
        }

        public void MarkAsInactive()
        {
            if (!IsActive)
            {
                return;
            }

            IsActive = false;
        }

        private static void Validate(
            Guid userId,
            string deviceToken)
        {
            if (userId == Guid.Empty)
            {
                throw new ArgumentException(
                    "User identifier cannot be empty.",
                    nameof(userId));
            }

            if (string.IsNullOrWhiteSpace(deviceToken))
            {
                throw new ArgumentException(
                    "Device token is required.",
                    nameof(deviceToken));
            }
        }
    }
}