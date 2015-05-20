public static class main {
	public static void Main(string[] args) {
		if (args != null && args.Length > 1) System.Console.WriteLine(PrintF.SPrintF.sprintf(args[0], args[1]));
		else System.Console.WriteLine(PrintF.SPrintF.sprintf("%-+010.5f %s %s!",3.14159265,"Hello","World"));
	}
}