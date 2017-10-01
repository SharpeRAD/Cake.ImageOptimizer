#region Using Statements
using System;
using System.Collections.Generic;
#endregion



namespace Cake.ImageOptimizer
{
    /// <summary>
    /// The optimized file event arguments
    /// </summary>
	public class OptimizedFileEventArgs : EventArgs
	{
        #region Constructors 
        /// <summary>
        /// Initializes a new instance of the <see cref="OptimizedFileEventArgs"/> class.
        /// </summary>
        /// <param name="file">The optimzied file</param>
        public OptimizedFileEventArgs(OptimizedFile file)
		{
            this.Files = new List<OptimizedFile>() { file };
		}

        /// <summary>
        /// Initializes a new instance of the <see cref="OptimizedFileEventArgs"/> class.
        /// </summary>
        /// <param name="files">The optimzied files</param>
        public OptimizedFileEventArgs(IList<OptimizedFile> files)
        {
            this.Files = files;
        }
		#endregion





		#region Properties 
        /// <summary>
        /// Gets the optimized file
        /// </summary>
        public OptimizedFile File
        {
            get
            {
                return this.Files[0];
            }
        }

        /// <summary>
        /// Gets the list of optimized files
        /// </summary>
        public IList<OptimizedFile> Files { get; private set; }
		#endregion
	}
}