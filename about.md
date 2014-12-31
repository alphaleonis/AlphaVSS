---
layout: page
title: About
inMenu: true
permalink: /about/
---
# Background

Using the Windows Volume Shadow Copy Service (VSS) on the .NET platform in C# (or VB) is somewhat problematic to say the least. There are numerous posts online about this issue, neither ever mentioning a robust solution that would allow full access to the VSS API from within a .NET application. The reasons are actually somewhat unclear, but it seems to have to do with there being COM interfaces without an IID, and also that several interfaces of the VSS API is not actually COM interfaces but rather C++ interfaces. This means there is no type library available for importing in your .NET application, and creating one from the existing DLL-files seems impossible.

The only viable solution to this problem appears to be to write a custom wrapper in managed C++/CLI, that provides a managed interface to the VSS API that can then be used from a .NET application. The sheer number of interfaces, structures and functions in the Volume Shadow Copy API are quite large, and writing a complete wrapper for all of this is not an attractive undertaking as part of your application development.

AlphaVSS was created as a solution to these complexities, and to be as simple to include in your application as any other .NET class library, but without simplifying the functionality to the point where you as a developer don’t have access to the functionality you need.