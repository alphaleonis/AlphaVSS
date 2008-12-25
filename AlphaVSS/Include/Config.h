/* Copyright (c) 2008 Peter Palotas
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

#define ALPHAVSS_TARGET_WINXP		0x0400
#define ALPHAVSS_TARGET_WIN2003		0x0500
#define ALPHAVSS_TARGET_WINVISTA	0x0600
#define ALPHAVSS_TARGET_WIN2008		0x0700

#ifndef ALPHAVSS_TARGET
#error "ALPHAVSS_TARGET must be defined"
#endif

#if ALPHAVSS_TARGET == ALPHAVSS_TARGET_WINXP

#define NTDDI_VERSION NTDDI_WINXP
#define _WIN32_WINNT _WIN32_WINNT_WINXP
#define WINVER _WIN32_WINNT

#elif ALPHAVSS_TARGET == ALPHAVSS_TARGET_WIN2003

#define NTDDI_VERSION NTDDI_WS03
#define _WIN32_WINNT _WIN32_WINNT_WS03
#define WINVER 0x501

#elif ALPHAVSS_TARGET == ALPHAVSS_TARGET_WINVISTA

#define NTDDI_VERSION NTDDI_VISTA
#define _WIN32_WINNT _WIN32_WINNT_VISTA
#define WINVER _WIN32_WINNT

#elif ALPHAVSS_TARGET == ALPHAVSS_TARGET_WIN2008

#define NTDDI_VERSION NTDDI_WS08
#define _WIN32_WINNT WIN32_WINNT_WS08
#define WINVER _WIN32_WINNT

#else
#error "ALPHAVSS_TARGET has unrecognized value"
#endif
