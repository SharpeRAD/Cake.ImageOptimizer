#region Using Statements
using System;
using System.Linq;
using System.Threading;
using System.Globalization;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Collections.Generic;

using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Diagnostics;
#endregion



namespace Cake.ImageOptimizer
{
    /// <summary>
    /// Responsible for loading and saving the optimized results
    /// </summary>
    public class FileConfig
    {
        #region Fields
        private readonly IFileSystem _FileSystem;
        private readonly ICakeEnvironment _Environment;
        private readonly ICakeLog _Log;

        private readonly object _Lock;
        private IList<OptimizedFile> _Files;
        #endregion





        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="FileConfig" /> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="log">The log.</param>
        public FileConfig(IFileSystem fileSystem, ICakeEnvironment environment, ICakeLog log)
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

            _Lock = new object();
            _Files = new List<OptimizedFile>();
        }
		#endregion





		#region Properties
        /// <summary>
        /// Gets the list of optimized files
        /// </summary>
        public IList<OptimizedFile> Files
        {
            get
            {
                return _Files;
            }
        }



        /// <summary>
        /// Gets the size of all the files before optimization
        /// </summary>
        public double TotalSizeBefore 
        {
            get
            {
                double total = 0;

                foreach (OptimizedFile file in _Files)
                {
                    total += file.SizeBefore;
                }

                return total;
            }
        }

        /// <summary>
        /// Gets the size of all the files after optimization
        /// </summary>
        public double TotalSizeAfter
        {
            get
            {
                double total = 0;

                foreach (OptimizedFile file in _Files)
                {
                    total += file.SizeAfter;
                }

                return total;
            }
        }

        /// <summary>
        /// Gets the saving in bytes from the optimization
        /// </summary>
        public double TotalSavedSize
        {
            get
            {
                if (this.TotalSizeAfter < this.TotalSizeBefore)
                {
                    return this.TotalSizeBefore - this.TotalSizeAfter;
                }
                else
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// Gets the saving as a percentage from the optimization
        /// </summary>
        public double TotalSavedPercent
        {
            get
            {
                if (this.TotalSizeAfter < this.TotalSizeBefore)
                {
                    return 100 - Math.Round((100 / this.TotalSizeBefore) * this.TotalSizeAfter, 1);
                }
                else
                {
                    return 0;
                }
            }
        }
		#endregion





        #region Mwthods
        /// <summary>
        /// Loads the list of already optimized files
        /// </summary>
        /// <param name="path">The location of the config file.</param>
        public void Load(FilePath path)
        {
            lock (_Lock)
            {
                path = path.MakeAbsolute(_Environment);

                if (_FileSystem.Exist(path))
                {
                    IFile file = _FileSystem.GetFile(path);

                    using (Stream stream = file.OpenRead())
                    {
                        XDocument doc = XDocument.Load(stream);

                        if (doc != null)
                        {
                            this.Configure(doc.Root);
                        }
                    }
                }
            }
        }

        private void Configure(XElement element)
        {
            //Set Culture
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-GB");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-GB");



            //Files
            _Files = (from c in element.Descendants("Add")

                        select new OptimizedFile(
                                c.Attribute("Location").Value,

                                c.Attribute("Service").Value,
                                DateTime.ParseExact(c.Attribute("Date").Value, "yyyy/MM/dd HH:mm", CultureInfo.CurrentCulture),
                                c.Attribute("Hash").Value,

                                Convert.ToDouble(c.Attribute("SizeBefore").Value),
                                Convert.ToDouble(c.Attribute("SizeAfter").Value)
                            )).ToList<OptimizedFile>();
        }



        /// <summary>
        /// Saves the list of optimized files
        /// </summary>
        /// <param name="path">The location of the config file.</param>
        public void Save(FilePath path)
        {
            lock (_Lock)
            {
                using (StreamWriter file = new StreamWriter(path.MakeAbsolute(_Environment).FullPath))
                {
                    file.Write(this.GetString(this.Create()));

                    file.Close();
                }
            }
        }

        private string GetString(XDocument doc)
        {
            //Settings
            XmlWriterSettings settings = new XmlWriterSettings
            {
                Indent = true,
                IndentChars = "     ",
                NewLineOnAttributes = false,
                OmitXmlDeclaration = true
            };



            //Write
            StringWriter output = new StringWriter();

            using (XmlWriter writer = XmlWriter.Create(output, settings))
            {
                doc.WriteTo(writer);
            }

            return output.ToString();
        }

        private XDocument Create()
        {
            //Files
            XElement element = new XElement("Files");

            foreach (OptimizedFile file in _Files)
            {
                element.Add(new XElement("Add",
                                    new XAttribute("Location", file.Path),

                                    new XAttribute("Service", file.Service),
                                    new XAttribute("Date", file.OptimizedDate.ToString("yyyy/MM/dd HH:mm")),
                                    new XAttribute("Hash", file.OptimizedHash),

                                    new XAttribute("SizeBefore", file.SizeBefore),
                                    new XAttribute("SizeAfter", file.SizeAfter)));
            }



            //Document
            XDocument doc = new XDocument(
                new XDeclaration("1.0", "utf-8", "yes"),
                    new XComment("Optimized Image Results"),
                    element);

            return doc;
        }



        /// <summary>
        /// Checks if a file requires optimization
        /// </summary>
        /// <param name="path">The file path to check.</param>
        /// <param name="hash">The file hash to compare.</param>
        public bool RequiresOptimization(FilePath path, string hash)
        {
            OptimizedFile file = _Files.FirstOrDefault(f => f.Path.FullPath.ToLower() == path.FullPath.ToLower());

            if (file != null)
            {
                //Existing File
                return file.RequiresOptimization(hash);
            }
            else
            {
                //No File
                return true;
            }
        }



        /// <summary>
        /// Adds a list of results to the config file
        /// </summary>
        /// <param name="files">The optimized files.</param>
        public void AddResults(IList<OptimizedFile> files)
        {
            foreach (OptimizedFile file in files)
            {
                this.AddResult(file);
            }
        }

        /// <summary>
        /// Adds a result to the config file
        /// </summary>
        /// <param name="file">The optimized file.</param>
        public void AddResult(OptimizedFile file)
        {
            this.AddResult(file.Path, file.Service, file.OptimizedDate, file.OptimizedHash, file.SizeBefore, file.SizeAfter);
        }

        private void AddResult(FilePath path, string service, DateTimeOffset optimizedDate, string optimizedHash, double sizeBefore, double sizeAfter)
        {
            lock (_Lock)
            {
                OptimizedFile file = _Files.FirstOrDefault(f => f.Path.FullPath.ToLower() == path.FullPath.ToLower());

                if (file != null)
                {
                    //Existing File
                    file.Service = service;

                    file.OptimizedDate = optimizedDate;
                    file.OptimizedHash = optimizedHash;

                    file.SizeBefore = sizeBefore;
                    file.SizeAfter = sizeAfter;
                }
                else
                {
                    //No File
                    _Files.Add(new OptimizedFile(path, service, optimizedDate, optimizedHash, sizeBefore, sizeAfter));
                }
            }
        }
        #endregion
    }
}