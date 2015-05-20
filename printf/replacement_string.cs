using System;
using System.Text.RegularExpressions;
namespace PrintF {
	public static class ReplaceString {
		public static void replacement_string(Object o, FormatSpecifier format) {
			string temp;// = "";
			int prec = format.precision;
			int width = format.width;
			string prefix = "";
			switch (format.type) {
				case FormatSpecifierType.Char: 
					temp = Convert.ToChar(o).ToString();
					break;
				case FormatSpecifierType.String:
					temp = o.ToString();
					break;
				case FormatSpecifierType.Signed_int:
					prefix = sign(format, Convert.ToInt32(o));
					temp = Math.Abs(Convert.ToInt32(o)).ToString("d");
					while (temp.Length + prefix.Length < width) temp = "0" + temp;
					temp = prefix + temp;
					break;
				case FormatSpecifierType.Unsigned_int:
					temp = Convert.ToUInt32(o).ToString("d" + width);
					break;
				case FormatSpecifierType.Unsigned_oct:
					temp = Convert.ToString(Convert.ToInt64(o), 8);
					break;
				case FormatSpecifierType.Unsigned_hex:
					temp = Convert.ToString(Convert.ToInt64(o), 16);
					break;
				case FormatSpecifierType.Unsigned_hex_upper:
					temp = Convert.ToString(Convert.ToInt64(o), 16).ToUpper();
					break;
				case FormatSpecifierType.Float:
					prefix = sign(format, Convert.ToDouble(o));
					temp = Math.Abs(Convert.ToDouble(o)).ToString("f" + prec);
					while (temp.Length + prefix.Length < width) temp = "0" + temp;
					temp = prefix + temp;
					break;
				case FormatSpecifierType.Float_upper:
					prefix = sign(format, Convert.ToDouble(o));
					temp = Math.Abs(Convert.ToDouble(o)).ToString("F" + prec);
					while (temp.Length + prefix.Length < width) temp = "0" + temp;
					temp = prefix + temp;
					break;
				case FormatSpecifierType.Sci:
					temp = Convert.ToDouble(o).ToString("e" + prec);
					break;
				case FormatSpecifierType.Sci_upper:
					temp = Convert.ToDouble(o).ToString("E" + prec);
					break;
				case FormatSpecifierType.Min_sci_float:
					temp = Math.Abs(Convert.ToDouble(o)).ToString("g" + prec);
					break;
				case FormatSpecifierType.Min_sci_float_upper:
					temp = Math.Abs(Convert.ToDouble(o)).ToString("G" + prec);
					break;
				default:
					temp = "";
					break;
				case FormatSpecifierType.Percent:
					format.replacement_str = "%";
					return;
				case FormatSpecifierType.Pointer:
					temp = new IntPtr(Convert.ToInt64(o)).ToString();
					break;
			}
			format.replacement_str = temp;
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
