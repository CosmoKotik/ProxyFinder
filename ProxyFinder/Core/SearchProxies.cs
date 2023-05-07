using Newtonsoft.Json;
using ProxyFinder.Enums;
using ProxyFinder.Geonode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Starksoft.Aspen.Proxy;
using System.Net.Sockets;

namespace ProxyFinder.Core
{
    public class SearchProxies
    {
        //URLS
        #region Geonode
        private string _geonode_proxies = "https://proxylist.geonode.com/api/proxy-list?limit=500";
        #endregion
        #region ProxyScaper
        /*private string _proxyscaper_http_url = "https://api.proxyscrape.com/v2/?request=getproxies&protocol=http&timeout=10000&country=all&ssl=all&anonymity=all";
        private string _proxyscaper_socks4_url = "https://api.proxyscrape.com/v2/?request=getproxies&protocol=socks4&timeout=10000&country=all";
        private string _proxyscaper_socks5_url = "https://api.proxyscrape.com/v2/?request=getproxies&protocol=socks5&timeout=10000&country=all";*/
        private string _proxyscaper_http_url = "https://cosmokott.ru/files/proxies/http_proxies.txt";
        private string _proxyscaper_socks4_url = "https://cosmokott.ru/files/proxies/socks4_proxies.txt";
        private string _proxyscaper_socks5_url = "https://cosmokott.ru/files/proxies/socks5_proxies.txt";
        #endregion

        private List<Proxy> _goodProxies = new List<Proxy>();

        public void Search(Logger log)
        {
            List<Proxy> proxies = new List<Proxy>();

            int p1index = log.AddProgressBar(1);

            for (int i = 0; i < (int)ProxySites.length; i++)
            {
                switch (i)
                {
                    case (int)ProxySites.Geonode:
                        //GetProxyFromGeoNode(ref proxies, _geonode_proxies, log, p1index);
                        break;
                    case (int)ProxySites.ProxyScaper:
                        GetProxyFromProxyScaper(ref proxies, _proxyscaper_http_url, Proxies.Http, log, p1index);
                        GetProxyFromProxyScaper(ref proxies, _proxyscaper_socks4_url, Proxies.Socks4, log, p1index);
                        GetProxyFromProxyScaper(ref proxies, _proxyscaper_socks5_url, Proxies.Socks5, log, p1index);
                        break;
                }
            }
            //Console.WriteLine(proxies.Count);
            
            CheckProxies(proxies, log, p1index);
        }

        public void CheckProxies(List<Proxy> proxies, Logger log, int p1index)
        {
            float amountDone = 0;
            float amountGood = 0;
            float amountBad = 0;

            foreach (Proxy proxy in proxies)
            {
                new Thread(() =>
                {
                    Socks5ProxyClient socks5ProxyClient;
                    Socks4ProxyClient socks4ProxyClient;
                    HttpProxyClient httpProxyClient;

                    TcpClient client;
                    NetworkStream stream;

                    DateTime startTime = DateTime.Now;

                    string destIp = "72.140.210.214";
                    int destPort = 51643;
                    
                    switch (proxy.ProxyType)
                    {
                        case Proxies.Http:
                            httpProxyClient = new HttpProxyClient(proxy.IP, proxy.Port);

                            try
                            {
                                client = httpProxyClient.CreateConnection(destIp, destPort);
                                stream = client.GetStream();
                                //Console.WriteLine("Trying to connect");

                                while ((DateTime.Now.Millisecond - startTime.Millisecond) <= 60000)
                                {
                                    if (client.Connected)
                                    {
                                        //Console.WriteLine($"Connected successfully:  {proxy.IP}:{proxy.Port} {proxy.ProxyType.ToString()}");
                                        _goodProxies.Add(proxy);
                                        amountDone++;
                                        amountGood++;
                                        log.UpdateProgressBar(p1index, (int)MathF.Ceiling((amountDone / proxies.Count) * 100));
                                        Thread.CurrentThread.Interrupt();
                                    }
                                    Thread.Sleep(1);
                                }
                                //Console.WriteLine($"Timeout:  {proxy.IP}:{proxy.Port}");
                            }
                            catch (Exception ex)
                            {
                                //Console.WriteLine($"fail:   {ex.ToString()}");
                            }
                            break;
                        case Proxies.Socks5:
                            socks5ProxyClient = new Socks5ProxyClient(proxy.IP, proxy.Port);

                            try
                            {
                                client = socks5ProxyClient.CreateConnection(destIp, destPort);
                                stream = client.GetStream();
                                //Console.WriteLine("Trying to connect");

                                while ((DateTime.Now.Millisecond - startTime.Millisecond) <= 60000)
                                {
                                    if (client.Connected)
                                    {
                                        //Console.WriteLine($"Connected successfully:  {proxy.IP}:{proxy.Port} {proxy.ProxyType.ToString()}");
                                        _goodProxies.Add(proxy);
                                        amountDone++;
                                        amountGood++;
                                        log.UpdateProgressBar(p1index, (int)MathF.Ceiling((amountDone / proxies.Count) * 100));
                                        Thread.CurrentThread.Interrupt();
                                    }
                                    Thread.Sleep(1);
                                }
                                //Console.WriteLine($"Timeout:  {proxy.IP}:{proxy.Port}");
                            }
                            catch (Exception ex)
                            {
                                //Console.WriteLine($"fail:   {ex.ToString()}");
                            }
                            break;
                        case Proxies.Socks4:
                            socks4ProxyClient = new Socks4ProxyClient(proxy.IP, proxy.Port);

                            try
                            {
                                client = socks4ProxyClient.CreateConnection(destIp, destPort);
                                stream = client.GetStream();
                                //Console.WriteLine("Trying to connect");

                                while ((DateTime.Now.Millisecond - startTime.Millisecond) <= 60000)
                                {
                                    if (client.Connected)
                                    {
                                        //Console.WriteLine($"Connected successfully:  {proxy.IP}:{proxy.Port} {proxy.ProxyType.ToString()}");
                                        _goodProxies.Add(proxy);
                                        amountDone++;
                                        amountGood++;
                                        log.UpdateProgressBar(p1index, (int)MathF.Ceiling((amountDone / proxies.Count) * 100));
                                        Thread.CurrentThread.Interrupt();
                                    }
                                    Thread.Sleep(1);
                                }
                                //Console.WriteLine($"Timeout:  {proxy.IP}:{proxy.Port}");
                            }
                            catch (Exception ex)
                            {
                                //Console.WriteLine($"fail:   {ex.ToString()}");
                            }
                            break;
                    }

                    amountDone++;
                    amountBad++;
                    log.UpdateProgressBar(p1index, (int)MathF.Ceiling((amountDone / proxies.Count) * 100));

                    Thread.CurrentThread.Interrupt();
                }).Start();

                new Thread(() => 
                {
                    while (amountDone < proxies.Count)
                    { 
                        Thread.Sleep(1); 
                    }

                    string json = JsonConvert.SerializeObject(_goodProxies);

                    File.WriteAllText(@"\proxies.cum", json);
                });
                //Thread.Sleep(1);
            }
        }

        private void GetProxyFromProxyScaper(ref List<Proxy> proxies, string url, Proxies type, Logger log, int p1index)
        {
            float maxAmount = 0;
            float amount = 0;

            var webRequest = WebRequest.Create(url);

            using (var response = webRequest.GetResponse())
            using (var content = response.GetResponseStream())
            using (var reader = new StreamReader(content))
            {
                string strContent = "";

                while ((strContent = reader.ReadLine()) != null)
                {
                    Proxy p = new Proxy()
                    {
                        IP = strContent.Split(':')[0],
                        Port = int.Parse(strContent.Split(':')[1]),
                        ProxyType = type,
                        Sites = ProxySites.ProxyScaper
                    };

                    //Console.WriteLine(MathF.Ceiling((amount / maxAmount) * 100));

                    log.UpdateProgressBar(p1index, (int)MathF.Ceiling((amount / maxAmount) * 100));

                    amount++;

                    proxies.Add(p);
                }
            }
        }

        private void GetProxyFromGeoNode(ref List<Proxy> proxies, string url, Logger log, int p1index)
        {
            float maxAmount = 7500; //500proxies per page, 15 pages  =>  500*15=7500
            float amount = 0;

            for (int i = 0; i < 15; i++)
            {
                var webRequest = WebRequest.Create(url + $"&page={i + 1}");

                using (var response = webRequest.GetResponse())
                using (var content = response.GetResponseStream())
                using (var reader = new StreamReader(content))
                {
                    string strContent = reader.ReadToEnd();

                    Geonode.Geonode data = JsonConvert.DeserializeObject<Geonode.Geonode>(strContent);

                    //maxAmount = data.data.Length;

                    for (int x = 0; x < data.data.Length; x++)
                    {
                        amount++;
                        Proxies pprotocol = Proxies.Http;

                        if (data.data[x].protocols[0].Equals("http"))
                            pprotocol = Proxies.Http;
                        else if (data.data[x].protocols[0].Equals("socks4"))
                            pprotocol = Proxies.Socks4;
                        else if (data.data[x].protocols[0].Equals("socks5"))
                            pprotocol = Proxies.Socks5;

                        Proxy p = new Proxy()
                        {
                            IP = data.data[x].ip,
                            Port = int.Parse(data.data[x].port),
                            ProxyType = pprotocol,
                            Sites = ProxySites.Geonode
                        };

                        log.UpdateProgressBar(p1index, (int)MathF.Ceiling((amount / maxAmount) * 100));

                        proxies.Add(p);
                    }
                }
            }
        }
    }
}
