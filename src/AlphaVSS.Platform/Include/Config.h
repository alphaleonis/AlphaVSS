/* Copyright (c) 2008-2012 Peter Palotas
 *  
 *  Permission is hereby granted, free of charge, to any person obtaining a copy
 *  of this software and associated documentation files (the "Software"), to deal
 *  in the Software without restriction, including without limitation the rights
 *  to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 *  copies of the Software, and to permit persons to whom the Software is
 *  furnished to do so, subject to the following conditions:
 *  
 *  The above copyright notice and this permission notice shall be included in
 *  all copies or substantial portions of the Software.
 *  
 *  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 *  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 *  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 *  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 *  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 *  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 *  THE SOFTWARE.
 */
#pragma once

/*
VSS requesters, writers, and providers compiled for 		Will run on
--------------------------------------------------------------------------------------------------------------------------------------------------
Windows Server 2008 (64-bit) and Windows Vista (64-bit) 	Windows Server 2008 (64-bit) and Windows Vista (64-bit)

Windows Server 2008 (32-bit) and Windows Vista (32-bit) 	Windows Server 2008 (32-bit) and Windows Vista (32-bit)

Windows Server 2003 (64-bit) 								Windows Server 2008 (64-bit), Windows Vista (64-bit), and Windows Server 2003 (64-bit)

Windows Server 2003 (32-bit) 								Windows Server 2008 (32-bit), Windows Vista (32-bit), and Windows Server 2003 (32-bit)
Note  Requesters will also run on Windows Server 2003 (64-bit).

Windows XP 64-Bit Edition 									Windows Server 2003 (64-bit) and Windows XP 64-Bit Edition

Windows XP (32-bit) 										Windows XP (32-bit)
Note  Requesters will also run on Windows XP 64-Bit Edition.
*/


/*****************************************
TARGETS 
******************************************/
#define ALPHAVSS_TARGET_WINXP_X86		   0x0501
#define ALPHAVSS_TARGET_WIN2003     		0x0502
#define ALPHAVSS_TARGET_WINXP_X64         0x0502
#define ALPHAVSS_TARGET_WINVISTAORLATER   0x0601


#ifndef ALPHAVSS_TARGET
#error "ALPHAVSS_TARGET must be defined"
#endif

#if ALPHAVSS_TARGET == ALPHAVSS_TARGET_WINXP_X86

#define NTDDI_VERSION NTDDI_WINXPSP2
#define _WIN32_WINNT _WIN32_WINNT_WINXP
#define WINVER _WIN32_WINNT

#elif ALPHAVSS_TARGET == ALPHAVSS_TARGET_WIN2003 

#define NTDDI_VERSION NTDDI_WS03SP1
#define _WIN32_WINNT _WIN32_WINNT_WS03
#define WINVER 0x501

#elif ALPHAVSS_TARGET >= ALPHAVSS_TARGET_WINVISTAORLATER

#define NTDDI_VERSION NTDDI_WS08
#define _WIN32_WINNT _WIN32_WINNT_WS08
#define WINVER _WIN32_WINNT

#else
#error "ALPHAVSS_TARGET has unrecognized value"
#endif


/***********************************************
FEATURE SELECTION
************************************************/
//#if ALPHAVSS_TARGET >= ALPHAVSS_TARGET_WIN2003
////#define ALPHAVSS_HAS_EWMEX
////#define ALPHAVSS_HAS_BACKUPEX
////#define ALPHAVSS_HAS_SNAPSHOTMGMT
////#define ALPHAVSS_HAS_DIFFERENTIALSOFTWARESNAPSHOTMGMT
////#define ALPHAVSS_HAS_WMDEPENDENCY
//#endif
//
//#if ALPHAVSS_TARGET >= ALPHAVSS_TARGET_WINVISTAORLATER
////#define ALPHAVSS_HAS_EWMEX2
////#define ALPHAVSS_HAS_BACKUPEX2
////#define ALPHAVSS_HAS_COMPONENTEX
////#define ALPHAVSS_HAS_SNAPSHOTMGMT2
////#define ALPHAVSS_HAS_DIFFERENTIALSOFTWARESNAPSHOTMGMT2
////#define ALPHAVSS_HAS_DIFFERENTIALSOFTWARESNAPSHOTMGMT3
//#define ALPHAVSS_HAS_BACKUPEX3
//#define ALPHAVSS_HAS_COMPONENTEX2
//#endif

// TODO: Remove any hard operating system version configuration from OperatingSystemInfo class. Use only numbers, and named requirements.