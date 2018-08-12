#region Using Statements
using Xunit;
#endregion



namespace Cake.ImageOptimizer.Tests
{
    public class ImageOptimizerTests
    {
        [Fact]
        public void Optimize_Images()
        {
            ImageOptimizerSettings settings = new ImageOptimizerSettings()
            {
                Services = "Kraken",
                SearchFilter = "*",
                ConfigFile = "./Files/config.xml"
            };

            CakeHelper.CreateContext().OptimizeImages("./Files/Source", "./Files/Destination", settings);
        }
    }
}