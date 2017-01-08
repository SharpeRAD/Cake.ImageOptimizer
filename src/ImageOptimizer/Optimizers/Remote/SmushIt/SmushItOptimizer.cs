#region Using Statements
    using System;
    using System.Collections.Generic;

    using Newtonsoft.Json;

    using Cake.Core;
    using Cake.Core.IO;
    using Cake.Core.Diagnostics;
    using Cake.Core.Configuration;
#endregion



namespace Cake.ImageOptimizer
{
    /// <summary>
    /// SmushIt image optimizer.
    /// </summary>
    public class SmushItOptimizer : BaseRemoteOptimizer, IImageOptimizer
    {
        #region Constructor (1)
            /// <summary>
            /// Initializes a new instance of the <see cref="PngOutOptimizer" /> class.
            /// </summary>
            /// <param name="fileSystem">The file system.</param>
            /// <param name="environment">The environment.</param>
            /// <param name="log">The log.</param>
            public SmushItOptimizer(IFileSystem fileSystem, ICakeEnvironment environment, ICakeLog log)
                : base(fileSystem, environment, log)
            {

            }
        #endregion





		#region Properties (4) 
            /// <summary>
            /// Gets the name of the optimizer.
            /// </summary>
            /// <value>The optimizer name.</value>
            public override string Name
            {
                get
                {
                    return "SmushIt";
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
                    return new Uri("http://www.smushit.com/ysmush.it/ws.php", UriKind.Absolute);
                }
            }

            /// <summary>
            /// Gets the parameter name of the file to be uploaded
            /// </summary>
            protected override string FileParameter
            {
                get
                {
                    return "files";
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
                this.Timeout = Convert.ToInt32(environment.GetEnvironmentVariable("PUNYPNG_TIMEOUT"));
                this.FileSize = Convert.ToInt32(environment.GetEnvironmentVariable("PUNYPNG_FILESIZE"));
            }

            /// <summary>
            /// Creates a new object that is a copy of the current instance.
            /// </summary>
            public object Clone()
            {
                return new SmushItOptimizer(_FileSystem, _Environment, _Log);
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
				    {"filename", path.GetFilename().FullPath},
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
                SmushItResponse res = JsonConvert.DeserializeObject<SmushItResponse>(response);

                //Check Error
                if (!String.IsNullOrEmpty(res.Error))
                {
                    return new ImageOptimizerResult(this.Name, path, res.Error);
                }
                else
                {
                    //Check Url
                    Uri url;

                    if (!Uri.TryCreate(res.Destination, UriKind.Absolute, out url))
                    {
                        return new ImageOptimizerResult(this.Name, path, "Invalid Url");
                    }



                    //Sucess
                    return new ImageOptimizerResult(this.Name, path, "")
                    {
                        SizeBefore = res.SourceSize,
                        SizeAfter = res.DestinationSize,

                        DownloadUrl = url.AbsoluteUri
                    };
                }
            }
		#endregion
    }
}