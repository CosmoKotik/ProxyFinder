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

namespace ProxyFinder.Core
{
    public class SearchProxies
    {
        //URLS
        #region Geonode
        private string _geonode_proxies = "https://proxylist.geonode.com/api/proxy-list?limit=500";
        #endregion
        #region ProxyScaper
        private string _proxyscaper_http_url = "https://api.proxyscrape.com/v2/?request=getproxies&protocol=http&timeout=10000&country=all&ssl=all&anonymity=all";
        private string _proxyscaper_socks4_url = "https://api.proxyscrape.com/v2/?request=getproxies&protocol=socks4&timeout=10000&country=all";
        private string _proxyscaper_socks5_url = "https://api.proxyscrape.com/v2/?request=getproxies&protocol=socks5&timeout=10000&country=all";
        #endregion

        public void Search()
        {
            List<Proxy> proxies = new List<Proxy>();

            for (int i = 0; i < (int)ProxySites.length; i++)
            {
                switch (i)
                {
                    case (int)ProxySites.Geonode:
                        GetProxyFromGeoNode(ref proxies, _geonode_proxies);
                        break;
                    case (int)ProxySites.ProxyScaper:
                        GetProxyFromProxyScaper(ref proxies, _proxyscaper_http_url, Proxies.Http);
                        GetProxyFromProxyScaper(ref proxies, _proxyscaper_socks4_url, Proxies.Socks4);
                        GetProxyFromProxyScaper(ref proxies, _proxyscaper_socks5_url, Proxies.Socks5);
                        break;
                }
            }

            Console.WriteLine(proxies.Count);
        }

        private void GetProxyFromProxyScaper(ref List<Proxy> proxies, string url, Proxies type)
        {
            for (int i = 0; i < 15; i++)
            {
                var webRequest = WebRequest.Create(url + $"&page={i + 1}");

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

                        proxies.Add(p);
                    }
                }
            }
        }

        private void GetProxyFromGeoNode(ref List<Proxy> proxies, string url)
        {
            var webRequest = WebRequest.Create(url);

            using (var response = webRequest.GetResponse())
            using (var content = response.GetResponseStream())
            using (var reader = new StreamReader(content))
            {
                string strContent = reader.ReadToEnd();

                Geonode.Geonode data = JsonConvert.DeserializeObject<Geonode.Geonode>(strContent);

                for (int i = 0; i < data.data.Length; i++)
                {
                    Proxies pprotocol = Proxies.Http;

                    if (data.data[i].protocols[0].Equals("http"))
                        pprotocol = Proxies.Http;
                    else if (data.data[i].protocols[0].Equals("socks4"))
                        pprotocol = Proxies.Socks4;
                    else if (data.data[i].protocols[0].Equals("socks5"))
                        pprotocol = Proxies.Socks5;

                    Proxy p = new Proxy()
                    {
                        IP = data.data[i].ip,
                        Port = int.Parse(data.data[i].port),
                        ProxyType = pprotocol,
                        Sites = ProxySites.Geonode
                    };

                    proxies.Add(p);
                }
            }
        }
    }
}
