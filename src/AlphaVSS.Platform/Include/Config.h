
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