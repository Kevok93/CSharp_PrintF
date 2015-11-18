using PrintF;
using System;
using System.Linq;
public static class main {
	public static void Main(string[] args) {
		//if (args != null) 
		//	Console.Out.printf(args[0], args.Skip(1).ToArray());
		//else {
		//	Console.Out.Write("USAGE: printf {format} [arg1 [arg2...]]");
		//}
		//else Console.Out.printf("%-+010.5f %s %s!\n",3.14159265,"Hello","World");
		//new glibc_printf_test().TestMethod();
		Console.Out.printf("Pi = |%8.5e|\n", Math.PI);
	}
}