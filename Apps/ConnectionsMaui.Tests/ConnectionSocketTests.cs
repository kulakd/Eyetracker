using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ConnectionsMaui.Tests
{
    public class ConnectionSocketTests
    {
        [Fact]
        public void ConnectionSettings_Constructor_SetsPropertiesCorrectly()
        {
            var settings = new ConnectionSettings(IPAddress.Loopback, 8080, 8081);
            Assert.Equal(IPAddress.Loopback, settings.Address);
            Assert.Equal(8080, settings.SendPort);
            Assert.Equal(8081, settings.ReceivePort);
        }

        [Fact]
        public void ConnectionSettings_StringConstructor_SetsPropertiesCorrectly()
        {
            var settings = new ConnectionSettings("127.0.0.1", 8080, 8081);
            Assert.Equal(IPAddress.Parse("127.0.0.1"), settings.Address);
            Assert.Equal(8080, settings.SendPort);
            Assert.Equal(8081, settings.ReceivePort);
        }

        [Fact]
        public void ConnectionSettings_Parse_CreatesSettingsCorrectly()
        {
            var settings = ConnectionSettings.Parse("127.0.0.1:8080:8081");
            Assert.Equal(IPAddress.Parse("127.0.0.1"), settings.Address);
            Assert.Equal(8080, settings.SendPort);
            Assert.Equal(8081, settings.ReceivePort);
        }

        [Fact]
        public void ConnectionSettings_ToString_ReturnsCorrectFormat()
        {
            var settings = new ConnectionSettings(IPAddress.Loopback, 8080, 8081);
            Assert.Equal("127.0.0.1:8080:8081", settings.ToString());
        }

        [Fact]
        public void ConnectionSettings_Parse_ThrowsFormatExceptionForInvalidInput()
        {
            Assert.Throws<FormatException>(() => ConnectionSettings.Parse("invalid input"));
        }

    }
}
