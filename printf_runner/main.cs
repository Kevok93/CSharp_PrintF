using PrintF;
using System;
public static class main {
	public static void Main(string[] args) {
		//if (args != null && args.Length > 1) Console.Out.printf(args[0], args[1]);
		//else Console.Out.printf("%-+010.5f %s %s!\n",3.14159265,"Hello","World");
		new glibc_printf_test().TestMethod();
	}
}