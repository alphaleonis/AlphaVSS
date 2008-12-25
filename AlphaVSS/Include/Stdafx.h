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

#include "Config.h"

#include <windows.h>
#include <winbase.h>

#include <vss.h>
#include <vsWriter.h>
#include <vsBackup.h>

#include "Utils.h"
#include "Macros.h"
#include "Exceptions/VssException.h"
#include "Error.h"
#include "OsInfo.h"

#include "Exceptions/VssInvalidXmlDocumentException.h"
#include "Exceptions/VssMaximumNumberOfVolumesReachedException.h"
#include "Exceptions/VssObjectNotFoundException.h"
#include "Exceptions/VssProviderVetoException.h"
#include "Exceptions/VssUnexpectedProviderError.h"
#include "Exceptions/VssMaximumNumberOfSnapshotsReachedException.h"
#include "Exceptions/VssProviderNotRegisteredException.h"
#include "Exceptions/VssVolumeNotSupportedException.h"
#include "Exceptions/VssVolumeNotSupportedByProviderException.h"
#include "Exceptions/VssUnexpectedWriterError.h"
#include "Exceptions/VssInsufficientStorageException.h"
#include "Exceptions/VssFlushWritesTimeoutException.h"
#include "Exceptions/VssHoldWritesTimeoutException.h"
#include "Exceptions/VssTransactionFreezeTimeoutException.h"
#include "Exceptions/VssRebootRequiredException.h"
#include "Exceptions/VssTransactionThawTimeoutException.h"
#include "Exceptions/VssRevertInProgressException.h"
#include "Exceptions/VssUnsupportedContextException.h"
#include "Exceptions/VssVolumeInUseException.h"
#include "Exceptions/VssSnapshotSetInProgressException.h"
#include "Exceptions/VssObjectAlreadyExistsException.h"
#include "Exceptions/VssWriterInfrastructureException.h"
