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

namespace Alphaleonis.Win32.Vss
{
   /// <summary>
   ///     The <see cref="VssSnapshotProperties"/> class contains the properties of a shadow copy or shadow copy set.
   /// </summary>
   [Serializable]
   public class VssSnapshotProperties
   {
      /// <summary>
      /// Initializes a new instance of the <see cref="VssSnapshotProperties"/> class.
      /// </summary>
      /// <param name="snapshotId">The snapshot id.</param>
      /// <param name="snapshotSetId">The snapshot set id.</param>
      /// <param name="snapshotCount">The snapshot count.</param>
      /// <param name="snapshotDeviceObject">The snapshot device object.</param>
      /// <param name="originalVolumeName">Name of the original volume.</param>
      /// <param name="originatingMachine">The originating machine.</param>
      /// <param name="serviceMachine">The service machine.</param>
      /// <param name="exposedName">Name of the exposed.</param>
      /// <param name="exposedPath">The exposed path.</param>
      /// <param name="providerId">The provider id.</param>
      /// <param name="snapshotAttributes">The snapshot attributes.</param>
      /// <param name="creationTimestamp">The creation timestamp.</param>
      /// <param name="snapshotState">State of the snapshot.</param>
      public VssSnapshotProperties(Guid snapshotId, Guid snapshotSetId, long snapshotCount, string snapshotDeviceObject,
          string originalVolumeName, string originatingMachine, string serviceMachine, string exposedName, string exposedPath,
          Guid providerId, VssVolumeSnapshotAttributes snapshotAttributes, DateTime creationTimestamp,
          VssSnapshotState snapshotState)
      {
         SnapshotId = snapshotId;
         SnapshotSetId = snapshotSetId;
         SnapshotsCount = snapshotCount;
         SnapshotDeviceObject = snapshotDeviceObject;
         OriginalVolumeName = originalVolumeName;
         OriginatingMachine = originatingMachine;
         ServiceMachine = serviceMachine;
         ExposedName = exposedName;
         ExposedPath = exposedPath;
         ProviderId = providerId;
         SnapshotAttributes = snapshotAttributes;
         CreationTimestamp = creationTimestamp;
         Status = snapshotState;
      }

      #region Public Properties

      /// <summary>A <see cref="Guid" /> uniquely identifying the shadow copy identifier.</summary>
      public Guid SnapshotId { get; private set; }

      /// <summary>A <see cref="Guid" /> uniquely identifying the shadow copy set containing the shadow copy.</summary>
      public Guid SnapshotSetId { get; private set; }

      /// <summary>
      /// 	<para>
      /// 		Number of volumes included with the shadow copy in the shadow copy set when it was created. 
      /// 		Because it is possible for applications to release individual shadow copies without releasing the shadow copy 
      /// 		set, at any given time the number of shadow copies in the shadow copy set may be less than <see cref="SnapshotsCount" />
      /// 	</para>
      /// 	<para>
      /// 		The maximum number of shadow-copied volumes permitted in a shadow copy set is 64.
      /// 	</para>
      /// </summary>
      public long SnapshotsCount { get; private set; }

      /// <summary>
      /// 	<para>
      /// 		The name of the device object for the shadow copy of the volume. The device object can be thought of as 
      /// 		the root of a shadow copy of a volume. Requesters will use this device name when accessing files on a 
      /// 		shadow-copied volume that it needs to work with.
      /// 	</para>
      /// 	<para>
      /// 		The device name does not contain a trailing "\".
      /// 	</para>
      /// </summary>
      public string SnapshotDeviceObject { get; private set; }

      /// <summary>The name of the volume that had been shadow copied.</summary>
      public string OriginalVolumeName { get; private set; }

      /// <summary>The name of the machine containing the original volume.</summary>
      public string OriginatingMachine { get; private set; }

      /// <summary>The name of the machine running the Volume Shadow Copy Service that created the shadow copy.</summary>
      public string ServiceMachine { get; private set; }

      /// <summary>The name of the shadow copy when it is exposed. This is a drive letter or mount point (if the shadow copy is exposed as a local volume), or a share name. </summary>
      public string ExposedName { get; private set; }

      /// <summary>The portion of the shadow copy of a volume made available if it is exposed as a share.</summary>
      public string ExposedPath { get; private set; }

      /// <summary>A <see cref="Guid"/> uniquely identifying the provider used to create this shadow copy.</summary>
      public Guid ProviderId { get; private set; }

      /// <summary>
      ///		The attributes of the shadow copy expressed as a bit mask (or bitwise OR) of members of 
      ///		the <see cref="VssVolumeSnapshotAttributes"/> enumeration.
      /// </summary>
      public VssVolumeSnapshotAttributes SnapshotAttributes { get; private set; }

      /// <summary>Time stamp indicating when the shadow copy was created. The exact time is determined by the provider.</summary>
      public DateTime CreationTimestamp { get; private set; }

      /// <summary>Current shadow copy creation status. See <see cref="VssSnapshotState"/>.</summary>
      public VssSnapshotState Status { get; private set; }

      #endregion
   };
}
