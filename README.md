# Cake.ImageOptimizer
Cake Build addin for optimizing images

[![Build status](https://ci.appveyor.com/api/projects/status/smmki80m1s7yi7xe?svg=true)](https://ci.appveyor.com/project/SharpeRAD/cake-topshelf)

[![cakebuild.net](https://img.shields.io/badge/WWW-cakebuild.net-blue.svg)](http://cakebuild.net/)

[![Join the chat at https://gitter.im/cake-build/cake](https://badges.gitter.im/Join%20Chat.svg)](https://gitter.im/cake-build/cake)



## Table of contents

1. [Implemented functionality](https://github.com/SharpeRAD/Cake.ImageOptimizer#implemented-functionality)
2. [Referencing](https://github.com/SharpeRAD/Cake.ImageOptimizer#referencing)
3. [Usage](https://github.com/SharpeRAD/Cake.ImageOptimizer#usage)
4. [Example](https://github.com/SharpeRAD/Cake.ImageOptimizer#example)
5. [Plays well with](https://github.com/SharpeRAD/Cake.ImageOptimizer#plays-well-with)
6. [License](https://github.com/SharpeRAD/Cake.ImageOptimizer#license)
7. [Share the love](https://github.com/SharpeRAD/Cake.ImageOptimizer#share-the-love)



## Implemented functionality

* Kraken.io



## Referencing

[![NuGet Version](http://img.shields.io/nuget/v/Cake.ImageOptimizer.svg?style=flat)](https://www.nuget.org/packages/Cake.ImageOptimizer/)

Cake.ImageOptimizer is available as a nuget package from the package manager console:

```csharp
Install-Package Cake.ImageOptimizer
```

or directly in your build script via a cake addin directive:

```csharp
#addin "Cake.ImageOptimizer"
```



## Usage

```csharp
#addin "Cake.ImageOptimizer"

Task("Optimize")
    .Description("Optimize images")
    .Does(() =>
{
    var settings = new ImageOptimizerSettings()
    {
        Services = "Kraken",
        SearchFilter = "*.png",
        ConfigFile = "./Files/config.xml"
    };

    OptimizeImages("./Files/Source", "./Files/Destination", settings);
});

RunTarget("Optimize");
```



## Example

A complete Cake example can be found [here](https://github.com/SharpeRAD/Cake.ImageOptimizer/blob/master/test/build.cake).



## Plays well with

If your looking to store images in S3 check out [Cake.AWS.S3](https://github.com/SharpeRAD/Cake.AWS.S3).

If your looking to distribute your images using CloudFront's CDN then check out [Cake.AWS.CloudFront](https://github.com/SharpeRAD/Cake.AWS.CloudFront).



## License

Copyright (c) 2015 - 2017 Phillip Sharpe

Cake.ImageOptimizer is provided as-is under the MIT license. For more information see [LICENSE](https://github.com/SharpeRAD/Cake.ImageOptimizer/blob/master/LICENSE).



## Share the love

If this project helps you in anyway then please :star: the repository.
