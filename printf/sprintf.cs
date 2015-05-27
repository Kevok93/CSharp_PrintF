using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace PrintF {
	public static class SPrintF {
		public const string R_FORMAT_SPECIFIER_TYPE = "([diuoxXfFeEgGaAcspn%])";
		public const string R_FORMAT_SPECIFIER_FLAGS = "([-+ #0]*)";
		public const string R_FORMAT_SPECIFIER_WIDTH = "([0-9*]*)";
		public const string R_FORMAT_SPECIFIER_LENGTH = "(|hh|h|l|ll|j|z|t|L)";
		public const string R_FORMAT_SPECIFIER_PRECISION = "(|\\.[0-9*]+)";
		public const string R_FORMAT_SPECIFIER = "%"
			+ R_FORMAT_SPECIFIER_FLAGS
			+ R_FORMAT_SPECIFIER_WIDTH
			+ R_FORMAT_SPECIFIER_PRECISION
			+ R_FORMAT_SPECIFIER_LENGTH
			+ R_FORMAT_SPECIFIER_TYPE;
		
		public static string sprintf(String format, params Object[] args) {
			Stack<FormatSpecifier> replacements = new Stack<FormatSpecifier>();
			if (args == null) args = new Object[]{null};
			replacements.Push(null);
			Match m = Regex.Match(format, R_FORMAT_SPECIFIER);
			int i = 0;
			FormatSpecifier format_specifier;
			for (; 
				m.Success && i < args.Length; 
				m = m.NextMatch()
			) {
				format_specifier = FormatSpecifier.parseFormatSpecifier(m,format);
				if (format_specifier.width == -2) format_specifier.width = Convert.ToInt32(args[i++]);
				if (format_specifier.precision == -2) format_specifier.precision = Convert.ToInt32(args[i++]);
				if (format_specifier.type != FormatSpecifierType.Percent)
					ReplaceString.replacement_string(args[i++], format_specifier);
				replacements.Push(format_specifier);
			}
			while ((format_specifier = replacements.Pop()) != null) {
				int start = format_specifier.start;
				int length = format_specifier.length;
				string replacement = format_specifier.replacement_str;
				string spliced_format = format.splice(replacement, start, length);
				format = spliced_format;
			}
			return format;
		}
		
		public static string splice(this string original, string replacement, int start, int length) {
			string prefix = original.Substring(0, start);
			string postfix = original.Substring(start + length, original.Length-(start+length));
			return prefix + replacement + postfix;
		}
	}
}
/*
		
			switch (format.type) {
				case FormatSpecifierType.Char:
				case FormatSpecifierType.String:
				case FormatSpecifierType.Pointer:
				case FormatSpecifierType.Percent:
				case FormatSpecifierType.Noop:
					break;
				default: //easier than manually specifying the other 20;
					
				
			}
*/