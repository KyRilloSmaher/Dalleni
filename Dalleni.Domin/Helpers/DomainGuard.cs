using Dalleni.Domin.Exceptions;

namespace Dalleni.Domin.Helpers
{
    /// <summary>
    /// Centralized domain validation helper that throws the project's custom exceptions.
    /// </summary>
    public static class DomainGuard
    {
        public static string AgainstNullOrWhiteSpace(string? value, string parameterName, string? message = null)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new BadRequestException(message ?? $"{parameterName} is required.");
            }

            return value.Trim();
        }

        public static Guid AgainstEmpty(Guid value, string parameterName, string? message = null)
        {
            if (value == Guid.Empty)
            {
                throw new BadRequestException(message ?? $"{parameterName} is required.");
            }

            return value;
        }

        public static decimal? AgainstNegative(decimal? value, string parameterName, string? message = null)
        {
            if (value.HasValue && value.Value < 0)
            {
                throw new BadRequestException(message ?? $"{parameterName} cannot be negative.");
            }

            return value;
        }

        public static int AgainstNegative(int value, string parameterName, string? message = null)
        {
            if (value < 0)
            {
                throw new BadRequestException(message ?? $"{parameterName} cannot be negative.");
            }

            return value;
        }

        public static double AgainstNegative(double value, string parameterName, string? message = null)
        {
            if (value < 0)
            {
                throw new BadRequestException(message ?? $"{parameterName} cannot be negative.");
            }

            return value;
        }

        public static DateTime AgainstPast(DateTime value, string parameterName, string? message = null)
        {
            if (value <= DateTime.UtcNow)
            {
                throw new BadRequestException(message ?? $"{parameterName} must be in the future.");
            }

            return value;
        }
    }
}
