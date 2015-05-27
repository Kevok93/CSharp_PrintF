
using System;
using System.Collections.Generic;

namespace PrintF
{
	/// <summary>
	/// Description of MyClass.
	/// </summary>
	public static class PrintF
	{
		public static void printf(this System.IO.TextWriter output, String format, params Object[] args) {
			output.Write(SPrintF.sprintf(format, args));
		}
	}
}