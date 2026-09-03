using System;
using System.Collections.Generic;
using System.Text;

namespace Dalleni.Domin.Helpers
{
    public static class APIROUTES
    {
        public const string Root = "api";
        public const string SingleRoute = "{id:guid}";

        #region Authentication Endpoints
        public static class Authentication
        {
            private const string Prefix = Root + "/auth/";
            public const string Login = Prefix + "login";
            public const string googleSignup = Prefix + "google-signin";
            public const string GoogleLogin = Prefix + "google-login";
            public const string SignUp = Prefix + "sign-up";
            public const string ConfirmEmail = Prefix + "confirm-email";
            public const string ResendConfirmationEmail = Prefix + "resend-confirmation-email";
            public const string ForgotPassword = Prefix + "forgot-password";
            public const string ConfirmResetPasswordCode = Prefix + "confirm-reset-password-code";
            public const string ChangePassword = Prefix + "change-password";
            public const string SendResetCode = Prefix + "send-reset-code";
            public const string ResendResetCode = Prefix + "resend-reset-code";
            public const string ResetPassword = Prefix + "reset-password";
            public const string RefreshToken = Prefix + "refresh-token";
            public const string Logout = Prefix + "logout";
        }
        #endregion

        #region User Endpoints
        public static class User
        {
            private const string Prefix = Root + "/users/";

            public const string GetAll = Root + "/users";
            public const string GetById = Prefix + SingleRoute;

            public const string GetCurrentUser = Prefix + "me";
            public const string GetByEmail = Prefix + "by-email/{email}";
            public const string Search = Prefix + "search";

            public const string UpdateProfile = Prefix + "update-profile";
            public const string UpdateProfileImage = Prefix + "update-profile-image";

            // ✅ FIXED: Removed trailing slash
            public const string Delete = Prefix + SingleRoute + "/delete";
            public const string Restore = Prefix + "restore";
            
            public const string GetTopUsers = Prefix + "top-users";
            public const string GetTopContributors = Prefix + "top-contributors";
            // ✅ FIXED: Removed trailing slash
            public const string GetStats = Prefix + SingleRoute + "/stats";
        }
        #endregion

        #region Question Endpoints
        public static class Questions
        {
            private const string Prefix = Root + "/questions/";
            public const string Create = Root + "/questions";
            public const string Update = Prefix + SingleRoute;
            public const string Delete = Prefix + SingleRoute;
            public const string GetById = Prefix + SingleRoute;
            public const string GetAllPaged = Root + "/questions";
            public const string GetByCategory = Prefix + "category/" + SingleRoute;
            public const string GetByTag = Prefix + SingleRoute + "/tags";
            public const string Search = Prefix + "search";
            public const string Related = Prefix + SingleRoute + "/related";
            public const string Similars = Prefix + "similars";
            public const string GetByUser = Root + "/user/" + SingleRoute + "/questions";
            public const string Close = Prefix + SingleRoute + "/close";
            public const string Reopen = Prefix + SingleRoute + "/reopen";
            public const string AcceptAnswer = Prefix + SingleRoute + "/accept-answer";
        }
        #endregion

        #region Answer Endpoints
        public static class Answers
        {
            private const string Prefix = Root + "/answers/";
            public const string Create = Root + "/answers/create";
            public const string Update = Prefix + SingleRoute + "/update";
            public const string Delete = Prefix + SingleRoute + "/delete";
            public const string GetById = Prefix + SingleRoute;
            public const string GetByQuestionId = Prefix + "question/" + SingleRoute + "/answers";
            public const string GetByUser = Root + "/user/" + SingleRoute + "/answers";

            public const string MarkAsSuccessful = Prefix + SingleRoute + "/mark-as-successful";
            public const string UnmarkAsSuccessful = Prefix + SingleRoute + "/unmark-as-successful";
            public const string AcceptAnswer = Prefix + SingleRoute + "/accept-answer";
            public const string UnacceptAnswer = Prefix + SingleRoute + "/unaccept-answer";
            public const string GetAcceptedAnswer = Prefix + "question/" + SingleRoute + "/accepted-answer";
        }
        #endregion

        #region Tag Endpoints
        public static class Tags
        {
            private const string Prefix = Root + "/tags/";
            public const string GetAll = Root + "/tags";
            public const string GetBySlug = Prefix + "{slug}";
            public const string Search = Prefix + "search";
        }
        #endregion

        #region Category Endpoints
        public static class Categories
        {
            private const string Prefix = Root + "/categories/";
            public const string GetAll = Root + "/categories";
            public const string GetById = Prefix + SingleRoute;
        }
        #endregion

        #region Voting Endpoints
        public static class Votes
        {
            private const string Prefix = Root + "/votes/";
            public const string VoteQuestion = Prefix + "question/" + SingleRoute;
            public const string VoteAnswer = Prefix + "answer/" + SingleRoute;
            public const string RemoveVote = Prefix + "remove/" + SingleRoute;
        }
        #endregion

        #region SavedQuestions Endpoints
        public static class savedQuestions
        {
            private const string Prefix = Root + "/user/saved-questions/";
            public const string GetAll = Prefix;
            public const string Create = Prefix + "add";
            public const string Remove = Prefix + SingleRoute + "/remove";
        }
        #endregion
    
        #region OfficialEntities Endpoints
        public static class OfficialEntities
        {
            private const string Prefix = Root + "/official-entities/";
            private const string AdminPrefix = Root + "/admin/official-entities/";
            public const string GetAll = Root + "/official-entities";
            public const string GetById = Prefix + SingleRoute;
            public const string Search = Prefix + "search";
            public const string Create = AdminPrefix + "create";
            public const string Update = Prefix + SingleRoute;
            public const string Verify = Prefix + SingleRoute + "/verify";
            public const string Delete = Prefix + SingleRoute;
            public const string Restore = Prefix + SingleRoute + "/restore";
            public const string GetServices = Prefix + SingleRoute + "/services";
            public const string GetVerifiedEntities = Prefix + "verified";
            public const string GetMyEntities = Prefix + "my-entities";
            public const string GetStats = Prefix + SingleRoute + "/stats";
        }
        #endregion
        
        #region OfficialEntityMembers Endpoints
        public static class OfficialEntityMembers
        {
            private const string Prefix = Root + "/official-entity-members/";
            private const string AdminPrefix = Root + "/admin/official-entity/";
            public const string GetAll = Root + "/official-entity-members";
            public const string GetById = Prefix + SingleRoute;
            public const string GetByOfficialEntityId = Prefix + "official-entity/" + SingleRoute + "/members";
            public const string CreateOwner = AdminPrefix +SingleRoute +"/create-owner";
            public const string Update = Prefix + SingleRoute;
            public const string Delete = Prefix + SingleRoute;
            public const string Restore = Prefix + SingleRoute + "/restore";
            public const string Activate = Prefix + SingleRoute + "/activate";
            public const string Deactivate = Prefix + SingleRoute + "/deactivate";
            public  const string inviteMember = Prefix + SingleRoute + "/invite";
            public const string AcceptInvitation = Prefix + "invitations/accept";
        }
        #endregion
        #region Service Endpoints
        public static class Services
        {
            private const string Prefix = Root + "/services/";
            public const string GetAll = Root + "/services";
            public const string GetById = Prefix + SingleRoute;
            public const string Search = Prefix + "search";
            public const string Create = Prefix + "create";
            public const string Update = Prefix + SingleRoute;
            public const string Delete = Prefix + SingleRoute;
            public const string Restore = Prefix + SingleRoute + "/restore";
            public const string ToggleAvailability = Prefix + "{id}/toggle-availability";
            public const string GetByCategory = Prefix + "category/{categoryId}";
            public const string GetByOfficialEntity = Prefix + "official-entity/{officialEntityId}";
        }
        #endregion
    
        #region Rating Endpoints   
        public static class Ratings
        {
            private const string Prefix = Root + "/ratings/";
            public const string GetAll = Root + "/ratings";
            public const string GetById = Prefix + SingleRoute;
            public const string GetByServiceId = Root + "/service/" + SingleRoute + "/ratings";
            public const string GetMyRatings = Prefix + "user-ratings";
            public const string GetMyRatingForService = Prefix + "service/" + SingleRoute + "/user-rate";
            public const string Create = Root + "/ratings/create";
            public const string Update = Prefix + SingleRoute + "/update";
            public const string Delete = Prefix + SingleRoute + "/delete";
            public const string Restore = Prefix + SingleRoute + "/restore";
        }
        #endregion
    }
}