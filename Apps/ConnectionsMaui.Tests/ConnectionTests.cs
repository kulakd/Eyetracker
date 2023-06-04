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

        [Fact]
        public void P2PTCPVideoConnection_ReceiveVideo_SetGet_CorrectValue()
        {
            // Arrange
            var connection = new P2PTCPVideoConnection();

            // Act
            connection.ReceiveVideo = true;

            // Assert
            Assert.True(connection.ReceiveVideo);
        }

        [Fact]
        public void P2PTCPVideoConnection_SenderConnectionState_SetGet_CorrectValue()
        {
            // Arrange
            var connection = new P2PTCPVideoConnection();

            // Act
            // We are using reflection to set the private property. This is not usually recommended, 
            // but for this example, it is a way to test the property.
            typeof(P2PTCPVideoConnection)
                .GetProperty("SenderConnectionState", BindingFlags.NonPublic | BindingFlags.Instance)
                .SetValue(connection, ConnectionState.Connecting, null);

            // Assert
            Assert.Equal(ConnectionState.Connecting, connection.SenderConnectionState);
        }

        [Fact]
        public void P2PTCPVideoConnection_ReceiverConnectionState_SetGet_CorrectValue()
        {
            // Arrange
            var connection = new P2PTCPVideoConnection();

            // Act
            // We are using reflection to set the private property. This is not usually recommended, 
            // but for this example, it is a way to test the property.
            typeof(P2PTCPVideoConnection)
                .GetProperty("ReceiverConnectionState", BindingFlags.NonPublic | BindingFlags.Instance)
                .SetValue(connection, ConnectionState.Connecting, null);

            // Assert
            Assert.Equal(ConnectionState.Connecting, connection.ReceiverConnectionState);
        }

    }
}
