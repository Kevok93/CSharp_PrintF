using System;
using System.Text.RegularExpressions;
namespace PrintF {
	public class ReplaceString {
	    Object o;
	    FormatSpecification format;
	    int precision ;
	    string prec_str ;
	    int width ;
	    string width_str ;
	    char pad ;
	    double exp;
	    string exp_string;

	    public ReplaceString(Object o, FormatSpecification format) {
	        this.o         = o;
            this.format    = format;
            this.precision = format.precision;
            this.prec_str  = precision == -1 ? "" : precision.ToString();
            this.width     = format.width;
            this.width_str = width == -1 ? "" : width.ToString();
            this.pad       = format.prefix_zero ? '0' : ' ';

	    }

	    public string asChar() { return Convert.ToChar (o).ToString(); }
	    public string asString() {
	        string output;
	        if (o == null) output = "(null)";
	        else output = o.ToString();
	        while (output.Length < width) {
	            if (format.left_justify) output = output + " ";
	            else output = " " + output;
	        }
	        if (precision > 0) output = output.Substring(0, precision);
	        return output;
	    }

	    public string asInt(bool signed) {
	        string output,prefix;
	        prefix = "";
	        if (signed) {
	            output = Math.Abs (Convert.ToInt32 (o)).ToString ("d");
	            prefix = sign (format, Convert.ToInt32 (o));
	        } else {
	            output = unchecked ((uint)Convert.ToInt64(o)).ToString("d");
	        }
	        return pad_str(output, prefix, format);
	    }

	    public string asOctal() {
	        string output,prefix;
	        prefix = format.hex_prefix ? "0" : "";
	        unchecked {
	            output = Convert.ToString(unchecked ((uint)Convert.ToInt64(o)), 8);
	        }
	        return pad_str(output, prefix, format);
	    }

	    public string asHex(bool upper) {
	        string output,prefix;
	        prefix = format.hex_prefix ? "0x" : "";
	        unchecked {
	            output = Convert.ToString(unchecked ((uint)Convert.ToInt64(o)), 16);
	        }
	        output = pad_str (output,prefix,format);
	        if (upper) output = output.ToUpper();
	        return output;
	    }

	    public string asFloat(bool upper) {
	        string output,prefix;
	        char f = upper ? 'F' : 'f';
	        prefix = sign(format, Convert.ToDouble(o));
	        output = Math.Abs(Convert.ToDouble(o)).ToString(f + prec_str);
	        output = prefix + output;
	        while (output.Length < width) output = pad + output;
	        return output;
	    }

	    public string asSci( bool upper) {
	        string output;
	        char e = upper ? 'E' : 'e';
	        output = Convert.ToDouble(o).ToString(e + prec_str);
	        //Just the raw exponent, so we can shrink the exponent (For compatibility with posix)
	        exp = Convert.ToDouble(output.Substring(output.Length-3));
	        exp_string = exp.ToString();
	        while (exp_string.Length < 2) exp_string = "0" + exp_string;
	        output = output.Substring(0, output.Length - 3) + exp_string;
	        return pad_str(output, "", format);
	    }

	    public string asAddress() {
	        string output;
	        output = ((IntPtr)Convert.ToInt64(o)).ToString("X8");
	        if (output.Equals("00000000")) output = "(nil)";
	        return pad_str(output, "", format);
	    }

	    public static void replacement_string(Object o, FormatSpecification format) {
			string output;
	        ReplaceString replace = new ReplaceString (o, format);
	        switch (format.type) {
	            case FormatSpecifierType.Char:
	                output = replace.asChar();
	                break;
	            case FormatSpecifierType.String:
	                output = replace.asString();
	                break;
	            case FormatSpecifierType.Signed_int:
	                output = replace.asInt (signed: true);
	                break;
	            case FormatSpecifierType.Unsigned_int:
	                output = replace.asInt (signed: false);
	                break;
	            case FormatSpecifierType.Unsigned_oct:
	                output = replace.asOctal();
	                break;
	            case FormatSpecifierType.Unsigned_hex:
	                output = replace.asHex (upper: false);
	                break;
	            case FormatSpecifierType.Unsigned_hex_upper:
	                output = replace.asHex (upper: true);
	                break;
	            case FormatSpecifierType.Float:
	                output = replace.asFloat (upper: false);
	                break;
	            case FormatSpecifierType.Float_upper:
	                output = replace.asFloat (upper: true);
	                break;
	            case FormatSpecifierType.Sci:
	                output = replace.asSci (upper: false);
	                break;
	            case FormatSpecifierType.Sci_upper:
	                output = replace.asSci (upper: true);
	                break;
	            case FormatSpecifierType.Min_sci_float: {
	                String float_s = replace.asFloat (upper: false);
	                String sci_s = replace.asSci (upper: false);
	                output = (float_s.Trim().Length < sci_s.Trim().Length) ? float_s : sci_s;
	            }
	                break;
	            case FormatSpecifierType.Min_sci_float_upper: {
	                String float_s = replace.asFloat (upper: true);
	                String sci_s = replace.asSci (upper: true);
	                output = (float_s.Length < sci_s.Length) ? float_s : sci_s;
	            }
	                break;
	            case FormatSpecifierType.Percent:
	                format.replacement_str = "%";
	                return;
	            case FormatSpecifierType.Pointer:
	                output = replace.asAddress();
	                break;
	            default:
	                output = "";
	                break;
	        }
	        format.replacement_str = output;
		}

		static string pad_str (string base_str, string prefix, FormatSpecification format) {
			int    width     = format.width                   ;
			int    precision = format.precision               ;
		    string output    = base_str                       ;
		    int    numWidth  = (precision > 0 && precision < width)
		        ? precision
		        : width;
			
			//TODO: %010ld
			 
			if (precision > 0)
			    while (output.Length < precision)
			        output = "0" + output;

			if (!format.left_justify) {
				if (format.prefix_zero) {
					while (output.Length + prefix.Length < numWidth)
					    output = '0' + output;
				}
			    output = prefix + output;
			    while (output.Length < width) {
			        output = ' ' + output;
			    }
			} else {
				while (output.Length + prefix.Length < width)
				    output = output + " ";
			    output = prefix + output;
			}
			return output;
		}

		static string sign (FormatSpecification format, double value) {
			if (value < 0) return "-";
			if (value > 0) {
				if (format.force_sign) return "+";
				if (format.positive_space) return " ";
			}
		    return "";
		}
	}
}
