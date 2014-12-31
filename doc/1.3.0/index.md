---
layout: page
title: AlphaVSS 1.3.0
inMenu: false
toc: true
---
# About this document

This document is mainly provided as an API reference and usage guidance to the AlphaVSS API. It does not cover the operation of the Volume Shadow Copy Service, or how it should be used by developers.

To gain a full understanding of the Volume Shadow Copy Service you should refer to the [official VSS documentation on MSDN](http://msdn.microsoft.com/en-us/library/bb968832(VS.85).aspx). 

# Introduction

The Volume Shadow Copy Service (VSS) is a set of COM interfaces, delivered with various versions of Microsoft Windows, that implements a framework to allow volume backups to be performed while applications on a system continue to write to the volumes.

AlphaVSS is a .NET class library written in C++/CLI aiming to provide a managed interface to this API. The goal is to provide an interface that is simple to use from a C# or VB.NET application, yet providing the full functionality of VSS.

Using the Windows Volume Shadow Copy Service (VSS) on the .NET platform in C# (or VB) is somewhat problematic to say the least. There are numerous posts online about this issue, neither ever mentioning a robust solution that would allow full access to the VSS API from within a .NET application. The reasons are actually somewhat unclear, but it seems to have to do with there being COM interfaces without an IID, and also that several interfaces of the VSS API is not actually COM interfaces but rather C++ interfaces. This means there is no type library available for importing in your .NET application, and creating such a thing seems to not be possible. As mentioned there are several discussions on various forums available online on this topic so this will not be covered in more detail in this manual.

The only viable solution to this problem seems to be to write a custom wrapper in managed C++/CLI, that provides a managed interface to the VSS API that can be used from .NET and C#. The number of interfaces, structures and functions in the Volume Shadow Copy API is quite large however, and writing a complete wrapper for all of this is not an attractive undertaking as part of your application development.

AlphaVSS is a library released under the MIT license written to remedy this situation and allow easy access to the full VSS API from within a managed C# or VB.NET application targeting the Microsoft Windows operating system.

AlphaVSS duplicates the original VSS API quite closely, but some things have been changed to conform more to the .NET way of doing things. The following is a brief summary of some of the differences:

* Most of the interfaces from the VSS API are exposed as classes in AlphaVSS. So `IVssSnapshotProp` becomes `VssSnapshotProp` and so on.

* Identifiers are CamelCased and rewritten to avoid abbreviations and unneccessary prefixes. So eg. `VSSSNAPSHOTCONTEXT` becomes `VssSnapshotContext`, `VSSCTXFILESHARE_BACKUP` becomes `FileShareBackup` and so on.

* Errors are handled by exceptions and not return codes. AlphaVSS provides a full set of exception classes matching the various possible return codes from the native VSS API.

* Many methods of the interfaces are changed to avoid out parameters and to use standard .NET constructs instead of the more clumsy C++ constructs. This should be clear from the API Reference documentation.

## Limitations

AlphaVSS currently does not provide any support for creating writers or providers .

# Getting Started
  
This section contains some basic information to get started developing an application using AlphaVSS.

## About the files and assemblies

There are three assemblies included with AlphaVSS:

**AlphaVSS.Common.dll**
: The platform-independent assembly that you must add a reference to from your application. This assembly contains all platform-independent classes along with the interfaces for the platform specific code and factories to create instances of them. 

**AlphaVSS.x86.dll**
: Assembly containing the actual implementation of the platform specific interfaces for 32-bit Windows. 

**AlphaVSS.x64.dll**
: Assembly containing the actual implementation of the platform specific interfaces for 64-bit Windows. 

Included are both Debug and Release builds of the above assemblies found under the directories with the corresponding names. 

The reason for requiring multiple assemblies are that the VSS API differs between platforms, and it is not possible to create a single assembly containing code for all of them. Instead the platform-independent assembly AlphaVSS.Common contains the code neccessary to load the platform specific assembly for the current platform on which the application is running.

All of the above assemblies should be distributed with your application if you want to support both x86 and x64 windows systems.

All of the assemblies are signed with a strong name, having the public key token 959d3993561034e3. The strong name are used to refer to the assemblies by the dynamic loading code in `VssUtils`. This is normally not something you need to be concerned with unless you are rebuilding the assemblies with another Public Key token or something similar.

 
## Accessing the main interface

The `VssUtils` class is the main static utility class for accessing the platform specific instances of the various VSS interfaces in a platform-independent manner.

The interface `IVssImplementation` is the main entry point to the AlphaVSS API, providing access to the rest of the platform specific interfaces and methods.

The simplest way to get started is to use `LoadImplementation` to allow AlphaVSS to automatically determine the current operating system, load the correct assembly (optionally into a specific AppDomain) and create an instance of the correct implementation of `IVssImplementation`.

If you have specific requirements on how the assembly should be loaded, or the instance created you are not required to use these methods but can use the methods in this class for accessing the suggested assembly name to load, and load it manually.

In this case you need to create an instance of the class called `Alphaleonis.Win32.Vss.VssImplementation` from the desired platform specific assembly. This class implements the `IVssImplementation` interface, and has a public parameterless constructor. 

## About `IVssImplementation`

Once you have an instance of `IVssImplementation` you have access to methods corresponding to the requester specific public functions of the Windows Volume Shadow Copy API. The most important of these are undoubtedly `CreateVssBackupComponents` corresponding to the [`CreateVssBackupComponents`](http://msdn.microsoft.com/en-us/library/aa381517(VS.85).aspx) function of the native VSS API. Call this method to retrieve an instance of the `IVssBackupComponents` interface (corresponding to the native VSS API [`IVssBackupComponents`](http://msdn.microsoft.com/en-us/library/aa382175(VS.85).aspx) interface.

Once you have created an instance of this interface you are ready to start using VSS. For more information refer to the [official VSS documentation from microsoft](http://msdn.microsoft.com/en-us/library/aa382175(VS.85).aspx), and the API Reference for AlphaVSS.

Note that most interfaces and classes created in AlphaVSS are disposable and should be disposed when no longer needed. Always create instances of these interfaces and classes in a using block to ensure they are correctly disposed. See the included samples for an example. 

# API Reference

The API reference can be found [here](api)