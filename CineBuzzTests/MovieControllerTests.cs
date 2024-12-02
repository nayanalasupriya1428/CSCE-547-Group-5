using CineBuzzApi.Controllers;
using CineBuzzApi.Models;
using CineBuzzApi.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace CineBuzzTests
{
    [TestClass]
    public class MovieControllerTests
    {
        private Mock<IMovieService> _mockMovieService;
        private MoviesController _controller;

        [TestInitialize]
        public void SetUp()
        {
            _mockMovieService = new Mock<IMovieService>();
            _controller = new MoviesController(_mockMovieService.Object);
        }
    }

}
