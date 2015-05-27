
using System.IO;
using System;
using PrintF;
using NUnit.Framework;


[TestFixture]
public class glibc_printf_test
{
	const int DEC = -123;
	const int INT = 255;
	const int UNS = (~0);
	[Test]
	public void TestMethod() {
		StringWriter output = new StringWriter();
		
		string shortstr = "Hi, Z.";
  		string longstr = 
  			"Good morning, Doctor Chandra.  This is Hal.  "+
  			"I am ready for my first lesson today.";
		fmtchk(output,"%.4x");
		fmtchk(output,"%04x");
		fmtchk(output,"%4.4x");
		fmtchk(output,"%04.4x");
		fmtchk(output,"%4.3x");
		fmtchk(output,"%04.3x");
		
		fmtst1chk(output,"%.*x");
		fmtst1chk(output,"%0*x");
		fmtst2chk(output,"%*.*x");
		fmtst2chk(output,"%0*.*x");
		output.printf("bad format:\t\"%b\"\n");
 		output.printf("nil pointer (padded):\t\"%10p\"\n", 0);  
 		output.printf("decimal negative:\t\"%d\"\n", -2345);
		output.printf("octal negative:\t\"%o\"\n", -2345);
		output.printf("hex negative:\t\"%x\"\n", -2345);
		output.printf("long decimal number:\t\"%ld\"\n", -123456L);
		output.printf("long octal negative:\t\"%lo\"\n", -2345L);
		output.printf("long unsigned decimal number:\t\"%lu\"\n", -123456L);
		output.printf("zero-padded LDN:\t\"%010ld\"\n", -123456L);
		output.printf("left-adjusted ZLDN:\t\"%-010ld\"\n", -123456L);
		output.printf("space-padded LDN:\t\"%10ld\"\n", -123456L);
		output.printf("left-adjusted SLDN:\t\"%-10ld\"\n", -123456L);
		
		output.printf("zero-padded string:\t\"%010s\"\n", shortstr);
		output.printf("left-adjusted Z string:\t\"%-010s\"\n", shortstr);
		output.printf("space-padded string:\t\"%10s\"\n", shortstr);
		output.printf("left-adjusted S string:\t\"%-10s\"\n", shortstr);
		output.printf("null string:\t\"%s\"\n", null);
		output.printf("limited string:\t\"%.22s\"\n", longstr);
		
		output.printf("e-style >= 1:\t\"%e\"\n", 12.34);
		output.printf("e-style >= .1:\t\"%e\"\n", 0.1234);
		output.printf("e-style < .1:\t\"%e\"\n", 0.001234);
		output.printf("e-style big:\t\"%.60e\"\n", 1e20);
		output.printf ("e-style == .1:\t\"%e\"\n", 0.1);
		output.printf("f-style >= 1:\t\"%f\"\n", 12.34);
		output.printf("f-style >= .1:\t\"%f\"\n", 0.1234);
		output.printf("f-style < .1:\t\"%f\"\n", 0.001234);
		output.printf("g-style >= 1:\t\"%g\"\n", 12.34);
		output.printf("g-style >= .1:\t\"%g\"\n", 0.1234);
		output.printf("g-style < .1:\t\"%g\"\n", 0.001234);
		output.printf("g-style big:\t\"%.60g\"\n", 1e20);
		
		output.printf (" %6.5f\n", .099999999860301614);
		output.printf (" %6.5f\n", .1);
		output.printf ("x%5.4fx\n", .5);
		
		output.printf ("%#03x\n", 1);
		{
			double d = float.MinValue;
			int niter = 17;
			
			while (niter-- != 0) output.printf ("%.17e\n", d / 2);
		}
		
		output.printf ("%15.5e\n", 4.9406564584124654e-324);
		
		string FORMAT = "|%12.4f|%12.4e|%12.4g|\n";
		output.printf (FORMAT, 0.0, 0.0, 0.0);
		output.printf (FORMAT, 1.0, 1.0, 1.0);
		output.printf (FORMAT, -1.0, -1.0, -1.0);
		output.printf (FORMAT, 100.0, 100.0, 100.0);
		output.printf (FORMAT, 1000.0, 1000.0, 1000.0);
		output.printf (FORMAT, 10000.0, 10000.0, 10000.0);
		output.printf (FORMAT, 12345.0, 12345.0, 12345.0);
		output.printf (FORMAT, 100000.0, 100000.0, 100000.0);
		output.printf (FORMAT, 123456.0, 123456.0, 123456.0);
		fp_test(output);
		output.printf ("%e should be 1.234568e+06\n", 1234567.8);
		output.printf ("%f should be 1234567.800000\n", 1234567.8);
		output.printf ("%g should be 1.23457e+06\n", 1234567.8);
		output.printf ("%g should be 123.456\n", 123.456);
		output.printf ("%g should be 1e+06\n", 1000000.0);
		output.printf ("%g should be 10\n", 10.0);
		output.printf ("%g should be 0.02\n", 0.02);
		output.printf ("printf (\"%%hhu\", %u) = %hhu\n", char.MaxValue + 2, char.MaxValue + 2);
		output.printf ("printf (\"%%hu\", %u) = %hu\n", short.MaxValue + 2, short.MaxValue + 2);
		output.printf ("printf (\"%%hhi\", %i) = %hhi\n", char.MaxValue + 2, char.MaxValue + 2);
		output.printf ("printf (\"%%hi\", %i) = %hi\n", short.MaxValue + 2, short.MaxValue + 2);
		
		output.Write ("--- Should be no further output. ---");
		Console.Out.WriteLine(output.ToString());
}
	
	public void fp_test (StringWriter output) {
	
		output.WriteLine("Formatted output test");
		output.printf("prefix  6d      6o      6x      6X      6u\n");
		for (int i = 0; i < 2; i++) {
			for (int j = 0; j < 2; j++) {
				for (int k = 0; k < 2; k++) {
					for (int l = 0; l < 2; l++) {
						string prefix = "%";
						string tp;
						if (i == 0) prefix += "-";
						if (j == 0) prefix += "+";
						if (k == 0) prefix += "#";
						if (l == 0) prefix += "0";
						output.printf("%5s |", prefix);
						tp = prefix + "6d |";
						output.printf(tp, DEC);
						tp = prefix + "6o |";
						output.printf(tp, INT);
						tp = prefix + "6x |";
						output.printf(tp, INT);
						tp = prefix + "6X |";
						output.printf(tp, INT);
						tp = prefix + "6u |";
						output.printf(tp, UNS);
						output.printf("\n");
					}
				}
			}
		}
		output.printf("%10s\n",null);
		output.printf("%-10s\n",null);
	}
	
	public void fmtchk (StringWriter output, string fmt) {
		output.Write(fmt);
		output.printf(":\t`");
		output.printf(fmt, 0x12);
		output.printf("'\n");
	}
	
	public void fmtst1chk (StringWriter output, string fmt) {
		output.Write(fmt);
		output.printf(":\t`");
		output.printf(fmt, 4, 0x12);
		output.printf("'\n");
	}
	
	public void fmtst2chk (StringWriter output, string fmt) {
		output.Write(fmt);
		output.printf(":\t`");
		output.printf(fmt, 4, 4, 0x12);
		output.printf("'\n");
	}
}
