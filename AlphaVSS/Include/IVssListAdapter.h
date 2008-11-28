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

namespace Alphaleonis { namespace Win32 { namespace Vss
{
	/// <summary>
	/// 	<para>
	/// 		Adapter class providing the common <see cref="System::Collections::Generic::IList"/> interface for working
	/// 		with collections of objects returned by the various Vss classes.
	/// 	</para>
	/// </summary>
	/// <typeparam name="T">The element type of the list.</typeparam>
	/// <remarks>
	/// 	<para>
	/// 		Instances of this list are always read-only.
	/// 	</para>
	/// 	<note type="caution">
	/// 		A list of this type must not be accessed or used in any way after the object that returned
	/// 		it has been disposed. Doing so will result in <see cref="ObjectDisposedException"/> even though
	///			the list has not been explicitly disposed by the user.
	/// 	</note>
	/// </remarks>
	generic <typename T>
	public interface class IVssListAdapter : System::Collections::Generic::IList<T>
	{
	};

}}}