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

            public const string Delete = Prefix + "delete/"+SingleRoute;
            public const string Restore = Prefix + "restore";
            
            public const string GetTopUsers = Prefix + "top-users";
            public const string GetTopContributors = Prefix + "top-contributors";
            public const string GetStats = Prefix + "stats/" + SingleRoute;
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
            public const string GetByTag = Prefix + "tag/" + SingleRoute;
            public const string Search = Prefix + "search";
            public const string Related = Prefix + SingleRoute +"/related";
            public const string Similars = Prefix +"similars";
            public const string GetByUser = Prefix + "user/" + SingleRoute;
            public const string Close = Prefix + "close/" + SingleRoute;
            public const string Reopen = Prefix + "reopen/" + SingleRoute;
            public const string AcceptAnswer = Prefix + "accept-answer/" + SingleRoute; // id is answerId
        }
        #endregion

        #region Answer Endpoints
        public static class Answers
        {
            private const string Prefix = Root + "/answers/";
            public const string Create = Root + "/answers";
            public const string Update = Prefix + SingleRoute;
            public const string Delete = Prefix + SingleRoute;
            public const string GetByQuestionId = Prefix + "question/" + SingleRoute;
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
            public const string GetAll = Prefix + "by-user-Id";
            public const string Create = Prefix + "add";
            public const string Remove = Prefix + "remove/" + SingleRoute;
        }
        #endregion
    }
}
