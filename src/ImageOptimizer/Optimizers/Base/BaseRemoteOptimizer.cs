#region Using Statements
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

using RestSharp;

using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Diagnostics;
#endregion



namespace Cake.ImageOptimizer
{
    /// <summary>
    /// Base class for remote, web image optimizers.
    /// </summary>
    public abstract class BaseRemoteOptimizer : BaseOptimizer
    {
        #region Properties
        /// <summary>
        /// Gets the endpoint URL of the web service.
        /// </summary>
        protected abstract Uri Endpoint { get; }

        /// <summary>
        /// Gets the parameter name of the file to be uploaded
        /// </summary>
        protected abstract string FileParameter { get; }
        #endregion





        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseRemoteOptimizer" /> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="log">The log.</param>
        public BaseRemoteOptimizer(IFileSystem fileSystem, ICakeEnvironment environment, ICakeLog log)
            : base(fileSystem, environment, log)
        {

        }
        #endregion





        #region Methods
        /// <summary>
        /// Optimizes the specified file.
        /// </summary>
        /// <param name="path">The path to the file.</param>
        /// <value>The <see cref="ImageOptimizerResult" /> result.</value>
        public ImageOptimizerResult Optimize(FilePath path)
        {
            IDictionary<string, object> parameters = this.PopulatePostData(path.GetFilename().ToString());

            return this.Post(parameters, path);
        }



        /// <summary>
        /// Populates the request data before posting it to the web optimizer
        /// </summary>
        /// <param name="path">The path to the file.</param>
        /// <value>The file request parameters.</value>
        protected abstract IDictionary<string, object> PopulatePostData(FilePath path);

        /// <summary>
        /// Read the response from the web optimizer
        /// </summary>
        /// <param name="response">The response content.</param>
        /// <param name="path">The path to the file.</param>
        /// <value>The <see cref="ImageOptimizerResult" /> result.</value>
        protected abstract ImageOptimizerResult ReadResponse(string response, FilePath path);



        //Helpers
        private ImageOptimizerResult Post(IDictionary<string, object> parameters, FilePath path)
        {
            //Create Request
            var request = new RestRequest(Method.POST);
                
            request.RequestFormat = DataFormat.Json;
            request.Timeout = this.Timeout;



            //Add File
            IFile file = _FileSystem.GetFile(path);

            using (Stream fs = file.OpenRead())
            {
                byte[] data = new byte[fs.Length];

                fs.Read(data, 0, data.Length);
                fs.Close();

                request.AddFile(this.FileParameter, data, path.GetFilename().FullPath);
            }



            //Add Parameters
            foreach (KeyValuePair<string, object> item in parameters)
            {
                request.AddParameter(item.Key, item.Value);
            }

            string value = request.Parameters[0].ToString();



            //Execute the Request
            var client = new RestClient(this.Endpoint.AbsoluteUri);

            IRestResponse response = client.Execute(request);



            //Handle Response
            if (String.IsNullOrEmpty(response.Content) && !String.IsNullOrEmpty(response.ErrorMessage))
            {
                return new ImageOptimizerResult(this.Name, path, response.ErrorMessage);
            }
            else
            {
                try
                {
                    return this.ReadResponse(response.Content, path);
                }
                catch (WebException ex)
                {
                    return new ImageOptimizerResult(this.Name, path, ex.Message);
                }
            }
        }
        #endregion
    }
}