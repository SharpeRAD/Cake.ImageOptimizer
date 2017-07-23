#region Using Statements
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Linq;

using Cake.Core;
using Cake.Core.IO;
#endregion



namespace Cake.ImageOptimizer
{
    /// <summary>
	/// A collection of image optimizers
	/// </summary>
    public class ImageOptimizerFactory : IImageOptimizerFactory
	{
		#region Fields (3) 
        private readonly IFileSystem _FileSystem;

        private IList<IImageOptimizer> _Optimizers = null;
        private readonly object _Lock;
		#endregion 
        




		#region Constructors (1) 
        /// <summary>
        /// Initializes a new instance of the <see cref="ImageOptimizerFactory" /> class.
        /// </summary>
        /// <param name="fileSystem">The file System.</param>
        public ImageOptimizerFactory(IFileSystem fileSystem)
        {
            if (fileSystem == null)
            {
                throw new ArgumentNullException("fileSystem");
            }

            _FileSystem = fileSystem;

            _Optimizers = new List<IImageOptimizer>();
            _Lock = new object();
        }
        #endregion





        #region Methods (10) 
        /// <summary>
        /// Does the optimzer support the image
        /// </summary>
        /// <param name="extension">The extension of the file to optimize.</param>
        public bool Supports(string extension)
        {
            foreach (IImageOptimizer optimizer in _Optimizers)
            {
                if (optimizer.Supports(extension))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Configure the optimizer
        /// </summary>
        /// <param name="environment">The environment.</param>
        public void Configure(ICakeEnvironment environment)
        {
            foreach (IImageOptimizer optimizer in _Optimizers)
            {
                optimizer.Configure(environment);
            }
        }



        /// <summary>
        /// Adds an optimizer to the factory
        /// </summary>
        /// <param name="optimizer">The optimizer to add.</param>
        public void Add(IImageOptimizer optimizer)
        {
            lock (_Lock)
            {
                _Optimizers.Add(optimizer);
            }
        }

        /// <summary>
        /// Removes an optimizer from the factory
        /// </summary>
        /// <param name="optimizer">The optimizer to remove.</param>
        public void Remove(IImageOptimizer optimizer)
        {
            lock (_Lock)
            {
                _Optimizers.Remove(optimizer);
            }
        }

        /// <summary>
        /// Removes all optimizers from the factory
        /// </summary>
        public void Clear()
        {
            lock (_Lock)
            {
                _Optimizers.Clear();
            }
        }



        /// <summary>
        /// Get an optimizer from the factory
        /// </summary>
        /// <param name="name">The name of the optimizer.</param>
        public IImageOptimizer GetByName(string name)
        {
            lock (_Lock)
            {
                IImageOptimizer optimizer = _Optimizers.FirstOrDefault(o => o.Name.ToLower() == name.ToLower());

                if (optimizer != null)
                {
                    return (IImageOptimizer)optimizer.Clone();
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Get an optimizer from the factory
        /// </summary>
        /// <param name="path">The path of the file that the optimzer needs to support.</param>
        public IImageOptimizer GetByPath(FilePath path)
        {
            return this.GetByExtension(path.GetExtension());
        }

        /// <summary>
        /// Get an optimizer from the factory
        /// </summary>
        /// <param name="extension">The extension of the file that the optimzer needs to support.</param>
        public IImageOptimizer GetByExtension(string extension)
        {
            lock (_Lock)
            {
                IImageOptimizer optimizer = _Optimizers.FirstOrDefault(o => o.Supports(extension));

                if (optimizer != null)
                {
                    return (IImageOptimizer)optimizer.Clone();
                }
                else
                {
                    return null;
                }
            }
        }



        /// <summary>
        /// Get an optimizer from the factory
        /// </summary>
        /// <param name="optimizer">The name of the optimizer.</param>
        /// <param name="path">The path of the file to optimize.</param>
        public ImageOptimizerResult Optimize(string optimizer, FilePath path)
        {
            return this.Optimize(optimizer, path, path);
        }

        /// <summary>
        /// Get an optimizer from the factory
        /// </summary>
        /// <param name="optimizer">The name of the optimizer.</param>
        /// <param name="sourcePath">The path of the file to optimize.</param>
        /// <param name="outputPath">The output path for the optimized image.</param>
        public ImageOptimizerResult Optimize(string optimizer, FilePath sourcePath, FilePath outputPath)
        {
            //Get Optimizer
            IImageOptimizer optim = null;

            if (!String.IsNullOrEmpty(optimizer))
            {
                optim = this.GetByName(optimizer);
            }

            if (optim == null)
            {
                optim = this.GetByPath(sourcePath);
            }

 

            //Get Result
            ImageOptimizerResult result = null;

            if (optim != null)
            {
                try
                {
                    //Optimize
                    IFile file = _FileSystem.GetFile(sourcePath);

                    if ((optim.FileSize == 0) || (file.Length < optim.FileSize))
                    {
                        result = optim.Optimize(sourcePath);



                        //Check Sizes
                        if (result.SizeBefore == 0)
                        {
                            result.SizeBefore = file.Length;
                            result.SizeAfter = result.SizeBefore;
                        }



                        //Replace File
                        if (result.SizeAfter != result.SizeBefore)
                        {
                            Directory.CreateDirectory(outputPath.GetDirectory().FullPath);

                            using (WebClient client = new WebClient())
                            {
                                client.DownloadFile(result.DownloadUrl, outputPath.FullPath);
                            }

                            //Date Modified
                            result.ModifiedDate = new FileInfo(file.Path.FullPath).LastWriteTime;
                        }
                        else
                        {
                            //Skipped
                            result = new ImageOptimizerResult(optim.Name, sourcePath, "Matching FileSize");
                        }
                    }
                    else
                    {
                        //Skipped
                        result = new ImageOptimizerResult(optim.Name, sourcePath, "Invalid FileSize");
                    }
                }
                catch(Exception ex)
                {
                    //Error
                    result = new ImageOptimizerResult(optim.Name, sourcePath, ex.Message);
                }
            }
            else
            {
                //Unsupported File
                result = new ImageOptimizerResult(optimizer, sourcePath, "Unsupported File");
            }

            return result;
        }
		#endregion
	}
}