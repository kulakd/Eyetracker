using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ConnectionsMaui.Tests
{
    public class ConnectionTests
    {
        [Fact]
        public void ConnectionEventEventArgs_Constructor_SetsPropertiesCorrectly()
        {
            var args = new ConnectionEventEventArgs(ConnectionState.Connected, ConnectionState.Disconnecting);
            Assert.Equal(ConnectionState.Connected, args.SenderState);
            Assert.Equal(ConnectionState.Disconnecting, args.ReceiverState);
        }

        [Fact]
        public void ConnectionEventEventArgs_ToString_ReturnsCorrectFormat_WhenStatesAreEqual()
        {
            var args = new ConnectionEventEventArgs(ConnectionState.Connected, ConnectionState.Connected);
            Assert.Equal("Connection Status: Connected", args.ToString());
        }

        [Fact]
        public void ConnectionEventEventArgs_ToString_ReturnsCorrectFormat_WhenStatesAreDifferent()
        {
            var args = new ConnectionEventEventArgs(ConnectionState.Connected, ConnectionState.Disconnecting);
            Assert.Equal("Connection Status: Sender Connected - Receiver Disconnecting", args.ToString());
        }

        

    }
}
