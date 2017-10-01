#region Using Statements
using System;

using Newtonsoft.Json;
#endregion



namespace Cake.ImageOptimizer
{
    [Serializable]
    internal class KrakenResponse
	{
        #region Properties
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }



        [JsonProperty("file_name")]
        public string FileName { get; set; }

        [JsonProperty("kraked_url")]
        public string KrakedUrl { get; set; }



        [JsonProperty("original_size")]
        public double OriginalSize { get; set; }

        [JsonProperty("kraked_size")]
        public double KrakedSize { get; set; }

        [JsonProperty("saved_bytes")]
        public double SavedBytes { get; set; }
        #endregion
    }
}