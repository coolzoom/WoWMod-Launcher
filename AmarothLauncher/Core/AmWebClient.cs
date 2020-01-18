using System;
using System.Net;

namespace AmarothLauncher.Core
{
    /// <summary>
    /// 具有可变超时的WebClient。
    /// </summary>
    public class AmWebClient : WebClient
    {
        // Timeout in miliseconds.
        public int Timeout { get; set; }

        /// <summary>
        /// 创建一个默认60秒超时的新WebClient。
        /// </summary>
        public AmWebClient() : this(60000) { }

        /// <summary>
        /// 创建具有给定超时（毫秒）的新WebClient。
        /// </summary>
        /// <param name="timeout"></param>
        public AmWebClient(int timeout)
        {
            Timeout = timeout;
        }

        protected override WebRequest GetWebRequest(Uri address)
        {
            var request = base.GetWebRequest(address);
            if (request != null)
            {
                request.Timeout = this.Timeout;
            }
            return request;
        }
    }
}