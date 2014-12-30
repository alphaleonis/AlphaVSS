AlphaVSS
========

AlphaVSS is a .NET class library released under the MIT license providing a managed API for the Volume Shadow Copy Service also known as VSS. The Volume Shadow Copy Service is a set of COM interfaces that implements a framework to allow volume backups to be performed while applications on a system continue to write to the volumes.

AlphaVSS, written in C# and C++/CLI provides a managed interface to this API.

The goal of AlphaVSS is to provide an interface that is simple to use from any .NET application, yet provides the full functionality of VSS.


## Background

Using the Windows Volume Shadow Copy Service (VSS) on the .NET platform in C# (or VB) is somewhat problematic to say the least. There are numerous posts online about this issue, neither ever mentioning a robust solution that would allow full access to the VSS API from within a .NET application. The reasons are actually somewhat unclear, but it seems to have to do with there being COM interfaces without an IID, and also that several interfaces of the VSS API is not actually COM interfaces but rather C++ interfaces. This means there is no type library available for importing in your .NET application, and creating one from the existing DLL-files seems impossible.

The only viable solution to this problem appears to be to write a custom wrapper in managed C++/CLI, that provides a managed interface to the VSS API that can then be used from a .NET application. The sheer number of interfaces, structures and functions in the Volume Shadow Copy API are quite large, and writing a complete wrapper for all of this is not an attractive undertaking as part of your application development.

To further complicate things there are actually multiple versions of VSS depending on what version of Microsoft Windows you are running and for which platform it is targeted. Most notably Windows XP and Windows Server 2003 requires compiling and linking your application against the Shadow Copy Service SDK 7.2, with separate header files and libraries depending on the version of Windows you are targeting. From Windows Vista and onwards things are simpler though with all the functionality available in the standard Windows SDK.

AlphaVSS was created as a solution to these complexities, and to be as simple to include in your application as any other .NET class library, but without simplifying the functionality to the point where you as a developer don’t have access to the functionality you need.
Differences from the VSS API

AlphaVSS duplicates the original VSS API quite closely, but some things have been changed to conform more to the “.NET way” of doing things. The following is a brief summary of some of the differences:

* Most of the interfaces from the VSS API are exposed as classes in AlphaVSS, for example the C++ interface `IVssWMDependency` is represented as the .NET class `VssWMDependency`. Some abbreviations in identifiers are written out, for example `IVssSnapshotProp` has become `VssSnapshotProperties`
  
* Identifiers are written exclusively using CamelCase and rewritten to avoid abbreviations and unnecessary prefixes. For example the enumeration `VSSSNAPSHOTCONTEXT` is represented as `VssSnapshotContext`, `VSSCTXFILESHARE_BACKUP` became `FileShareBackup` and so on.

* Errors are handled by exceptions instead of return codes. AlphaVSS provides a full set of exception classes matching the various possible return codes from the native VSS API. For example the error code `VSS_E_LEGACY_PROVIDER` generates an exception of the type `VssLegacyProviderException`.

* Many methods of the VSS interfaces were changed to avoid out-parameters and to use standard .NET constructs instead of the more clumsy C++ constructs. Some methods have become properties, and enumerations of various kinds are represented as `IEnumerable<T>` or `IList<T>` instead of the more cumbersome versions used in the C++ interfaces. This should be clear from the API Reference documentation. 

## Limitations

AlphaVSS currently does not provide any support for creating writers or providers.


