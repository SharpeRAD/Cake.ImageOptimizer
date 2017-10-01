#region Using Statements
using System;

using Newtonsoft.Json;
#endregion



namespace Cake.ImageOptimizer
{
    [Serializable]
	internal class SmushItResponse
	{
        #region Properties
        [JsonProperty("src")]
		public string Source { get; set; }

        [JsonProperty("src_size")]
		public double SourceSize { get; set; }



        [JsonProperty("dest")]
		public string Destination { get; set; }

        [JsonProperty("dest_size")]
		public double DestinationSize { get; set; }



        [JsonProperty("percent")]
		public double Percent { get; set; }

        [JsonProperty("error")]
        public string Error { get; set; }
        #endregion
	}
}