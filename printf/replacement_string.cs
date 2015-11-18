using System;
using System.Text.RegularExpressions;
namespace PrintF {
	public static class ReplaceString {
		public static void replacement_string(Object o, FormatSpecifier format) {
			string temp;
			string temp2;
			int precision = format.precision;
			string prec_str = (precision==-1 ? "" : precision.ToString());
			int width = format.width;
			string width_str = (width==-1 ? "" : width.ToString());
			string prefix = "";
			char pad = (format.prefix_zero ? '0' : ' ');
			double exp;
			string exp_string;
			switch (format.type) {
				case FormatSpecifierType.Char: 
					temp = Convert.ToChar(o).ToString();
					break;
				case FormatSpecifierType.String:
					if (o == null) 
						temp = "(null)";
					else temp = o.ToString();
					while (temp.Length < width) {
						if (format.left_justify) temp = temp + " ";
						else temp = " " + temp;
					}
					if (precision > 0) temp = temp.Substring(0, precision);
					break;
				case FormatSpecifierType.Signed_int:
					temp = Math.Abs(Convert.ToInt32(o)).ToString("d");
					prefix = sign(format, Convert.ToInt32(o));
					temp = pad_str(temp, prefix, format);
					break;
				case FormatSpecifierType.Unsigned_int:
					temp = unchecked ((uint)Convert.ToInt64(o)).ToString("d");
					temp = pad_str(temp, prefix, format);
					break;
				case FormatSpecifierType.Unsigned_oct:
					prefix = (format.hex_prefix ? "0" : "");
					unchecked {
						temp = Convert.ToString(unchecked ((uint)Convert.ToInt64(o)), 8);
					}
					temp = pad_str(temp, prefix, format);
					break;
				case FormatSpecifierType.Unsigned_hex:
					prefix = (format.hex_prefix ? "0x" : "");
					unchecked {
						temp = Convert.ToString(unchecked ((uint)Convert.ToInt64(o)), 16);
					}
					temp = pad_str(temp, prefix, format);
					break;
				case FormatSpecifierType.Unsigned_hex_upper:
					prefix = (format.hex_prefix ? "0X" : "");
					unchecked {
						temp = Convert.ToString(unchecked ((uint)Convert.ToInt64(o)), 16).ToUpper();
					}
					temp = pad_str(temp, prefix, format);
					break;
				case FormatSpecifierType.Float:
					prefix = sign(format, Convert.ToDouble(o));
					temp = Math.Abs(Convert.ToDouble(o)).ToString("f" + prec_str);
					while (temp.Length + prefix.Length < width) temp = pad + temp;
					temp = prefix + temp;
					break;
				case FormatSpecifierType.Float_upper:
					prefix = sign(format, Convert.ToDouble(o));
					temp = Math.Abs(Convert.ToDouble(o)).ToString("F" + prec_str);
					while (temp.Length + prefix.Length < width) temp = pad + temp;
					temp = prefix + temp;
					break;
				case FormatSpecifierType.Sci:
					temp = Convert.ToDouble(o).ToString("e" + prec_str);
					//Just the raw exponent, so we can shrink the exponent (For compatibility with posix)
					exp = Convert.ToDouble(temp.Substring(temp.Length-3)); 
					exp_string = exp.ToString();
					while (exp_string.Length < 2) exp_string = "0" + exp_string;
					temp = temp.Substring(0, temp.Length - 3) + exp_string;
					temp = pad_str(temp, prefix, format);
					break;
				case FormatSpecifierType.Sci_upper:
					temp = Convert.ToDouble(o).ToString("E" + prec_str);
					//Just the raw exponent, so we can shrink the exponent (For compatibility with posix)
					exp = Convert.ToDouble(temp.Substring(temp.Length-3)); 
					exp_string = exp.ToString();
					while (exp_string.Length < 2) exp_string = "0" + exp_string;
					temp = temp.Substring(0, temp.Length - 3) + exp_string;
					temp = pad_str(temp, prefix, format);
					break;
				case FormatSpecifierType.Min_sci_float:
					temp = Convert.ToDouble(o).ToString("e" + prec_str);
					//Just the raw exponent, so we can shrink the exponent (For compatibility with posix)
					exp = Convert.ToDouble(temp.Substring(temp.Length-3));
					if (exp < 4 || exp >= precision) { //Finish the exp notation
						exp_string = exp.ToString();
						while (exp_string.Length < 2) exp_string = 0 + exp_string;
						temp = temp.Substring(0, temp.Length - 3) + exp_string;
					} else { //Float will be better
						prefix = sign(format, Convert.ToDouble(o));
						temp = Math.Abs(Convert.ToDouble(o)).ToString("f" + prec_str);
						while (temp.Length + prefix.Length < width) temp = pad + temp;
						temp = prefix + temp;
					}
					temp = pad_str(temp, prefix, format);
					break;
				case FormatSpecifierType.Min_sci_float_upper:
					temp = Convert.ToDouble(o).ToString("E" + prec_str);
					//Just the raw exponent, so we can shrink the exponent (For compatibility with posix)
					exp = Convert.ToDouble(temp.Substring(temp.Length-3));
					if (exp < 4 || exp >= precision) { //Finish the exp notation
						exp_string = exp.ToString();
						while (exp_string.Length < 2) exp_string = 0 + exp_string;
						temp = temp.Substring(0, temp.Length - 3) + exp_string;
					} else { //Float will be better
						prefix = sign(format, Convert.ToDouble(o));
						temp = Math.Abs(Convert.ToDouble(o)).ToString("F" + prec_str);
						while (temp.Length + prefix.Length < width) temp = pad + temp;
						temp = prefix + temp;
					}
					temp = pad_str(temp, prefix, format);
					break;
				default:
					temp = "";
					break;
				case FormatSpecifierType.Percent:
					format.replacement_str = "%";
					return;
				case FormatSpecifierType.Pointer:
					temp = ((IntPtr)Convert.ToInt64(o)).ToString("X8");
					if (temp.Equals("00000000")) temp = "(nil)";
					temp = pad_str(temp, prefix, format);
					break;
			}
			format.replacement_str = temp;
		}
		static string pad_str (string base_str, string prefix, FormatSpecifier format) {
			int width = format.width;
			int precision = format.precision;
			char pad = (format.prefix_zero ? '0' : ' ');
			
			 //TODO: %010ld
			 
			 if (precision > 0) while (base_str.Length < precision) base_str = "0" + base_str;
			if (!format.left_justify) {
				if (format.prefix_zero) {
					while (base_str.Length + prefix.Length < width) base_str = pad + base_str;
					base_str = prefix + base_str;
				} else {
					base_str = prefix + base_str;
					while (base_str.Length < width) base_str = pad + base_str;
				}
			} else {
				while (base_str.Length + prefix.Length < width) base_str = base_str + " ";
				base_str = prefix + base_str;
			}
			return base_str;
		}
		static string sign (FormatSpecifier format, double value) {
			if (value < 0) return "-";
			if (value > 0) {
				if (format.force_sign) return "+";
				if (format.positive_space) return " ";
			} return "";
		}
	}
}
