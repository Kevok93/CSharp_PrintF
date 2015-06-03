all: webserver

build: printf_runner/bin/Debug/printf_runner.exe

build_exe: build

build_lib: printf/bin/Debug/printf.dll

webserver: /var/www/html/resume/api/printf

run_script: printf_runner/bin/Debug/run.sh

printf/bin/Debug/printf.dll: printf/format_specifier.cs printf/Printf.cs printf/replacement_string.cs printf/sprintf.cs
	xbuild printf/printf.csproj
	touch printf/bin/Debug/printf.dll

printf_runner/bin/Debug/printf_runner.exe: build_lib printf_runner/glibc_printf_test.cs printf_runner/main.cs
	xbuild printf_runner/printf_runner.csproj
	touch printf_runner/bin/Debug/printf_runner.exe

printf_runner/bin/Debug/run.sh: 
	echo 'script_abs_loc=`readlink -f $$0`' >printf_runner/bin/Debug/run.sh
	echo 'script_abs_dir=`dirname $$script_abs_loc`' >>printf_runner/bin/Debug/run.sh
	echo '$$script_abs_dir/printf_runner.exe $$@' >>printf_runner/bin/Debug/run.sh

/var/www/html/resume/api/printf: build run_script
	ln -fs `readlink -f printf_runner/bin/Debug/run.sh` /var/www/html/resume/api/printf
