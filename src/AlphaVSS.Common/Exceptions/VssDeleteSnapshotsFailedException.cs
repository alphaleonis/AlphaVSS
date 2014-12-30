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
using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Alphaleonis.Win32.Vss
{

   /// <summary>
   ///		Exception thrown to indicate that the requested deletion of snapshots did not complete successfully.
   /// </summary>	
   /// <remarks>
   ///    To get further information about the cause of the error, check the inner exception which is populated with the 
   ///    original exception that caused the deletion to fail.
   /// </remarks>
   [Serializable]
   public sealed class VssDeleteSnapshotsFailedException : VssException
   {
      /// <summary>
      /// Initializes a new instance of the <see cref="VssDeleteSnapshotsFailedException"/> class.
      /// </summary>
      public VssDeleteSnapshotsFailedException()
         : this(Alphaleonis.Win32.Vss.Resources.LocalizedStrings.DeletionOfSnapshotsFailed)
      {
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="VssDeleteSnapshotsFailedException"/> class with the specified error message.
      /// </summary>
      /// <param name="message">The error message.</param>
      public VssDeleteSnapshotsFailedException(string message)
         : base(message)
      {
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="VssDeleteSnapshotsFailedException"/> class with the specified error message and a reference
      /// to the exception causing this exception to be thrown.
      /// </summary>
      /// <param name="message">The error message.</param>
      /// <param name="innerException">The inner exception.</param>
      public VssDeleteSnapshotsFailedException(string message, Exception innerException)
         : base(message, innerException)
      {
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="VssDeleteSnapshotsFailedException"/> class, specifying the number of 
      /// successfully deleted snapshots, the id of the snapshot on which the delete operation failed and the exception 
      /// causing the delete operation to fail.
      /// </summary>
      /// <param name="deletedSnapshotsCount">The number of successfully deleted snapshots.</param>
      /// <param name="nonDeletedSnapshotId">The id of the non deleted snapshot, or <see cref="Guid.Empty"/> if such information is not available.</param>
      /// <param name="innerException">The inner exception.</param>
      public VssDeleteSnapshotsFailedException(int deletedSnapshotsCount, Guid nonDeletedSnapshotId, Exception innerException)
         : base(Alphaleonis.Win32.Vss.Resources.LocalizedStrings.DeletionOfSnapshotFailedSeeInnerExceptionF, innerException)
      {
         m_deletedSnapshotsCount = deletedSnapshotsCount;
         m_nonDeletedSnapshotId = nonDeletedSnapshotId;
      }

      /// <summary>
      /// Gets the number of successfully deleted snapshots.
      /// </summary>
      /// <value>The number of successfully deleted snapshots.</value>
      public int DeletedSnapshotsCount { get { return m_deletedSnapshotsCount; } }

      /// <summary>
      /// Gets the non id of the snapshot that failed to be deleted.
      /// </summary>
      /// <value>The id of the snapshot that could not be deleted.</value>
      public Guid NonDeletedSnapshotId { get { return m_nonDeletedSnapshotId; } }

      /// <summary>
      /// Initializes a new instance of the <see cref="VssDeleteSnapshotsFailedException"/> class.
      /// </summary>
      /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
      /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"/> that contains contextual information about the source or destination.</param>
      /// <exception cref="T:System.ArgumentNullException">The <paramref name="info"/> parameter is <see langword="null"/>. </exception>
      /// <exception cref="T:System.Runtime.Serialization.SerializationException">The class name is <see langword="null"/> or <see cref="P:System.Exception.HResult"/> is zero (0). </exception>
      private VssDeleteSnapshotsFailedException(SerializationInfo info, StreamingContext context)
         : base(info, context)
      {
      }

      /// <summary>
      /// Sets the <see cref="T:System.Runtime.Serialization.SerializationInfo"/> with information about the exception.
      /// </summary>
      /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
      /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"/> that contains contextual information about the source or destination.</param>
      /// <exception cref="T:System.ArgumentNullException">The <paramref name="info"/> parameter is a null reference (Nothing in Visual Basic). </exception>
      /// <PermissionSet>
      /// 	<IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Read="*AllFiles*" PathDiscovery="*AllFiles*"/>
      /// 	<IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="SerializationFormatter"/>
      /// </PermissionSet>
      [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
      public override void GetObjectData(SerializationInfo info, StreamingContext context)
      {
         if (info == null)
            throw new ArgumentNullException("info");

         base.GetObjectData(info, context);

         info.AddValue("DeletedSnapshotsCount", m_deletedSnapshotsCount);
         info.AddValue("NonDeletedSnapshotId", m_nonDeletedSnapshotId);
      }

      private int m_deletedSnapshotsCount;
      private Guid m_nonDeletedSnapshotId;
   }
}
