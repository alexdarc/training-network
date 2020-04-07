namespace RRNetworkConsole
{
    using System;
    using System.Net;
    using System.Net.Sockets;
    using System.Text;

    internal class ConsoleClient
    {
        public static void Main(
            string[] args)
        {
            var clientSettings = InitializeClientSettings();
            TestConnection(clientSettings: clientSettings);

            while (true)
            {
                Console.Write(value: "Enter message (Enter to exit): ");
                var message = Console.ReadLine();

                if (message?.Length == 0)
                {
                    break;
                }

                Console.WriteLine(value: "Sending...");
                SendMessage(
                    clientSettings: clientSettings,
                    message: message);
                Console.WriteLine(format: "Message '{0}' sent", arg0: message);
            }
        }

        private static void SendMessage(
            ClientSettings clientSettings,
            string message)
        {
            var client = new TcpClient(
                hostname: clientSettings.hostname,
                port: clientSettings.port);
            var stream = client.GetStream();
            var bytesToSend = Encoding.UTF8.GetBytes(s: message);

            stream.Write(
                buffer: bytesToSend,
                offset: 0,
                size: bytesToSend.Length);
            client.Close();
        }

        private static ClientSettings InitializeClientSettings()
        {
            Console.WriteLine(value: "Enter hostname: ");
            var host = Console.ReadLine();
            Console.WriteLine(value: "Enter port: ");
            var port = Console.ReadLine();

            if (host == null || port == null)
            {
                throw new Exception(message: "Hostname or port are incorrect");
            }

            return new ClientSettings(
                hostname: host,
                port: int.Parse(s: port));
        }

        private static void TestConnection(ClientSettings clientSettings)
        {
            var client = new TcpClient(
                hostname: clientSettings.hostname,
                port: clientSettings.port);
            client.Close();
        }

        private class ClientSettings
        {
            public ClientSettings(
                string hostname,
                int port)
            {
                this.hostname = hostname;
                this.port = port;
            }

            public string hostname { get; }

            public int port { get; }
        }
    }
}
