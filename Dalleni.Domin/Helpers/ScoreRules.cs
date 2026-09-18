
namespace Dalleni.Domin.Helpers
{
    /// <summary>
    /// Centralized scoring rules for questions.
    /// Modify the values here to change the scoring system globally.
    /// </summary>
    public static class ScoreRules
    {
        /// <summary>
        /// Score added when a question receives an upvote.
        /// </summary>
        public const double Upvote = 1.5;

        /// <summary>
        /// Score removed when a question receives a downvote.
        /// </summary>
        public const double Downvote = 1.0;

        /// <summary>
        /// Score added when a question receives a view.
        /// </summary>
        public const double View = 0.01;

        /// <summary>
        /// Score added when an answer is added.
        /// </summary>
        public const double AnswerAdded = 2.0;

        /// <summary>
        /// Score removed when an answer is removed.
        /// </summary>
        public const double AnswerRemoved = 2.0;

        /// <summary>
        /// Score added when an answer is accepted.
        /// </summary>
        public const double AcceptedAnswer = 20.0;

        /// <summary>
        /// Score removed when an answer is unaccepted.
        /// </summary>
        public const double UnacceptedAnswer = 20.0;
        /// <summary>
        /// Score added when a Answer is marked as successful.
        /// </summary>
        public const double SuccessfulAnswer = 10.0;
        /// <summary>
        /// Score removed when a Answer is marked as unsuccessful.
        /// </summary>
        public const double UnsuccessfulAnswer = 10.0;
    }
}
