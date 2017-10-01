#region Using Statements
using System;
using System.Collections.Generic;

using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Diagnostics;
#endregion



namespace Cake.ImageOptimizer
{
    /// <summary>
    /// Base class for all image optimizers.
    /// </summary>
    public abstract class BaseOptimizer
    {
        #region Fields
        /// <summary>
        /// Represents a file system.
        /// </summary>
        protected readonly IFileSystem _FileSystem;

        /// <summary>
        /// Represents the environment Cake operates in.
        /// </summary>
        protected readonly ICakeEnvironment _Environment;

        /// <summary>
        /// Represents a log.
        /// </summary>
        protected readonly ICakeLog _Log;
        #endregion





        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseOptimizer" /> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="log">The log.</param>
        public BaseOptimizer(IFileSystem fileSystem, ICakeEnvironment environment, ICakeLog log)
        {
            if (fileSystem == null)
            {
                throw new ArgumentNullException("fileSystem");
            }
            if (environment == null)
            {
                throw new ArgumentNullException("environment");
            }
            if (log == null)
            {
                throw new ArgumentNullException("log");
            }

            _FileSystem = fileSystem;
            _Environment = environment;
            _Log = log;
        }
        #endregion





        #region Properties
        /// <summary>
        /// Gets the name of the optimizer.
        /// </summary>
        /// <value>The optimizer name.</value>
        public abstract string Name { get; }

        /// <summary>
        /// A list of extensions supported by the Optimizer
        /// </summary>
        /// <value>The file extensions.</value>
        public abstract IList<string> Extensions { get; }



        /// <summary>
        /// Gets the maximum file size to optimize.
        /// </summary>
        public int FileSize { get; set; }
  
        /// <summary>
        /// Gets the maximum timespan for the request.
        /// </summary>
        public int Timeout { get; set; }
        #endregion





        #region Methods
        /// <summary>
        /// Does the specified file.
        /// </summary>
        /// <param name="extension">The file extensions.</param>
        public bool Supports(string extension)
        {
            return this.Extensions.Contains(extension.ToLower());
        }
        #endregion
    }
}