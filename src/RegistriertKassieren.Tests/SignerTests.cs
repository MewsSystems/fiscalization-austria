using System;
using RegistriertKassieren.Dto;
using Xunit;

namespace RegistriertKassieren.Tests
{
    public class Fact
    {
        [Fact]
        public void ItWorks()
        {
            var signer = new Signer();
            var result = signer.Sign(new SignerInput("123456789"));
            Console.WriteLine(result.ToString());
        }
    }
}
