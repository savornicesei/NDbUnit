/*
 * NDbUnit2
 * https://github.com/savornicesei/NDbUnit2
 * This source code is released under the Apache 2.0 License; see the accompanying license file.
 *
 */
using System;
using System.Runtime.Serialization;

namespace NDbUnit.Core
{
	/// <summary>
	/// The base class exception of all exceptions thrown by objects
	/// in NDbUnit.
	/// </summary>
	[Serializable]
	public class NDbUnitException : Exception
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="NDbUnitException"/> class.
		/// </summary>
		public NDbUnitException() : base()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NDbUnitException"/> class 
		/// with a specified error message.
		/// </summary>
		/// <param name="message"></param>
		public NDbUnitException(string message) : base(message)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NDbUnitException"/> class 
		/// with serialized data.
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		protected NDbUnitException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NDbUnitException"/> class 
		/// with the specified error message and a reference to the inner exception 
		/// that is the cause of this exception.
		/// </summary>
		/// <param name="message"></param>
		/// <param name="innerException"></param>
		public NDbUnitException(string message, Exception innerException) : base(message, innerException)
		{
		}
	}
}
