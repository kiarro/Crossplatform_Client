using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.Linq;
using NLog;

namespace a4.Models
{
    class Client
    {
        static int port = 11000;
        static string address = "188.134.73.56";
        static public string Port { get { return port.ToString(); } set { port = Int32.Parse(value); } } // server port
        static public string Address { get { return address; } set { address = value; } } // server address
        public static string Send(string message)
        {
            Logger logger = LogManager.GetCurrentClassLogger();
            logger.Debug("Started");
            try
            {
                IPAddress ipAddress;
                if (Regex.IsMatch(address, "\\d+\\.\\d+\\.\\d+\\.\\d+"))
                {
                    ipAddress = new IPAddress(address.Split('.').Select(x => Byte.Parse(x)).ToArray());
                    logger.Debug("numeric ip: {0}", ipAddress);
                } else {
                    ipAddress = Dns.GetHostEntry(address).AddressList[0];
                    logger.Debug("host name to ip: {0}", ipAddress);
                }
                IPEndPoint ipPoint = new IPEndPoint(ipAddress, port);

                Socket socket = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                
                // connect to server
                socket.Connect(ipPoint, TimeSpan.FromSeconds(2));
                Console.Write("Message: \n "+message);
                message += "\u0000\u0000";
                byte[] buffer = Encoding.Unicode.GetBytes(message);
                logger.Debug("Send data: {0}", message);
                socket.Send(buffer);

                // receive response
                buffer = new byte[16]; 
                StringBuilder builder = new StringBuilder();
                int bytes = 0; 
                do
                {
                    bytes = socket.Receive(buffer, buffer.Length, 0);
                    logger.Debug("Receive part data: {0}", Encoding.Unicode.GetString(buffer, 0, bytes));
                    builder.Append(Encoding.Unicode.GetString(buffer, 0, bytes));
                }
                while (builder[builder.Length-1] != '\u0000' || builder[builder.Length-2] != '\u0000');
                logger.Debug("Received data: {0}", builder);
                // close socket
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
                return builder.ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                logger.Error(ex, "Some error: {0}");
                return "::E";
            }
        }
    }
}
