# About this document

This document is mainly provided as an API reference and usage guidance to the AlphaVSS API. It does not cover the operation of the [Volume Shadow Copy Service](xref:VSS), or how it should be used by developers.

To gain a full understanding of the Volume Shadow Copy Service you should refer to the [official Microsoft documentation on VSS](xref:VSS). 

# Introduction

The [Volume Shadow Copy Service (VSS)](xref:VSS) is a set of COM interfaces included in Microsoft Windows, that implements a framework to allow volume backups to be performed while applications on a system continue to write to the volumes.

AlphaVSS is a .NET class library written in C++/CLI aiming to provide a managed interface to this API. The goal is to provide an interface that is simple to use from a C# or VB.NET application, yet providing the full functionality of VSS.

AlphaVSS duplicates the original VSS API quite closely, but some things have been changed to conform more to the .NET way of doing things. The following is a brief summary of some of the differences:

* Most of the interfaces from the VSS API are exposed as classes in AlphaVSS. So `IVssSnapshotProp` becomes @Alphaleonis.Win32.Vss.VssSnapshotProperties and so on.

* Identifiers are CamelCased and rewritten to avoid abbreviations and unneccessary prefixes. So eg. `VSSSNAPSHOTCONTEXT` becomes @Alphaleonis.Win32.Vss.VssSnapshotContext, `VSSCTXFILESHARE_BACKUP` becomes @Alphaleonis.Win32.Vss.VssSnapshotContext.FileShareBackup and so on.

* Errors are handled by exceptions and not return codes. AlphaVSS provides a full set of exception classes matching the various possible return codes from the native VSS API.

* Many methods of the interfaces are changed to avoid out parameters and to use standard .NET constructs instead of the more clumsy C++ constructs. This should be clear from the API Reference documentation.

## Limitations

AlphaVSS currently does not provide any support for creating writers or providers.

# Background

Using the [Windows Volume Shadow Copy Service (VSS)](xref:VSS) on the .NET platform in C# (or VB) is somewhat problematic. There are numerous posts online about this issue, neither ever mentioning a robust solution that would allow full access to the VSS API from within a .NET application.  The reason seems to be that the main VSS interfaces are not true COM interfaces but rather simple C++ interfaces. This means that there is no type library available to import into your .NET application, and creating one from the existing DLL-files seems impossible.

The only viable solution to this problem appears to be to write a custom wrapper in managed C++/CLI, that provides a managed interface to the VSS API that can then be used from a .NET application. The sheer number of interfaces, structures and functions in the Volume Shadow Copy API are quite large, and writing a complete wrapper for all of this is not an attractive undertaking as part of your application development.

AlphaVSS was created as a solution to these complexities, and to be as simple to include in your application as any other .NET class library, but without simplifying the functionality to the point where you as a developer don’t have access to the functionality you need.
