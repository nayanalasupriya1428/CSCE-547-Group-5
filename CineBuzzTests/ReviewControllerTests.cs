using CineBuzzAPI.Controllers;
using CineBuzzApi.Models;
using CineBuzzApi.Services;
using CineBuzzAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;

namespace CineBuzzAPI.Tests.Controllers
{
    [TestClass]
    public class ReviewControllerTests
    {
        private Mock<IReviewService> _mockReviewService;
        private ReviewController _reviewController;

        [TestInitialize]
        public void TestInitialize()
        {
            // Create mock for IReviewService
            _mockReviewService = new Mock<IReviewService>();

            // Instantiate the ReviewController with the mocked IReviewService
            _reviewController = new ReviewController(_mockReviewService.Object);
        }

    }
}
