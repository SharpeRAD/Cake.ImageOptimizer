#region Using Statements
using System.IO;

using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;
using Cake.Core.Configuration;
using Cake.Testing;

using NSubstitute;
#endregion



namespace Cake.ImageOptimizer.Tests
{
    internal static class CakeHelper
    {
        #region Methods
        public static ICakeEnvironment CreateEnvironment()
        {
            var environment = FakeEnvironment.CreateWindowsEnvironment();
            environment.WorkingDirectory = Directory.GetCurrentDirectory();
            environment.WorkingDirectory = environment.WorkingDirectory.Combine("../../../");

            return environment;
        }

        public static ICakeArguments CreateArguments()
        {
            var environment = Substitute.For<ICakeArguments>();

            return environment;
        }
        
        public static ICakeConfiguration CreateConfiguration()
        {
            var environment = Substitute.For<ICakeConfiguration>();

            return environment;
        }

        public static IToolLocator CreateToolLocator(ICakeEnvironment environment, ICakeConfiguration config)
        {
            return new ToolLocator(environment, new ToolRepository(environment), new ToolResolutionStrategy(new FileSystem(), environment, new Globber(new FileSystem(), environment), config));
        }


        public static ICakeContext CreateContext()
        {
            ICakeEnvironment enviroment = CakeHelper.CreateEnvironment();
            ICakeConfiguration config = CakeHelper.CreateConfiguration();
            IToolLocator toolLocator = CakeHelper.CreateToolLocator(enviroment, config);

            return new CakeContext(new FileSystem(), enviroment, new Globber(new FileSystem(), enviroment), new DebugLog(), CreateArguments(), new ProcessRunner(new FileSystem(), enviroment, new DebugLog(), toolLocator, config), new WindowsRegistry(), toolLocator, new CakeDataService(), config);
        }
        #endregion
    }
}