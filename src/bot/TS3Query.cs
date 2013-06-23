using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace TS3Query
{
    public class TS3Query
    {
        /**
         * Properties
         */

        // Public properties
        public IPEndPoint Endpoint
        { get; set; }
        public static ushort DefaultPort
        { get { return 25639; } }
        public static IPAddress DefaultIP
        { get { return IPAddress.Loopback; } }
        public static IPEndPoint DefaultEndpoint
        { get { return new IPEndPoint(DefaultIP, DefaultPort); } }

        // Public events
        public event EventHandler<TS3QueryRequestEventArgs> QueryRequestSent;
        public event EventHandler<TS3QueryResponseEventArgs> QueryResponseReceived;
        public event EventHandler Connected;
        public event EventHandler Disconnected;

        // Public read-only properties
        public string ServerName
        { get; private set; }
        public string ServerMOTD
        { get; private set; }

        // Private fields
        private TcpClient _tcp;
        private NetworkStream _ns;
        private StreamReader _sr;
        private StreamWriter _sw;
        private Task _t;

        /**
         * Functions/Methods
         */

        // Public methods
        public void Connect(IPEndPoint endpoint)
        {
            Endpoint = endpoint;

            Connect();
        }
        public void Connect()
        {
            if (Endpoint == null)
            {
                //throw new InvalidOperationException("Needs an endpoint.");
                Endpoint = DefaultEndpoint;
            }

            _tcp = new TcpClient();
            _tcp.Connect(Endpoint);

            _ns = _tcp.GetStream();
            _sr = new StreamReader(_ns);
            _sw = new StreamWriter(_ns);

            ServerName = ServerMOTD = null;

            do
            {
                ServerName = _sr.ReadLine();
            } while (string.IsNullOrEmpty(ServerName));
            Debug.WriteLine("ServerName = {0}", ServerName, null);

            do
            {
                ServerMOTD = _sr.ReadLine();
            } while (string.IsNullOrEmpty(ServerMOTD));
            Debug.WriteLine("ServerMOTD = {0}", ServerMOTD, null);

            OnConnected();

            _t = Task.Factory.StartNew(() =>
            {
                while (_tcp.Connected)
                {
                    var line = _sr.ReadLine();

                    if (string.IsNullOrEmpty(line) || string.IsNullOrWhiteSpace(line))
                        continue;

                    OnQueryResponseReceived(new TS3QueryResponse(line));
                }
            });

            // On exceptions
            // TODO: Handle unexpected disconnection properly
            // TODO: Handle expected disconnection properly
            _t.ContinueWith((task) =>
            {
                Console.Error.WriteLine(task.Exception.ToString());
                Disconnect();
            }, TaskContinuationOptions.OnlyOnFaulted);

        }
        public void Disconnect()
        {
            try
            {
                _tcp.Close();
                _sr.Dispose();
                _sw.Dispose();
                _ns.Dispose();
            }
            catch { { } }

            OnDisconnected();
        }
        public void Send(TS3QueryRequest request)
        {
            if (request == null)
                throw new ArgumentNullException("request");
            if (string.IsNullOrEmpty(request.Name) || string.IsNullOrWhiteSpace(request.Name))
                throw new InvalidDataException("Request command name is invalid.");
            _sw.WriteLine(request.ToString());
            _sw.Flush();

            OnQueryRequestSent(request);
        }

        // Protected event methods
        protected void OnDisconnected()
        {
            if (Disconnected != null)
                Disconnected.Invoke(this, null);
        }
        protected void OnConnected()
        {
            if (Connected != null)
                Connected.Invoke(this, null);
        }
        protected void OnQueryRequestSent(TS3QueryRequest e)
        {
            if (QueryRequestSent != null)
                QueryRequestSent.Invoke(this, new TS3QueryRequestEventArgs(e));
        }
        protected void OnQueryResponseReceived(TS3QueryResponse e)
        {
            if (QueryResponseReceived != null)
                QueryResponseReceived.Invoke(this, new TS3QueryResponseEventArgs(e));
        }
    }
}
