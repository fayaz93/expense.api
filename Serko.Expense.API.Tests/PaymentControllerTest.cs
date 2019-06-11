using Serko.Expense.Contracts;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
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
            //If URL is not found, application returns help text.

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
            //Auth Key is not sent in Http Header, Unauthorized

            var request = CreateRequest(string.Empty);
            RemoveAuthHeader();

            var response = await client.PostAsync(request.Url, ContentHelper.GetStringContent(request.Body));
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task Payment_Expense_Authentication_WithApiKey_ReturnsSuccess()
        {
            //Authenticated request should always return OK

            var request = CreateRequest(string.Empty);
            SetAuthHeader();


            var response = await client.PostAsync(request.Url, ContentHelper.GetStringContent(request.Body));
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Payment_Expense_ReturnsResponseAwlays()
        {
            //Authenticated request should always ensure response even in falure/exception case

            var request = CreateRequest(string.Empty);
            SetAuthHeader();

            var response = await client.PostAsync(request.Url, ContentHelper.GetStringContent(request.Body));
            var value = await response.Content.ReadAsAsync<ExpenseResponse>();
            response.EnsureSuccessStatusCode();

            Assert.True(typeof(ExpenseResponse) == value.GetType());
            Assert.True(!string.IsNullOrEmpty(value.ExecutionResult.Status));
        }

        [Fact]
        public async Task Payment_Expense_NoXmlTags_ReturnsValidationError()
        {
            //No xml in raw text, validation error

            var text = @"Hi Yvaine, Please create an expense claim for the below.";

            var request = CreateRequest(text);
            SetAuthHeader();

            var response = await client.PostAsync(request.Url, ContentHelper.GetStringContent(request.Body));
            var value = await response.Content.ReadAsAsync<ExpenseResponse>();
            response.EnsureSuccessStatusCode();

            Assert.Equal("Failed", value.ExecutionResult.Status);
            Assert.True(value.ExecutionResult.Errors.Any());
        }

        [Fact]
        public async Task Payment_Expense_InvalidXml_ReturnsValidationError()
        {
            //2 cost_centre elements, one does not have a closing tag, validation error

            var text = @"Hi Yvaine, Please create an expense claim for the below. <expense><cost_centre><cost_centre>DEV002</cost_centre></expense>.";

            var request = CreateRequest(text);
            SetAuthHeader();

            var response = await client.PostAsync(request.Url, ContentHelper.GetStringContent(request.Body));
            var value = await response.Content.ReadAsAsync<ExpenseResponse>();
            response.EnsureSuccessStatusCode();

            Assert.Equal("Failed", value.ExecutionResult.Status);
            Assert.Contains(value.ExecutionResult.Errors, e => e.Contains("Invalid Xml in text"));
        }

        [Fact]
        public async Task Payment_Expense_MissingTotal_ReturnsValidationError()
        {
            //Valid Xml, return response

            var text = @"Hi Yvaine,

                        Please create an expense claim for the below. Relevant details are marked up as requested…

                        <expense><cost_centre>DEV002</cost_centre><payment_method>personal card</payment_method> </expense>
                        
                        From: Ivan Castle 
                        Sent: Friday, 16 February 2018 10:32 AM 
                        To: Antoine Lloyd <Antoine.Lloyd@example.com> 
                        Subject: test

                        Hi Antoine,
                        
                        Please create a reservation at the <vendor>Viaduct Steakhouse</vendor> our <description>development team’s project end celebration dinner</description> on <date>Tuesday 27 April 2017</date>. We expect to arrive around 7.15pm. Approximately 12 people but I’ll confirm exact numbers closer to the day.
                        
                        Regards,
                        Ivan";

            var request = CreateRequest(text);
            SetAuthHeader();

            var response = await client.PostAsync(request.Url, ContentHelper.GetStringContent(request.Body));
            var value = await response.Content.ReadAsAsync<ExpenseResponse>();
            response.EnsureSuccessStatusCode();

            Assert.Equal("Failed", value.ExecutionResult.Status);
            Assert.Contains(value.ExecutionResult.Errors, e => e.Contains("does not have a total"));
        }

        [Fact]
        public async Task Payment_Expense_ValidInput_ReturnsResponse()
        {
            //Valid Xml, return response

            var text = @"Hi Yvaine,

                        Please create an expense claim for the below. Relevant details are marked up as requested…

                        <expense><cost_centre>DEV002</cost_centre> <total>1024.01</total><payment_method>personal card</payment_method> </expense>
                        
                        From: Ivan Castle 
                        Sent: Friday, 16 February 2018 10:32 AM 
                        To: Antoine Lloyd <Antoine.Lloyd@example.com> 
                        Subject: test

                        Hi Antoine,
                        
                        Please create a reservation at the <vendor>Viaduct Steakhouse</vendor> our <description>development team’s project end celebration dinner</description> on <date>Tuesday 27 April 2017</date>. We expect to arrive around 7.15pm. Approximately 12 people but I’ll confirm exact numbers closer to the day.
                        
                        Regards,
                        Ivan";

            var request = CreateRequest(text);
            SetAuthHeader();

            var response = await client.PostAsync(request.Url, ContentHelper.GetStringContent(request.Body));
            var value = await response.Content.ReadAsAsync<ExpenseResponse>();
            response.EnsureSuccessStatusCode();

            Assert.Equal("Success", value.ExecutionResult.Status);
            Assert.Equal("DEV002", value.CostCentre);
            Assert.Equal(1024.01M, value.Total);
            Assert.True(value.TotalExcludingGST > 0 && value.TotalExcludingGST <= value.Total);
        }

        [Fact]
        public async Task Payment_Expense_ValidInput_NoCostCenter_ReturnsResponse()
        {
            //Valid Xml, return response

            var text = @"Hi Yvaine,

                        Please create an expense claim for the below. Relevant details are marked up as requested…

                        <expense><total>1024.01</total><payment_method>personal card</payment_method> </expense>
                        
                        From: Ivan Castle 
                        Sent: Friday, 16 February 2018 10:32 AM 
                        To: Antoine Lloyd <Antoine.Lloyd@example.com> 
                        Subject: test

                        Hi Antoine,
                        
                        Please create a reservation at the <vendor>Viaduct Steakhouse</vendor> our <description>development team’s project end celebration dinner</description> on <date>Tuesday 27 April 2017</date>. We expect to arrive around 7.15pm. Approximately 12 people but I’ll confirm exact numbers closer to the day.
                        
                        Regards,
                        Ivan";

            var request = CreateRequest(text);
            SetAuthHeader();

            var response = await client.PostAsync(request.Url, ContentHelper.GetStringContent(request.Body));
            var value = await response.Content.ReadAsAsync<ExpenseResponse>();
            response.EnsureSuccessStatusCode();

            Assert.Equal("Success", value.ExecutionResult.Status);
            Assert.Equal("UNKNOWN", value.CostCentre);
            Assert.Equal(1024.01M, value.Total);
            Assert.True(value.TotalExcludingGST > 0 && value.TotalExcludingGST <= value.Total);
        }

        #region Private Methods
        private (string Url, ExpenseRequest Body) CreateRequest(string rawText)
        {
            var ExpenseRequest = new ExpenseRequest
            {
                RawText = rawText
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

        private void RemoveAuthHeader()
        {
            client.DefaultRequestHeaders.Remove(Library.Constants.API_KEY_HEADER_NAME);
        }
        #endregion
    }
}
