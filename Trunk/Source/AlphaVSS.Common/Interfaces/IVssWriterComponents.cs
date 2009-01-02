using System;
using System.Collections.Generic;

namespace Alphaleonis.Win32.Vss
{
    /// <summary>
    /// 	The <see cref="IVssWriterComponents"/> interface contains methods used to obtain and modify component information 
    /// 	(in the form of <see cref="IVssComponent"/> instances) associated with a given writer but stored in a 
    /// 	requester's Backup Components Document.
    /// </summary>
    public interface IVssWriterComponents
    {
		/// <summary>
		/// 	A read-only collection of <see cref="IVssComponent"/> instances to the a given writer's 
		/// 	components explicitly stored in the Backup Components Document. 
		/// </summary>
		/// <value>A read-only collection of <see cref="IVssComponent"/> instances to the a given writer's 
		/// 	components explicitly stored in the Backup Components Document. <note type="caution">This list 
        /// 	must not be accessed after the <see cref="IVssComponent"/> from which it was obtained has been disposed.</note>
		/// </value>
		 IList<IVssComponent> Components { get; }

		/// <summary>
        ///     Identifier of the writer instance responsible for the components.
        /// </summary>
		 Guid InstanceId { get; }

		/// <summary>
        ///     Identifier of the writer class responsible for the components.
        /// </summary>
		 Guid WriterId { get; }
    }
}
