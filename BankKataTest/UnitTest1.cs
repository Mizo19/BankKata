using System.Security.Principal;
using Xunit;
using System;
using System.IO;
using System.Linq;
using Moq;
using BankKata;
using System.Text.RegularExpressions;
namespace BankKataTest
{
    public class UnitTest1
    {
        [Fact]
        public void Test_PrintStatement()
        {
            // Arrange
            var mockDateTimeProvider = new Mock<IDateTimeProvider>();

            // -Moock Pour les dates specifique 
            var date1 = new DateTime(2012, 12, 10);
            var date2 = new DateTime(2012, 01, 13);
            var date3 = new DateTime(2012, 01, 14);

            // Setup Mock Pour les dates specifiques
            mockDateTimeProvider.SetupSequence(m => m.Now)
                .Returns(date1)  // First deposit
                .Returns(date2)  // Second deposit
                .Returns(date3); // Withdrawal

            // Account witg mocked dates 
            var account = new Account(mockDateTimeProvider.Object);

            // Act: Perform transactions
            account.Deposit(1000);  // deposit on 10-12-2012
            account.Deposit(2000);  // deposit on 13-01-2012
            account.Withdraw(500);  // withdraw on 14-01-2012

            // Capture the printed output using StringWriter
            var stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            // Print the statement
            account.PrintStatement();

            // Expected output
            var expected = $@"
                Date       || Amount || Balance
                {date1:dd/MM/yyyy} ||  1000  || 1000
                {date2:dd/MM/yyyy} ||  2000  || 3000
                {date3:dd/MM/yyyy} ||  -500  || 2500
            ";

            // Assert: Take in consideration whitespaces
            Assert.Equal(RemoveWhitespace(expected), RemoveWhitespace(stringWriter.ToString()));
        }



        //Mtethod to remove white spaces
        private string RemoveWhitespace(string input)
        {
            return Regex.Replace(input, @"\s+", "");
        }
    }
}
