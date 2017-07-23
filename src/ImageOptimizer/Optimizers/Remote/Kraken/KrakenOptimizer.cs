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
    /// Kraken image optimizer.
    /// </summary>
    public class KrakenOptimizer : BaseRemoteOptimizer, IImageOptimizer
	{
        #region Fields (3)
        private string _ApiKey = "";
        private string _SecretKey = "";

        private bool _Lossy = false;
        #endregion





        #region Constructors (2)
        /// <summary>
        /// Initializes a new instance of the <see cref="KrakenOptimizer" /> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="log">The log.</param>
        public KrakenOptimizer(IFileSystem fileSystem, ICakeEnvironment environment, ICakeLog log)
            : this(fileSystem, environment, log, "", "")
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="KrakenOptimizer" /> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="log">The log.</param>
        /// <param name="apiKey">The Kraken API Key.</param>
        /// <param name="secretKey">The Kraken API Secret Key.</param>
        public KrakenOptimizer(IFileSystem fileSystem, ICakeEnvironment environment, ICakeLog log, string apiKey, string secretKey)
            : base(fileSystem, environment, log)
        {
            _ApiKey = apiKey;
            _SecretKey = secretKey;
        }
        #endregion





        #region Properties (7)
            /// <summary>
            /// Gets the name of the optimizer.
            /// </summary>
            /// <value>The optimizer name.</value>
            public override string Name
            {
                get
                {
                    return "Kraken";
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
                    return new Uri("https://api.kraken.io/v1/upload", UriKind.Absolute); 
                }
		    }

            /// <summary>
            /// Gets the parameter name of the file to be uploaded
            /// </summary>
            protected override string FileParameter
            {
                get
                {
                    return "file";
                }
            }



            /// <summary>
            /// Gets or sets the Kraken API Key
            /// </summary>
            public string ApiKey
            {
                get
                {
                    return _ApiKey;
                }
                set
                {
                    _ApiKey = value;
                }
            }

            /// <summary>
            /// Gets or sets the Kraken API Secret Key
            /// </summary>
            public string SecretKey
            {
                get
                {
                    return _SecretKey;
                }
                set
                {
                    _SecretKey = value;
                }
            }



            /// <summary>
            /// Gets or sets weather to use lossless image compression
            /// </summary>
            public bool Lossy
            {
                get
                {
                    return _Lossy;
                }
                set
                {
                    _Lossy = value;
                }
            }
		#endregion





		#region Methods (4) 
            /// <summary>
            /// Configure the optimizer
            /// </summary>
            /// <param name="environment">The environment.</param>
            public void Configure(ICakeEnvironment environment)
            {
                this.ApiKey = environment.GetEnvironmentVariable("KRAKEN_API_KEY");
                this.SecretKey = environment.GetEnvironmentVariable("KRAKEN_SECRET_KEY");

                this.Lossy = Convert.ToBoolean(environment.GetEnvironmentVariable("KRAKEN_LOSSY"));

                this.Timeout = Convert.ToInt32(environment.GetEnvironmentVariable("KRAKEN_TIMEOUT"));
                this.FileSize = Convert.ToInt32(environment.GetEnvironmentVariable("KRAKEN_FILESIZE"));
            }

            /// <summary>
            /// Creates a new object that is a copy of the current instance.
            /// </summary>
            public object Clone()
            {
                return new KrakenOptimizer(_FileSystem, _Environment, _Log, _ApiKey, _SecretKey);
            }



            /// <summary>
            /// Populates the request data before posting it to the web optimizer
            /// </summary>
            /// <param name="path">The path to the file.</param>
            /// <value>The file request parameters.</value>
            protected override IDictionary<string, object> PopulatePostData(FilePath path)
		    {
                Dictionary<string, object> parameters = new Dictionary<string, object>()
                {
				    { "options",  JsonConvert.SerializeObject(new KrakenOptions(new KrakenAuth(_ApiKey, _SecretKey), true, _Lossy)) }
			    };

                return parameters;
		    }

            /// <summary>
            /// Read the response from the web optimizer
            /// </summary>
            /// <param name="response">The response content.</param>
            /// <param name="path">The path to the file.</param>
            /// <value>The <see cref="ImageOptimizerResult" /> result.</value>
            protected override ImageOptimizerResult ReadResponse(string response, FilePath path)
		    {
                KrakenResponse res = JsonConvert.DeserializeObject<KrakenResponse>(response);

                //Check
                if (res == null)
                {
                    //Invalid
                    return new ImageOptimizerResult(this.Name, path, "Invalid Response");
                }
                else if (!res.Success)
                {
                    //Errored
                    return new ImageOptimizerResult(this.Name, path, String.IsNullOrEmpty(res.Message) ? "Unknown Error" : res.Message);
                }
                else if (res.Message == "This image can not be optimized any further")
                {
                    //Skipped
                    return new ImageOptimizerResult(this.Name, path, "")
                    {
                        SizeBefore = 0,
                        SizeAfter = 0,

                        DownloadUrl = ""
                    };
                }
                else
                {
                    //Check Url
                    Uri url;

                    if (!Uri.TryCreate(res.KrakedUrl, UriKind.Absolute, out url))
                    {
                        return new ImageOptimizerResult(this.Name, path, "Invalid Url");
                    }



                    //Success
                    return new ImageOptimizerResult(this.Name, path, "")
                    {
                        SizeBefore = res.OriginalSize,
                        SizeAfter = res.KrakedSize,

                        DownloadUrl = url.AbsoluteUri
                    };
                }
		    }
		#endregion
	}
}