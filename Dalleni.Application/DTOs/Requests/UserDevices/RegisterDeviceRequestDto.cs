using Dalleni.Domin.Enums;

namespace Dalleni.Application.DTOs.Requests.UserDevices
{
    public class RegisterDeviceRequestDto
    {
        public string DeviceToken { get; set; } = null!;

        public DevicePlatform Platform { get; set; }
    }
}