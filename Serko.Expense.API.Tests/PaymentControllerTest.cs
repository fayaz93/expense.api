using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;
using Moq;
using Serko.Expense.API.Controllers;
using Serko.Expense.API.Interfaces;
using Serko.Expense.Contracts;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Xunit;

namespace Serko.Expense.API.Tests
{
    public class PaymentControllerTest : IClassFixture<TestFixture<Startup>>
    {
        private readonly HttpClient client;

        public PaymentControllerTest(TestFixture<Startup> fixture)
        {
            this.client = fixture.Client;
        }

        [Fact]
        public async Task Payment_Expense_InValidUrl_ReturnHelpText()
        {
            var request = new
            {
                Url = "/api/v1/payment/nonexisting"
            };

            var response = await client.GetAsync(request.Url);
            var value = await response.Content.ReadAsStringAsync();

            response.EnsureSuccessStatusCode();
            Assert.Contains("Following are available Actions", value);
        }

        [Fact]
        public async Task Payment_Expense_Authentication_NoApiKey_ReturnsUnauthorized()
        {
            var request = new
            {
                Url = "/api/v1/payment/expense/import",
                Body = new
                {
                    text = "Hello"
                }
            };

            var response = await client.PostAsync(request.Url, ContentHelper.GetStringContent(request.Body));
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task Payment_Expense_Authentication_WithApiKey_ReturnsSuccess()
        {
            var request = CreateRequest(string.Empty);
            SetAuthHeader();


            var response = await client.PostAsync(request.Url, ContentHelper.GetStringContent(request.Body));
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Payment_Expense_ReturnsResponseAwlays()
        {
            var request = CreateRequest(string.Empty);
            SetAuthHeader();

            var response = await client.PostAsync(request.Url, ContentHelper.GetStringContent(request.Body));
            var value = await response.Content.ReadAsAsync<ExpenseResponse>();

            response.EnsureSuccessStatusCode();
            Assert.True(typeof(ExpenseResponse) == value.GetType());
            Assert.True(!string.IsNullOrEmpty(value.ExecutionResult.Status));
        }

        #region Private Methods
        private (string Url, ExpenseRequest Body) CreateRequest(string rawText)
        {
            var ExpenseRequest = new ExpenseRequest
            {
                RawText = ""
            };

            var request = new
            {
                Url = "/api/v1/payment/expense/import",
                Body = ExpenseRequest
            };

            return (request.Url, request.Body);
        }

        private void SetAuthHeader()
        {
            client.DefaultRequestHeaders.Add(Library.Constants.API_KEY_HEADER_NAME, "26F8C32804324AF5BC7C034D5E31DD5C");
        }
        #endregion
    }
}
