
using System.IO;
using System;
using PrintF;
using NUnit.Framework;


[TestFixture]
public class glibc_printf_test
{
	const string shortstr = "Hi, Z.";
  	const string longstr = 
  			"Good morning, Doctor Chandra.  This is Hal.  "+
  			"I am ready for my first lesson today.";
  			
	static StringWriter report = new StringWriter();
	
	[	TestCase("0012",					"%.4x",		0x12, 					TestName="Precision"						)]
	[	TestCase("0012",					"%04x",		0x12, 					TestName="Zero"								)]
	[	TestCase("0012",					"%4.4x",	0x12, 					TestName="Width"							)]
	[	TestCase("0012",					"%04.4x",	0x12,					TestName="Zero Width"						)]
	[	TestCase(" 012",					"%4.3x",	0x12,					TestName="Width Precision"					)]
	[	TestCase(" 012",					"%04.3x",	0x12, 					TestName="Zero Width Precision"				)]
	[	TestCase("0012",					"%.*x",	4,	0x12,					TestName="Arbitrary Precision"				)]
	[	TestCase("0012",					"%0*x",	4,	0x12,					TestName="Zero Arbitrary Width"				)]
	[	TestCase("0012",					"%*.*x",	4,	4,	0x12,			TestName="Arbitrary Precision Width"		)]
	[	TestCase("0012",					"%0*.*x",	4,	4,	0x12,			TestName="Zero Arbitrary Precision Width"	)]
	[	TestCase("%b", 						"%b",								TestName="Bad Format"						)]
	[	TestCase("     (nil)", 				"%10p",		0,						TestName="Null Pointer"						)]
	[	TestCase("-2345", 					"%d",		-2345,					TestName="decimal negative"					)]
	[	TestCase("37777773327", 			"%o",		-2345,					TestName="octal negative"					)]
	[	TestCase("fffff6d7", 				"%x",		-2345,					TestName="hex negative"						)]
	[	TestCase("-123456", 				"%ld",		-123456L,				TestName="long decimal number"				)]
	[	TestCase("37777773327", 			"%lo",		-2345L,					TestName="long octal negative"				)]
	[	TestCase("4294843840", 				"%lu",		-123456L,				TestName="long unsigned decimal number"		)]
	[	TestCase("-000123456", 				"%010ld",	-123456L,				TestName="zero-padded LDN"					)]
	[	TestCase("-123456   ", 				"%-010ld",	-123456L,				TestName="left-adjusted ZLDN"				)]
	[	TestCase("   -123456", 				"%10ld",	-123456L,				TestName="space-padded LDN"					)]
	[	TestCase("-123456   ", 				"%-10ld",	-123456L,				TestName="left-adjusted SLDN"				)]
	[	TestCase("    Hi, Z.", 				"%010s",	shortstr,				TestName="zero-padded string"				)]
	[	TestCase("Hi, Z.    ", 				"%-010s",	shortstr,				TestName="left-adjusted Z string"			)]
	[	TestCase("    Hi, Z.", 				"%10s",		shortstr,				TestName="space-padded string"				)]
	[	TestCase("Hi, Z.    ", 				"%-10s",	shortstr,				TestName="left-adjusted S string"			)]
	[	TestCase("(null)", 					"%s",		null,					TestName="null string"						)]
	[	TestCase("Good morning, Doctor", 	"%.20s",	longstr,				TestName="limited string"					)]
	[	TestCase("1.234000e+01", 			"%e",		12.34,					TestName="e-style >= 1"						)]
	[	TestCase("1.234000e-01", 			"%e",		0.1234,					TestName="e-style >= .1"					)]
	[	TestCase("1.234000e-03", 			"%e",		0.001234,				TestName="e-style < .1"						)]
	[	TestCase("1.000000000000000e+20",	"%.15e",	1e20,					TestName="e-style big"						)]
	[	TestCase("1.000000e-01",		 	"%e",		0.1,					TestName="e-style == .1"					)]
	[	TestCase("12.340000", 				"%f",		12.34,					TestName="f-style >=  1"					)]
	[	TestCase("0.123400",	 			"%f",		0.1234,					TestName="f-style >= .1"					)]
	[	TestCase("0.001234", 				"%f",		0.001234,				TestName="f-style <  .1"					)]
	[	TestCase("12.34", 					"%g",		12.34,					TestName="g-style >=  1"					)]
	[	TestCase("0.1234", 					"%g",		0.1234,					TestName="g-style >= .1"					)]
	[	TestCase("0.001234", 				"%g",		0.001234,				TestName="g-style <  .1"					)]
	[	TestCase("100000000000000000000",	"%.40g",	1e20,					TestName="g-style big"						)]
	[	TestCase(" 0.10000",				" %6.5f",	.09999999,				TestName="Rounding"							)]
	[	TestCase(" 0.10000",				" %6.5f",	.1,						TestName="Not Rounding .1"					)]
	[	TestCase("x0.5000x",				"x%5.4fx",	.5,						TestName="Not Rounding .5"					)]
	[	TestCase("   4.94066e-324",			"%15.5e", 	4.940656458412465e-324, TestName="Really small number"				)]
	[	TestCase("1.234568e+06",			"%e",		1234567.8,				TestName="G consistency 1"					)]
	[	TestCase("1234567.800000", 			"%f",		1234567.8,				TestName="G consistency 2"					)]
	[	TestCase("1.23457e+06",				"%g",		1234567.8,				TestName="G consistency 3"					)]
	[	TestCase("123.456",					"%g",		123.456,				TestName="G consistency 4"					)]
	[	TestCase("1e+06",					"%g",		1000000.0,				TestName="G consistency 5"					)]
	[	TestCase("10",						"%g",		10.0,					TestName="G consistency 6"					)]
	[	TestCase("0.02",					"%g",		0.02,					TestName="G consistency 7"					)]
	public void sprintfTest(String expected, String format, params Object[] args) {
		string actual = SPrintF.sprintf(format, args);
  		AssertOrPrint(actual, expected);
	}
  	
	[SetUpFixture]
	public class teardown {
		[SetUp]
		public void clearReport() {
			glibc_printf_test.report = new StringWriter();
		}
		[TearDown]
		public void printReport() {
			Console.WriteLine(glibc_printf_test.report);
		}
	}
	
	[Test]
	public void entropy_test () {
		for(int i = 0; i < 50; i++) 
			AssertOrPrint("-1.70141173319264430e+038", SPrintF.sprintf("%.17e", float.MinValue / 2));
	}
	[Test]
	public void digital_test ([Range(0,15)] int testcase) {
		const int DEC = -123;
		const int INT = 255;
		const int UNS = (~0);
		string prefix = "%";
		if ((testcase & 8) == 0) prefix += "-";
		if ((testcase & 4) == 0) prefix += "+";
		if ((testcase & 2) == 0) prefix += "#";
		if ((testcase & 1) == 0) prefix += "0";
		string format = SPrintF.sprintf("|%s6d |%s6o |%s6x |%s6X |%s6u |", prefix, prefix, prefix, prefix, prefix);
		string output = SPrintF.sprintf(format, DEC, INT, INT, INT, UNS);
  		AssertOrPrint(digital_test_expected[testcase], output);
	}
	
	[Test]
	public void float_test ([Range(0,8)] int testcase) {
		string FORMAT = "|%12.4f|%12.4e|%12.4g|\n";
		double value = float_test_expected[testcase].Item2;
		string output = SPrintF.sprintf(FORMAT, value, value, value);
  		AssertOrPrint(float_test_expected[testcase].Item1, output);
	}
	
	public void AssertOrPrint ( string s1, string s2) {
  		try {
  			Assert.AreEqual(s2, s1);
  		} catch (AssertionException e) {
			report.printf("|%35.35s| != |%35.35s|\n", s1, s2);
			throw;
  		}
	}
	
	static readonly Tuple<string,double>[] float_test_expected = {
		new Tuple<string,double>("|      0.0000|  0.0000e+00|           0|",0.0),
		new Tuple<string,double>("|      1.0000|  1.0000e+00|           1|",1.0),
		new Tuple<string,double>("|     -1.0000| -1.0000e+00|          -1|",-1.0),
		new Tuple<string,double>("|    100.0000|  1.0000e+02|         100|",100.0),
		new Tuple<string,double>("|   1000.0000|  1.0000e+03|        1000|",1000.0),
		new Tuple<string,double>("|  10000.0000|  1.0000e+04|       1e+04|",10000.0),
		new Tuple<string,double>("|  12345.0000|  1.2345e+04|   1.234e+04|",12345.0),
		new Tuple<string,double>("| 100000.0000|  1.0000e+05|       1e+05|",100000.0),
		new Tuple<string,double>("| 123456.0000|  1.2346e+05|   1.235e+05|",123456.0),
	};
	
	static readonly string[] digital_test_expected = {
		"|-123   |0377   |0xff   |0XFF   |4294967295 |",
		"|-123   |0377   |0xff   |0XFF   |4294967295 |",
		"|-123   |377    |ff     |FF     |4294967295 |",
		"|-123   |377    |ff     |FF     |4294967295 |",
		"|-123   |0377   |0xff   |0XFF   |4294967295 |",
		"|-123   |0377   |0xff   |0XFF   |4294967295 |",
		"|-123   |377    |ff     |FF     |4294967295 |",
		"|-123   |377    |ff     |FF     |4294967295 |",
		"|-00123 |000377 |0x00ff |0X00FF |4294967295 |",
		"|  -123 |  0377 |  0xff |  0XFF |4294967295 |",
		"|-00123 |000377 |0000ff |0000FF |4294967295 |",
		"|  -123 |   377 |    ff |    FF |4294967295 |",
		"|-00123 |000377 |0x00ff |0X00FF |4294967295 |",
		"|  -123 |  0377 |  0xff |  0XFF |4294967295 |",
		"|-00123 |000377 |0000ff |0000FF |4294967295 |",
		"|  -123 |   377 |    ff |    FF |4294967295 |"
	};
	
}
