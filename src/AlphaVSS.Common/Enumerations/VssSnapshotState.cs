

namespace Alphaleonis.Win32.Vss
{
   /// <summary>The <see cref="VssSnapshotState"/> enumeration is returned by a provider to specify the state of a given shadow copy operation.</summary>
   public enum VssSnapshotState
   {
      /// <summary><para>Reserved for system use.</para><para>Unknown shadow copy state.</para></summary>
      Unknown = 0x00,
      /// <summary><para>Reserved for system use.</para><para>Shadow copy is being prepared.</para></summary>
      Preparing = 0x01,
      /// <summary><para>Reserved for system use.</para><para>Processing of the shadow copy preparation is in progress.</para></summary>
      ProcessingPrepare = 0x02,
      /// <summary><para>Reserved for system use.</para><para>Shadow copy has been prepared.</para></summary>
      Prepared = 0x03,
      /// <summary><para>Reserved for system use.</para><para>Processing of the shadow copy precommit is in process.</para></summary>
      ProcessingPreCommit = 0x04,
      /// <summary><para>Reserved for system use.</para><para>Shadow copy is precommitted.</para></summary>
      PreComitted = 0x05,
      /// <summary><para>Reserved for system use.</para><para>Processing of the shadow copy commit is in process.</para></summary>
      ProcessingCommit = 0x06,
      /// <summary><para>Reserved for system use.</para><para>Shadow copy is committed.</para></summary>
      Committed = 0x07,
      /// <summary><para>Reserved for system use.</para><para>Processing of the shadow copy postcommit is in process.</para></summary>
      ProcessingPostCommit = 0x08,
      /// <summary><para>Reserved for system use.</para><para>Processing of the shadow copy file commit operation is underway.</para></summary>
      ProcessingPreFinalCommit = 0x09,
      /// <summary><para>Reserved for system use.</para><para>Processing of the shadow copy file commit operation is done.</para></summary>
      PreFinalCommitted = 0x0A,
      /// <summary><para>Reserved for system use.</para><para>Processing of the shadow copy following the final commit and prior to shadow copy create is underway.</para></summary>
      ProcessingPostFinalCommit = 0x0B,
      /// <summary><para>Shadow copy is created.</para></summary>
      Created = 0x0C,
      /// <summary><para>Reserved for system use.</para><para>Shadow copy creation is aborted.</para></summary>
      Aborted = 0x0D,
      /// <summary><para>Reserved for system use.</para><para>Shadow copy has been deleted.</para></summary>
      Deleted = 0x0E,
      /// <summary><para>Reserved value.</para></summary>
      PostCommitted = 0x0F,
   }
}
