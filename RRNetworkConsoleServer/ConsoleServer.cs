namespace RRNetworkConsoleServer
{
    using System;
    using System.Net;
    using System.Net.Sockets;
    using System.Text;

    internal class ConsoleServer
    {
        public static void Main(
            string[] args)
        {
            TcpListener server = null;
            try
            {
                var portSettings = InitializePortSettings();
                server = new TcpListener(
                    localaddr: IPAddress.Any,
                    port: portSettings.port);

                Console.WriteLine(value: "Listening...");
                server.Start();

                while (true)
                {
                    var client = server.AcceptTcpClient();

                    var stream = client.GetStream();
                    var buffer = new byte[client.ReceiveBufferSize];

                    var bytesRead = stream.Read(
                        buffer: buffer,
                        offset: 0,
                        size: client.ReceiveBufferSize);

                    var receivedMessage = Encoding.UTF8.GetString(
                        bytes: buffer,
                        index: 0,
                        count: bytesRead);

                    if (receivedMessage.Length > 0)
                    {
                        Console.WriteLine(
                            format: "Received: {0}",
                            arg0: receivedMessage);
                    }

                    client.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(value: e.Message);
            }
            finally
            {
                server?.Stop();
            }
        }

        private static PortSettings InitializePortSettings()
        {
            Console.WriteLine(value: "Enter host port: ");
            var port = Console.ReadLine();

            if (port == null)
            {
                throw new Exception(message: "Enter correct values");
            }

            return new PortSettings(
                port: int.Parse(s: port));
        }

        private class PortSettings
        {
            public PortSettings(
                int port)
            {
                this.port = port;
            }

            public int port { get; }
        }
    }
}
