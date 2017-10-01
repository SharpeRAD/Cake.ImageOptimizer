#region Using Statements
using System;
#endregion



namespace Cake.ImageOptimizer
{
    [Serializable]
    internal class KrakenAuth
	{
        #region Constructors
        public KrakenAuth()
            : this("", "")
        {

        }

        public KrakenAuth(string key, string secret)
        {
            api_key = key;
            api_secret = secret;
        }
        #endregion





        #region Properties
        public string api_key { get; set; }

        public string api_secret { get; set; }
        #endregion
    }
}