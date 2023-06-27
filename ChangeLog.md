Version 2.0.0
-------------

### Breaking Changes

  * Now requires Visual C++ Redistributables for Visual Studio 2019
  * No longer supports .NET 4.0
  * `VssUtils` has been removed and is replaced with `VssFactoryProvider`
  * `IVssImplementation` has been replaced by `IVssFactory` and `IVssInfoProvider`
  * Old APM Begin/End style asynchronous methods have been deprecated. Use new Async-methods instead.
  * Supports only .NET Framework version 4.5 and later, as well as .NET Core 3.1
  * `OSVersionName`, `ProcessorArchitecture` and `OperatingSystemInfo` classes have been removed.
  * License changed from MIT to Apache 2.0

### New features

  * Supports .NET Core 3.1
  * Supports Task-based asynchronous pattern (TAP) with support for `CancellationToken` instead of the old  Asynchronous Programming Model (APM).
  * Supports extension point for loading of platform specific assemblies. (See `IVssAssemblyResolver` and `VssFactoryProvider`)


Version 1.4.0
-------------
  * Now requires Visual C++ Redistributable for Visual Studio 2017
  * Added support for GetRootAndLogicalPrefixPaths from IVssBackupComponentsEx4.
  * Distributed as a NuGet package.
  * Added a build targetting .NET 4.0
  * Internal reorganization of build process.

Version 1.3.0
---------------

  * Moved source to GitHub, reorganized file structure of sources. 
  * Removed support for any pre-vista operating system.
  * Changed target .NET framework to 4.5
  * Changed assembly name of platform specific dll's so they no longer include the version number since the same assembly can be used on all supported operating systems. (eg. AlphaVSS.x86.dll)

Version 1.2.4000.3
------------------
  * BUG-18251: Fixed bug causing IVssBackupComponents.WriterStatus to incorrectly throw exception 
               stating that the operation is not supported on the current operating system on 
               Windows 2008 and Windows Vista.

Version 1.2.4000.2
------------------
  * BUG-17860: All platfom-specific X86 assemblies were signed with the wrong public key and 
               failed to load.
  
Version 1.2.4000.1
------------------
  * BREAKING CHANGE: The class VssWMFileDescription has been renamed to VssWMFileDescriptor
  
  * BREAKING CHANGE: The IVssAsync interface has been removed. Any methods previously returning 
                     IVssAsync are now synchronous and returns void. Asynchronous versions of these
                     methods have been added with the standard APM naming scheme of BeginXXX, EndXXX
                     where the Begin method returns an IVssAsyncResult instance which implements
                     the standard IAsyncResult interface.
  
  * The properties of IVssWMComponent that are not supported on earlier operating systems no longer 
    throw exceptions but return default values instead.
  
  * A new sample application was added; AlphaShadow, which implements much of the same functionality as 
    the VShadow sample application from the Windows SDK.
  
  * Delay signing has been removed and the public key of the assemblies has changed from the previous 
    version.

Version 1.2.3000
--------------
  * Changed assembly naming of the platform specifiec assemblies so that their names 
    are the same regardless of the build configuration.
  
  * Fixed invalid initialization VssBackupComponents of that caused potential memory issue 
    during finalization of this object (calling Release on IVssBackupComponentsEx and Ex2).
  
  * Changed naming of the platform specific libraries, since the same library works on any 
    windows version >= Vista. They are now called AlphaVSS.XX.<platform>.dll for release 
    versions and AlphaVSS.XXd.<platform>.dll for debug versions where XX is 51 for XP, 
    52 for 2003 (and XP x64), and 60 for Vista and later windows versions.
  
  * Removed feature configuration #defines in Config.h. Now uses ALPHAVSS_TARGET everywhere instead.
  
  * The VssAsync interface now has a Wait-method accepting a timeout that is supported on 
    Windows Vista or later operating systems.
  
  * Added new exception VssUnexpectedErrorException to represent the error codes E_UNEXPECTED
    and VSS_E_UNEXPECTED.
  
  * Made most classes in AlphaVSS.Common serializable.
