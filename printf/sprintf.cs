using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace PrintF {
	public static class SPrintF {
		public const string R_FORMAT_SPECIFIER_TYPE      = "([diuoxXfFeEgGaAcspn%])";
        public const string R_FORMAT_SPECIFIER_FLAGS     = "([-+ #0]*)";
        public const string R_FORMAT_SPECIFIER_WIDTH     = "([0-9*]*)";
        public const string R_FORMAT_SPECIFIER_LENGTH    = "(|hh|h|l|ll|j|z|t|L)";
        public const string R_FORMAT_SPECIFIER_PRECISION = "(|\\.[0-9*]+)";
		public const string R_FORMAT_SPECIFIER = "%"
			+ R_FORMAT_SPECIFIER_FLAGS
			+ R_FORMAT_SPECIFIER_WIDTH
			+ R_FORMAT_SPECIFIER_PRECISION
			+ R_FORMAT_SPECIFIER_LENGTH
			+ R_FORMAT_SPECIFIER_TYPE;

	    //No args, no work
	    public static string sprintf(String format) { return format; }

		public static string sprintf(String format, params Object[] args) {
		    string output = format;

		    //We need to replace tokens back-to-front, or else we change the tokens' absolute positions
		    Stack<FormatSpecification> replacements = new Stack<FormatSpecification>();

			Match m = Regex.Match(format, R_FORMAT_SPECIFIER);
			FormatSpecification formatSpecification;

			for (
			    int i = 0;
				m.Success && i < args.Length;
				m = m.NextMatch()
			) {
				formatSpecification = FormatSpecification.parseFormatSpecifier(m,format);
				if (formatSpecification.width     == -2) formatSpecification.width     = Convert.ToInt32(args[i++]);
				if (formatSpecification.precision == -2) formatSpecification.precision = Convert.ToInt32(args[i++]);

				if (formatSpecification.type != FormatSpecifierType.Percent)
					    ReplaceString.replacement_string(args[i++], formatSpecification);

				replacements.Push(formatSpecification);
			}


			while (replacements.Count > 0) {
			    formatSpecification   = replacements.Pop()                  ;
			    int    start          = formatSpecification.start           ;
				int    length         = formatSpecification.length          ;
				string replacement    = formatSpecification.replacement_str ;

				output = output.splice(replacement, start, length);
			}

			return output;
		}
		
		public static string splice(this string original, string replacement, int start, int length) {
		    int    end     = start + length;
		    int    end_len = original.Length - end;
		    string prefix  = original.Substring(   0 , start   );
			string postfix = original.Substring( end , end_len );
			return prefix + replacement + postfix;
		}

	    public static void printf(this System.IO.TextWriter output, String format, params Object[] args) {
	        output.Write(sprintf(format, args));
	    }
	}
}
