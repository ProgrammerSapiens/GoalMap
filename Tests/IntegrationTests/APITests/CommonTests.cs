namespace Tests.IntegrationTests.APITests
{
    public class CommonTests
    {
        #region Authorization tests

        [Fact]
        public async Task Authorization_ShouldReturnUnauthorized_WhenTokenIsMissing()
        {

        }

        public async Task Authorization_ShouldReturnForbidden_WhenTryingToAccessAnotherUserData()
        {

        }

        #endregion

        #region ErrorHandling tests

        [Fact]
        public async Task ErrorHandling_ShouldReturnBadRequest_WhenInvalidDataIsProvided()
        {

        }

        [Fact]
        public async Task ErrorHandling_ShouldReturnNotFound_WhenIdNotFound()
        {

        }

        [Fact]
        public async Task ErrorHandling_ShouldReturnInternalServirError_WhenExceptionOccurs()
        {

        }

        #endregion
    }
}
