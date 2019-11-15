// pch.h: This is a precompiled header file.
// Files listed below are compiled only once, improving build performance for future builds.
// This also affects IntelliSense performance, including code completion and many code browsing features.
// However, files listed here are ALL re-compiled if any one of them is updated between builds.
// Do not add files here that you will be updating frequently as this negates the performance advantage.

#ifndef PCH_H
#define PCH_H

// add headers that you want to pre-compile here


#pragma once

#include "version.h"
#include <windows.h>
#include <winbase.h>

#include <vss.h>
#include <vsWriter.h>
#include <vsBackup.h>

#include "Utils.h"
#include "Macros.h"
#include "Error.h"

#include "FactoryMethods.h"
#include "VssAsyncTaskFactory.h"

#include <atlbase.h>

#endif //PCH_H
