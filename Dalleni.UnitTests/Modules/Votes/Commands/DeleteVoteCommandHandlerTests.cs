using Dalleni.Application.Features.Votes.Commands.DeleteVote;
using Dalleni.Domin.Enums;
using Dalleni.Domin.Interfaces.Handlers;
using Dalleni.Domin.Interfaces.Repositories;
using Dalleni.Domin.Models;
using Dalleni.Domin.ResponsePattern;
using Microsoft.Extensions.Logging;
using Moq;

namespace Dalleni.UnitTests.Modules.Votes.Commands
{
    public class DeleteVoteHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IVoteRepository> _voteRepositoryMock;
        private readonly Mock<IQuestionRepository> _questionRepositoryMock;
        private readonly Mock<IAnswerRepository> _answerRepositoryMock;
        private readonly Mock<IResponseHandler> _responseHandlerMock;
        private readonly Mock<ILogger<DeleteVoteCommandHandler>> _loggerMock;

        private readonly DeleteVoteCommandHandler _handler;

        public DeleteVoteHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();

            _voteRepositoryMock = new Mock<IVoteRepository>();
            _questionRepositoryMock = new Mock<IQuestionRepository>();
            _answerRepositoryMock = new Mock<IAnswerRepository>();

            _responseHandlerMock = new Mock<IResponseHandler>();
            _loggerMock = new Mock<ILogger<DeleteVoteCommandHandler>>();

            // Connect repositories to UnitOfWork
            _unitOfWorkMock
                .Setup(x => x.Votes)
                .Returns(_voteRepositoryMock.Object);

            _unitOfWorkMock
                .Setup(x => x.Questions)
                .Returns(_questionRepositoryMock.Object);

            _unitOfWorkMock
                .Setup(x => x.Answers)
                .Returns(_answerRepositoryMock.Object);

            _handler = new DeleteVoteCommandHandler(
                _unitOfWorkMock.Object,
                _responseHandlerMock.Object,
                _loggerMock.Object);
        }

        private static Vote CreateQuestionVote(
            Guid? userId = null,
            VoteType type = VoteType.Upvote)
        {
            return Vote.Create(
                userId ?? Guid.NewGuid(),
                type,
                Guid.NewGuid(),
                null);
        }

        private static Vote CreateAnswerVote(
            Guid? userId = null,
            VoteType type = VoteType.Upvote)
        {
            return Vote.Create(
                userId ?? Guid.NewGuid(),
                type,
                null,
                Guid.NewGuid());
        }

        private void SetupVote(Vote vote)
        {
            Console.WriteLine();
            Console.WriteLine("========== SETTING UP VOTE ==========");
            Console.WriteLine($"Vote.Id      : {vote.Id}");
            Console.WriteLine($"Vote.UserId  : {vote.UserId}");
            Console.WriteLine($"QuestionId   : {vote.QuestionId}");
            Console.WriteLine($"AnswerId     : {vote.AnswerId}");
            Console.WriteLine($"Vote.Type    : {vote.Type}");
            Console.WriteLine("Tracking     : true");
            Console.WriteLine("=====================================");
            Console.WriteLine();

            _voteRepositoryMock
                .Setup(x => x.GetByIdAsync(
                    vote.Id,
                    true,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(vote);
        }

        [Fact]
        public async Task Handle_DeleteQuestionVoteSuccessfully_ReturnsSuccess()
        {
            // Arrange
            var userId = Guid.NewGuid();

            var vote = CreateQuestionVote(
                userId,
                VoteType.Upvote);

            var question = new Question();

            SetupVote(vote);

            _questionRepositoryMock
                .Setup(x => x.GetByIdAsync(
                    vote.QuestionId!.Value,
                    true,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(question);
var command = new DeleteVoteCommand(
                UserId: userId,
                voteId: vote.Id);

            _responseHandlerMock
                .Setup(x => x.Success(true, It.IsAny<string>()))
                .Returns(new Response<bool>
                {
                    Succeeded = true,
                    Data = true
                });

            Console.WriteLine("========== DELETE QUESTION VOTE TEST ==========");
            Console.WriteLine($"Expected Vote.Id : {vote.Id}");
            Console.WriteLine($"Expected User.Id : {userId}");
            Console.WriteLine($"Command Vote.Id  : {command.voteId}");
            Console.WriteLine($"Command User.Id  : {command.UserId}");
            Console.WriteLine($"Question.Id     : {vote.QuestionId}");
            Console.WriteLine("===============================================");

            // Act
            var result = await _handler.Handle(
                command,
                CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Succeeded);

            _voteRepositoryMock.Verify(
                x => x.GetByIdAsync(
                    vote.Id,
                    true,
                    It.IsAny<CancellationToken>()),
                Times.Once);

            _questionRepositoryMock.Verify(
                x => x.GetByIdAsync(
                    vote.QuestionId!.Value,
                    true,
                    It.IsAny<CancellationToken>()),
                Times.Once);

            _voteRepositoryMock.Verify(
                x => x.Remove(vote),
                Times.Once);

            _unitOfWorkMock.Verify(
                x => x.SaveChangesAsync(),
                Times.Once);

            _responseHandlerMock.Verify(
                x => x.Success(true, It.IsAny<string>()),
                Times.Once);
        }

        [Fact]
        public async Task Handle_DeleteAnswerVoteSuccessfully_ReturnsSuccess()
        {
            // Arrange
            var userId = Guid.NewGuid();

            var vote = CreateAnswerVote(
                userId,
                VoteType.Upvote);

            var answer = new Answer();

            SetupVote(vote);

            _answerRepositoryMock
                .Setup(x => x.GetByIdAsync(
                    vote.AnswerId!.Value,
                    true,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(answer);

            var command = new DeleteVoteCommand(
                UserId: userId,
                voteId: vote.Id);

            _responseHandlerMock
                .Setup(x => x.Success(true, It.IsAny<string>()))
                .Returns(new Response<bool>
                {
                    Succeeded = true,
                    Data = true
                });

            Console.WriteLine("========== DELETE ANSWER VOTE TEST ==========");
            Console.WriteLine($"Vote.Id      : {vote.Id}");
            Console.WriteLine($"User.Id      : {userId}");
            Console.WriteLine($"Answer.Id    : {vote.AnswerId}");
            Console.WriteLine($"Vote.Type    : {vote.Type}");
            Console.WriteLine("=============================================");

            // Act
            var result = await _handler.Handle(
                command,
                CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Succeeded);

            _voteRepositoryMock.Verify(
                x => x.GetByIdAsync(
                    vote.Id,
                    true,
                    It.IsAny<CancellationToken>()),
                Times.Once);

            _answerRepositoryMock.Verify(
                x => x.GetByIdAsync(
                    vote.AnswerId!.Value,
                    true,
                    It.IsAny<CancellationToken>()),
                Times.Once);

            _voteRepositoryMock.Verify(
                x => x.Remove(vote),
                Times.Once);
_unitOfWorkMock.Verify(
                x => x.SaveChangesAsync(),
                Times.Once);

            _responseHandlerMock.Verify(
                x => x.Success(true, It.IsAny<string>()),
                Times.Once);
        }

        [Fact]
        public async Task Handle_VoteDoesNotExist_ReturnsNotFound()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var voteId = Guid.NewGuid();

            _voteRepositoryMock
                .Setup(x => x.GetByIdAsync(
                    voteId,
                    true,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((Vote?)null);

            var command = new DeleteVoteCommand(
                UserId: userId,
                voteId: voteId);

            _responseHandlerMock
                .Setup(x => x.NotFound<bool>(It.IsAny<string>()))
                .Returns(new Response<bool>
                {
                    Succeeded = false
                });

            Console.WriteLine("========== VOTE NOT FOUND TEST ==========");
            Console.WriteLine($"Expected Vote.Id : {voteId}");
            Console.WriteLine($"Command Vote.Id  : {command.voteId}");
            Console.WriteLine($"Command User.Id  : {command.UserId}");
            Console.WriteLine("=========================================");

            // Act
            var result = await _handler.Handle(
                command,
                CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.Succeeded);

            _voteRepositoryMock.Verify(
                x => x.GetByIdAsync(
                    voteId,
                    true,
                    It.IsAny<CancellationToken>()),
                Times.Once);

            _voteRepositoryMock.Verify(
                x => x.Remove(It.IsAny<Vote>()),
                Times.Never);

            _unitOfWorkMock.Verify(
                x => x.SaveChangesAsync(),
                Times.Never);

            _responseHandlerMock.Verify(
                x => x.NotFound<bool>(It.IsAny<string>()),
                Times.Once);
        }

        [Fact]
        public async Task Handle_UserDoesNotOwnVote_ReturnsUnauthorized()
        {
            // Arrange
            var voteOwnerId = Guid.NewGuid();
            var requesterId = Guid.NewGuid();

            var vote = CreateQuestionVote(
                voteOwnerId,
                VoteType.Upvote);

            SetupVote(vote);

            var command = new DeleteVoteCommand(
                UserId: requesterId,
                voteId: vote.Id);

            _responseHandlerMock
                .Setup(x => x.NotFound<bool>(It.IsAny<string>()))
                .Returns(new Response<bool>
                {
                    Succeeded = false
                });

            Console.WriteLine("========== UNAUTHORIZED TEST ==========");
            Console.WriteLine($"Vote.Id          : {vote.Id}");
            Console.WriteLine($"Vote Owner.Id    : {voteOwnerId}");
            Console.WriteLine($"Requester.Id     : {requesterId}");
            Console.WriteLine($"Command Vote.Id  : {command.voteId}");
            Console.WriteLine($"Command User.Id  : {command.UserId}");
            Console.WriteLine("=======================================");

            // Act
            var result = await _handler.Handle(
                command,
                CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.Succeeded);

            _voteRepositoryMock.Verify(
                x => x.GetByIdAsync(
                    vote.Id,
                    true,
                    It.IsAny<CancellationToken>()),
                Times.Once);

            _voteRepositoryMock.Verify(
                x => x.Remove(It.IsAny<Vote>()),
                Times.Never);

            _unitOfWorkMock.Verify(
                x => x.SaveChangesAsync(),
                Times.Never);
_responseHandlerMock.Verify(
                x => x.NotFound<bool>(It.IsAny<string>()),
                Times.Once);
        }

        [Fact]
        public async Task Handle_UsesTrackedVote()
        {
            // Arrange
            var userId = Guid.NewGuid();

            var vote = CreateQuestionVote(
                userId,
                VoteType.Upvote);

            var question = new Question();

            SetupVote(vote);

            _questionRepositoryMock
                .Setup(x => x.GetByIdAsync(
                    vote.QuestionId!.Value,
                    true,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(question);

            var command = new DeleteVoteCommand(
                UserId: userId,
                voteId: vote.Id);

            _responseHandlerMock
                .Setup(x => x.Success(true, It.IsAny<string>()))
                .Returns(new Response<bool>
                {
                    Succeeded = true,
                    Data = true
                });

            // Act
            await _handler.Handle(
                command,
                CancellationToken.None);

            // Assert
            _voteRepositoryMock.Verify(
                x => x.GetByIdAsync(
                    vote.Id,
                    true,
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task VoteRepositoryMock_ReturnsConfiguredVote()
        {
            // Arrange
            var vote = CreateQuestionVote();

            SetupVote(vote);

            // Act
            var result = await _voteRepositoryMock.Object.GetByIdAsync(
                vote.Id,
                true,
                CancellationToken.None);

            // Assert
            Console.WriteLine();
            Console.WriteLine("========== DIRECT MOCK TEST ==========");
            Console.WriteLine($"Configured Vote.Id : {vote.Id}");
            Console.WriteLine($"Configured User.Id : {vote.UserId}");
            Console.WriteLine($"Question.Id        : {vote.QuestionId}");
            Console.WriteLine($"Result is null     : {result is null}");

            if (result != null)
            {
                Console.WriteLine($"Result Vote.Id     : {result.Id}");
                Console.WriteLine($"Result User.Id     : {result.UserId}");
            }

            Console.WriteLine("======================================");
            Console.WriteLine();

            Assert.NotNull(result);
            Assert.Equal(vote.Id, result.Id);
            Assert.Equal(vote.UserId, result.UserId);
            Assert.Equal(vote.QuestionId, result.QuestionId);
        }
    }
}