#region Using Statements
using System;
using System.Collections.Generic;

using Newtonsoft.Json;

using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Diagnostics;
#endregion



namespace Cake.ImageOptimizer
{
    /// <summary>
    /// PunyPng image optimizer.
    /// </summary>
    public class PunyPngOptimizer : BaseRemoteOptimizer, IImageOptimizer
    {
        #region Fields
        private string _Key = "441946bd843132b36fb27ce1d71106ee5b52e5fb";
        #endregion
        




        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="PngOutOptimizer" /> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="log">The log.</param>
        public PunyPngOptimizer(IFileSystem fileSystem, ICakeEnvironment environment, ICakeLog log)
            : base(fileSystem, environment, log)
        {

        }
        #endregion





        #region Properties
        /// <summary>
        /// Gets the name of the optimizer.
        /// </summary>
        /// <value>The optimizer name.</value>
        public override string Name
        {
            get
            {
                return "PunyPng";
            }
        }

        /// <summary>
        /// A list of extensions supported by the Optimizer
        /// </summary>
        /// <value>The file extensions.</value>
        public override IList<string> Extensions
        {
            get
            {
                return new List<string>() { ".png", ".jpg", ".jpeg", ".gif" };
            }
        }

        /// <summary>
        /// Gets the endpoint URL of the web service.
        /// </summary>
		protected override Uri Endpoint
		{
			get 
            { 
                return new Uri("http://www.punypng.com/api/optimize", UriKind.Absolute); 
            }
		}

        /// <summary>
        /// Gets the parameter name of the file to be uploaded
        /// </summary>
        protected override string FileParameter
        {
            get
            {
                return "img";
            }
        }



        /// <summary>
        /// Gets or sets the PunyPng API Key
        /// </summary>
        public string Key
        {
            get 
            { 
                return _Key; 
            }
            set
            {
                _Key = value;
            }
        }
		#endregion





		#region Methods
        /// <summary>
        /// Configure the optimizer
        /// </summary>
        /// <param name="environment">The environment.</param>
        public void Configure(ICakeEnvironment environment)
        {
            this.Key = environment.GetEnvironmentVariable("PUNYPNG_KEY");

            this.Timeout = Convert.ToInt32(environment.GetEnvironmentVariable("PUNYPNG_TIMEOUT"));
            this.FileSize = Convert.ToInt32(environment.GetEnvironmentVariable("PUNYPNG_FILESIZE"));
        }

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        public object Clone()
        {
            return new PunyPngOptimizer(_FileSystem, _Environment, _Log);
        }



        /// <summary>
        /// Populates the request data before posting it to the web optimizer
        /// </summary>
        /// <param name="path">The path to the file.</param>
        /// <value>The file request parameters.</value>
        protected override IDictionary<string, object> PopulatePostData(FilePath path)
		{
			return new Dictionary<string, object>()
            {
				{"key", _Key},
			};
		}

        /// <summary>
        /// Read the response from the web optimizer
        /// </summary>
        /// <param name="response">The response content.</param>
        /// <param name="path">The path to the file.</param>
        /// <value>The <see cref="ImageOptimizerResult" /> result.</value>
        protected override ImageOptimizerResult ReadResponse(string response, FilePath path)
		{
            PunyPngResponse res = JsonConvert.DeserializeObject<PunyPngResponse>(response);

            //Check Error
            if (!String.IsNullOrEmpty(res.Error))
            {
                return new ImageOptimizerResult(this.Name, path, res.Error);
            }
            else
            {
                //Check Url
                Uri url;

                if (!Uri.TryCreate(res.OptimizedUrl, UriKind.Absolute, out url))
                {
                    return new ImageOptimizerResult(this.Name, path, "Invalid Url");
                }



                //Sucess
                return new ImageOptimizerResult(this.Name, path, "")
                {
                    SizeBefore = res.OriginalSize,
                    SizeAfter = res.OptimizedSize,

                    DownloadUrl = url.AbsoluteUri
                };
            }
		}
		#endregion
	}
}