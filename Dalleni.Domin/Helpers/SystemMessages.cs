using System;
using System.Collections.Generic;
using System.Text;

namespace Dalleni.Domin.Helpers
{
    public static class SystemMessages
    {
        // =====================
        // ✅ General Success
        // =====================
        public const string SUCCESS = "Operation completed successfully.";
        public const string OPERATION_SUCCESSFUL = "The operation was successful.";
        public const string DATA_RETRIEVED = "Data retrieved successfully.";
        public const string RECORD_ADDED = "Record added successfully.";
        public const string RECORD_UPDATED = "Record updated successfully.";
        public const string RECORD_DELETED = "Record deleted successfully.";
        public const string SETTINGS_SAVED = "Settings saved successfully.";
        public const string ALREADY_ACTIVE = "Already Active.";
        // =====================
        // ⚠️ Common Validation / Errors
        // =====================
        public const string ERROR = "An unexpected error occurred. Please try again.";
        public const string FAILED = "The operation failed. Please try again.";
        public const string INVALID_INPUT = "Invalid input. Please check the provided data.";
        public const string RECORD_NOT_FOUND = "The requested record was not found.";
        public const string DUPLICATE_RECORD = "A record with similar data already exists.";
        public const string REQUIRED_FIELDS_MISSING = "Some required fields are missing.";
        public const string INVALID_TOKEN = "Invalid or expired token.";
        public const string DATABASE_ERROR = "A database error occurred.";
        public const string NETWORK_ERROR = "A network error occurred. Please try again later.";
        public const string SERVER_ERROR = "Internal server error.";
        public const string BAD_REQUEST = "Bad request. Please verify the input data.";
        public const string NULL_PARAMETER = "Null Parameter to the Function";
        public const string NOT_FOUND = "The requested resource was not found.";
        public const string INVALID_PAGINATION_PARAMETERS = " Page Number and Page Size Should be Greater than 0";
        public const string NO_DATA_FOUND = "No data found.";
        public const string INVALID_DATE_RANGE = "INVALID DATE RANGE";
        public const string OPERATION_TIMEOUT = "Time Out !";
        public const string RESERVATION_INVALID = "RESERVATION INVALID";
        // =====================
        // 👤 User & Auth
        // =====================
        public const string USER_CREATED = "User account created successfully.";
        public const string USER_UPDATED = "User account updated successfully.";
        public const string USER_DELETED = "User account deleted successfully.";
        public const string USER_VERIFIED = "User verified successfully.";
        public const string USER_NOT_FOUND = "User not found.";
        public const string UNAUTHORIZED = "Invalid or expired refresh token";
        public const string EMAIL_ALREADY_EXISTS = "The email address is already registered.";
        public const string USERNAME_ALREADY_EXISTS = "The username is already taken.";
        public const string PHONE_ALREADY_EXISTS = "The phone number is already registered.";
        public const string PASSWORD_CHANGED = "Password changed successfully.";
        public const string PASSWORD_RESET_SUCCESS = "Password reset successfully.";
        public const string INVALID_CREDENTIALS = "Invalid username or password.";
        public const string ACCOUNT_LOCKED = "Your account is locked. Please contact support.";
        public const string TOKEN_EXPIRED = "Your session has expired. Please log in again.";
        public const string TOKEN_GENERATED = "New Refresh Token has been generated";
        public const string LOGIN_SUCCESS = "Login successful.";
        public const string LOGOUT_SUCCESS = "Logout successful.";
        public const string ACCESS_DENIED = "Access denied. You do not have permission to perform this action.";
        public const string GENERATING_CODE_FAILED = "Failed To Generate Reset Code";
        public const string RESET_PASSWORD_CODE_SENT = "Reset Code Sent To Your Email Successfully";
        public const string INVALID_RESET_CODE = "The reset code is invalid or has expired.";
        public const string PASSWORD_RESET_CODE_CONFIRMED = "Reset Password Code confirmed successfully.";
        public const string ADDRESS_IS_REQUIRED = "Address is Required ";
        public const string ADDRESS_ADDED = "Address added to client";
        public const string ADDRESS_NOT_FOUND = "Address not found";
        public const string PASSWORD_CHANGED_SUCCESS = "Password changed successfully";
        public const string PASSWORD_CHANGE_FAILED = "Failed to change password";
        public const string RESET_CODE_EXPIRED = "Reset code has expired";
        public const string NO_RESET_CODE = "No reset code found";
        public const string MAX_ATTEMPTS_REACHED = "Maximum attempts reached. Please request a new code";
        public const string RESET_NOT_CONFIRMED = "Please confirm your reset code first";
        public const string PASSWORD_RESET_RATE_LIMIT = "Please wait {0} seconds before requesting another code";
        public const string REFRESH_TOKEN_EXPIRED = "Refresh token Has been Expired";
        public const string INVALID_REFRESH_TOKEN = "Invalid Refresh token ";
        public const string TOKEN_REFRESHED_SUCCESS = " Refresh token refreshed ";
        public const string YOU_CAN_RESTORE_YOUR_ACCOUNT = "An account with this email already exists but is deleted. You can restore your account instead of creating a new one if You Know the associated PhoneNumber..";

        // =====================
        // 📧 Email Operations
        // =====================
        public const string EMAIL_SENT = "Email sent successfully.";
        public const string EMAIL_NOT_SENT = "Failed to send email. Please try again later.";
        public const string VERIFICATION_SUCCESS = "Verification completed successfully.";
        public const string VERIFICATION_FAILED = "Verification failed. Please check the provided information.";
        public const string EMAIL_ALREADY_VERIFIED = "Email is already verified.";
        public const string EMAIL_NOT_CONFIRMED = "Please confirm your email before continuing.";
        public const string EMAIL_VERIFICATION_LINK_EXPIRED = "This Email VERIFICATION Link Has been Expired , Try order New One.";
        // =====================
        // 📁 File Upload / Media
        // =====================
        public const string FILE_UPLOADED = "File uploaded successfully.";
        public const string FILE_UPLOAD_FAILED = "File upload failed. Please try again.";
        public const string INVALID_FILE_TYPE = "Invalid file type. Please upload a supported file format.";
        public const string FILE_TOO_LARGE = "The uploaded file is too large. Please upload a smaller file.";
        public const string FILE_NOT_FOUND = "Requested file was not found.";

    }
}
