#region Using Statements
using System;

using Newtonsoft.Json;
#endregion



namespace Cake.ImageOptimizer
{
    [Serializable]
	internal class PunyPngResponse
	{
        #region Properties
        [JsonProperty("original_url")]
		public string OriginalUrl { get; set; }

        [JsonProperty("original_size")]
		public double OriginalSize { get; set; }



        [JsonProperty("optimized_url")]
		public string OptimizedUrl { get; set; }

        [JsonProperty("optimized_size")]
		public double OptimizedSize { get; set; }



        [JsonProperty("savings_percent")]
		public double SavingsPercent { get; set; }



        [JsonProperty("error")]
        public string Error { get; set; }
        #endregion
	}
}