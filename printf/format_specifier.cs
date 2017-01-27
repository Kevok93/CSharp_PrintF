using System;
using System.Text.RegularExpressions;
namespace PrintF {
	public class FormatSpecification {
		public int 
			start, 
			length,
			width, 
			precision;
			
		public string 
			specifier_str,
			replacement_str;
			
		public FormatSpecifierType 
			type;
			
		public bool 
			left_justify,
			force_sign,
			positive_space,
			hex_prefix,
			force_decimal,
			prefix_zero,
			var_width,
			var_precision;
		
		public static FormatSpecification parseFormatSpecifier(Match match, String format) {
			FormatSpecification temp = new FormatSpecification();
			temp.length = match.Length;
			temp.start = match.Index;
			temp.specifier_str = format.Substring(temp.start, temp.length);
			temp.replacement_str = "";
			temp.var_width = temp.var_precision = false;
			
			#region type
			switch (match.Groups[5].Value) {
				case "d":
				case "i":
					temp.type = FormatSpecifierType.Signed_int;
					break;
				case "u":
					temp.type = FormatSpecifierType.Unsigned_int;
					break;
				case "o":
					temp.type = FormatSpecifierType.Unsigned_oct;
					break;
				case "x":
					temp.type = FormatSpecifierType.Unsigned_hex;
					break;
				case "X":
					temp.type = FormatSpecifierType.Unsigned_hex_upper;
					break;
				case "f":
					temp.type = FormatSpecifierType.Float;
					break;
				case "F":
					temp.type = FormatSpecifierType.Float_upper;
					break;
				case "e":
					temp.type = FormatSpecifierType.Sci;
					break;
				case "E":
					temp.type = FormatSpecifierType.Sci_upper;
					break;
				case "g":
					temp.type = FormatSpecifierType.Min_sci_float;
					break;
				case "G":
					temp.type = FormatSpecifierType.Min_sci_float;
					break;
				case "a":
					temp.type = FormatSpecifierType.Float_hex;
					break;
				case "A":
					temp.type = FormatSpecifierType.Float_hex_upper;
					break;
				case "c":
					temp.type = FormatSpecifierType.Char;
					break;
				case "s":
					temp.type = FormatSpecifierType.String;
					break;
				case "p":
					temp.type = FormatSpecifierType.Pointer;
					break;
				case "n":
					temp.type = FormatSpecifierType.Noop;
					break;
				case "%":
					temp.type = FormatSpecifierType.Percent;
					temp.replacement_str = "%";
					break;
				default:
					throw new FormatException(match.Groups[4].Value + " is not a valid format specifier type.");
			}
			#endregion
			#region flags
			{
				string flags = match.Groups[1].Value;
				temp.left_justify = flags.Contains("-");
				temp.force_sign = flags.Contains("+");
				temp.positive_space = flags.Contains(" ");
				temp.force_decimal = flags.Contains("#");
				temp.hex_prefix = flags.Contains("#");
				temp.prefix_zero = flags.Contains("0");
			}
			#endregion
			#region width
			if (match.Groups[2].Value.Length > 0)
				if (match.Groups[2].Value.Equals("*")) temp.width = -2;
				else temp.width = Convert.ToInt32(match.Groups[2].Value);
			else
				temp.width = -1;
			#endregion
			#region precision
			if (match.Groups[3].Value.Length > 0)
				if (match.Groups[3].Value.TrimStart('.').Equals("*")) temp.precision = -2;
				else temp.precision = Convert.ToInt32(match.Groups[3].Value.TrimStart('.'));
			else
				temp.precision = -1;
			#endregion
			
			return temp;
		}
		
		public override String ToString() {
			return specifier_str;
		}
	}
	public enum FormatSpecifierType {
		Signed_int,
		Unsigned_int,
		Unsigned_oct,
		Unsigned_hex,
		Unsigned_hex_upper,
		Float,
		Float_upper,
		Sci,
		Sci_upper,
		Min_sci_float,
		Min_sci_float_upper,
		Float_hex,
		Float_hex_upper,
		Char,
		String,
		Pointer,
		Noop,
		Percent
	}
}
