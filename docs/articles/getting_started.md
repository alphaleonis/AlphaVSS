# Getting Started
  
This section contains some basic information to get started developing an application using AlphaVSS.

# Requirements

* Visual C++ 2019 Redistributables must be installed on the machine running any application using AlphaVSS.

# Installing

The easiest way to get started using AlphaVSS in a new project is to use the [AlphaVSS NuGet package](xref:AlphaVssNuGet).

This will add a reference to the *AlphaVSS.Common* assembly which contains the interfaces and classes that your code should directly access. When building 
the project, one or more (depending on your target platform) additional assemblies containing the platform specific implementation are copied to the output directory.
These are namely *AlphaVSS.x86.dll* and *AlphaVSS.x64.dll*, containing the 32-bit and 64-bit code respectively.  

# Deploying

The `AlphaVSS.x86.dll` and `AlphaVSS.x64.dll` assemblies must be deployed together with your application. Their location is dependent on the target framework of your 
application.  When deploying your application, ensure to include these in the same location relative to your application as they were put in when you built it,
unless you also provide your own [`IVssAssemblyResolver`](xref:Alphaleonis.Win32.Vss.IVssAssemblyResolver) that locates these from somewhere else (see below).

On .NET Framework these assemblies must be located in the same directory as `AlphaVSS.Common.dll` for the default assembly resolution to work.

On .NET Core the assemblies are located in a `runtimes\win-x86\native` and `runtimes\win-64\native` folders respectively. Along with these is a file 
called `Ijwhost.dll` which must also be distributed with your application. (This is needed to be able to execute C++/CLI code in .NET Core)


# Getting an instance of `IVssFactory`

The [`VssFactoryProvider`](xref:Alphaleonis.Win32.Vss.VssFactoryProvider) class is the main starting point of accessing AlphaVSS in a platform-independent (x86 vs x64) manner.  It has a default implementation in the static `Defualt` property from which you 
can retrieve an instance of `IVssFactory` which can then be used to create the other objects used for accessing the VSS subsystem.

## Default way
```cs 
    // Get the default factory...
    var vssFactory = VssFactoryProvider.Default.GetVssFactory();

    // ...then use that to create an instance of IVssBackupComponents.
    var vssBackupComponents = vssFactory.CreateVssBackupComponents();

    // ... use vssBackupComponents.
```

## Advanced usage

If you have specific requirements on how the implementation specific assemblies should be loaded, you can implement [`IVssAssemblyResolver`](xref:Alphaleonis.Win32.Vss.IVssAssemblyResolver) and pass that to a new instance of
`VssFactoryProvider` to load the platform specific assembly in the way that you want.

```cs
    class MyAssemblyResolver : IVssAssemblyResolver
    {
        public Assembly LoadAssembly(AssemblyName assemblyName)
        {
            // This is a silly implementation just to illustrate. Don't use this!
            if (assemblyName.Name == "AlphaVSS.x64.dll")
                return Assembly.LoadFile("C:\\AlphaVSS.x64.dll");
            else
                throw FileNotFoundException();
        }
    }

    var vssFactoryProvider = new VssFactoryProvider(new MyAssemblyResolver());

    // Get the factory...
    var vssFactory = VssFactoryProvider.Default.GetVssFactory();

    // ...then use that to create an instance of IVssBackupComponents.
    var vssBackupComponents = vssFactory.CreateVssBackupComponents();    
```    

## Troubleshooting

If you encounter problems when running `VssFactoryProvider.Default.GetVssFactory()` you can subscribe to the @System.Diagnostics.Trace class to get some logs detailing the process of locating and
loading the platform specific assemblies.

```cs
using System;
using System.Diagnostics;

class Test
{
    static void Main()
    {
       Trace.Listeners.Add(new TextWriterTraceListener(Console.Out));
       Trace.AutoFlush = true;

       // Logs detailing the assembly load process are now logged to the console.
       // The category of any logs emitted is "AlphaVSS"
       var vssFactory = VssFactoryProvider.Default.GetVssFactory();
    }
}
```


## Sample Projects

There are currently three sample projects available for AlphaVSS. They can be found at https://github.com/alphaleonis/AlphaVSS-Samples or the source downloaded from the 
AlphaVSS release page.

# Further Reading

Once you have an instance of `IVssFactory` you have access to methods corresponding to the requester specific public functions of the Windows Volume Shadow Copy API. The most important of these are undoubtedly `CreateVssBackupComponents` corresponding to the [`CreateVssBackupComponents`](https://docs.microsoft.com/en-us/windows/win32/api/vsbackup/nf-vsbackup-createvssbackupcomponents) function of the native VSS API. Call this method to retrieve an instance of the `IVssBackupComponents` interface (corresponding to the native VSS API [`IVssBackupComponents`](https://docs.microsoft.com/en-us/windows/win32/api/vsbackup/nl-vsbackup-ivssbackupcomponents) interface.

Once you have created an instance of this interface you are ready to start using VSS. For more information refer to the [official VSS documentation from microsoft](xref:VSS), and the [API Reference for AlphaVSS](../api).

Note that most interfaces and classes created in AlphaVSS are disposable and should be disposed when no longer needed. Always create instances of these interfaces and classes in a using block to ensure they are correctly disposed. See the included sample projects on [GitHub](https://github.com/alphaleonis/AlphaVSS) for an example. 
