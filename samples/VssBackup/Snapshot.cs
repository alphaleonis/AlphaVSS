

#region Copyright Notice
/*
 * AlphaVSS Sample Code
 * Written by Jay Miller
 * 
 * This code is hereby released into the public domain, This applies
 * worldwide.
 */
#endregion

using System;
using Alphaleonis.Win32.Vss;

namespace VssSample
{
   /// <summary>
   /// Utility class to manage the snapshot's contents and ID.
   /// </summary>
   class Snapshot : IDisposable
   {
      /// <summary>A reference to the VSS context.</summary>
      IVssBackupComponents _backup;
      
      /// <summary>Metadata about this object's snapshot.</summary>
      VssSnapshotProperties _props;

      /// <summary>Identifier for the overall shadow copy.</summary>
      Guid _set_id;

      /// <summary>Identifier for our single snapshot volume.</summary>
      Guid _snap_id;

      /// <summary>
      /// Initializes a snapshot.  We save the GUID of this snap in order to
      /// refer to it elsewhere in the class.
      /// </summary>
      /// <param name="backup">A VssBackupComponents implementation for the current OS.</param>
      public Snapshot(IVssBackupComponents backup)
      {
         _backup = backup;
         _set_id = backup.StartSnapshotSet();
      }

      /// <summary>
      /// Dispose of the shadow copies created by this instance.
      /// </summary>
      public void Dispose()
      {
         try { Delete(); } catch { }
      }

      /// <summary>
      /// Adds a volume to the current snapshot.
      /// </summary>
      /// <param name="volumeName">Name of the volume to add (eg. "C:\").</param>
      /// <remarks>
      /// Note the IsVolumeSupported check prior to adding each volume.
      /// </remarks>
      public void AddVolume(string volumeName)
      {
         if (_backup.IsVolumeSupported(volumeName))
            _snap_id = _backup.AddToSnapshotSet(volumeName);
         else
            throw new VssVolumeNotSupportedException(volumeName);
      }

      /// <summary>
      /// Create the actual snapshot.  This process can take around 10s.
      /// </summary>
      public void Copy()
      {
         _backup.DoSnapshotSet();
      }

      /// <summary>
      /// Remove all snapshots.
      /// </summary>
      public void Delete()
      {
         _backup.DeleteSnapshotSet(_set_id, false);
      }

      /// <summary>
      /// Gets the string that identifies the root of this snapshot.
      /// </summary>
      public string Root
      {
         get
         {
            if (_props == null)
               _props = _backup.GetSnapshotProperties(_snap_id);
            return _props.SnapshotDeviceObject;
         }
      }
   }
}
