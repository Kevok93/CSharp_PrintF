
build_lib=printf/bin/Debug/printf.dll
build_exe=printf_runner/bin/Debug/printf_runner.exe
run_script=printf_runner/bin/Debug/run.sh
webserver=/var/www/html/resume/api/printf


all: $(webserver)

build: $(build_exe)

build_exe: $(build_exe)

build_lib: $(build_lib)

webserver: $(webserver)

run_script: $(run_script)

$(build_lib): printf/format_specifier.cs printf/Printf.cs printf/replacement_string.cs printf/sprintf.cs
	xbuild printf/printf.csproj
	touch printf/bin/Debug/printf.dll

$(build_exe): $(build_lib) printf_runner/glibc_printf_test.cs printf_runner/main.cs
	xbuild printf_runner/printf_runner.csproj
	touch printf_runner/bin/Debug/printf_runner.exe

$(run_script): 
	echo 'script_abs_loc=`readlink -f $$0`' >printf_runner/bin/Debug/run.sh
	echo 'script_abs_dir=`dirname $$script_abs_loc`' >>printf_runner/bin/Debug/run.sh
	echo '$$script_abs_dir/printf_runner.exe $$@' >>printf_runner/bin/Debug/run.sh

$(webserver): $(build_exe) $(run_script)
	ln -fs `readlink -f $(run_script)` /var/www/html/resume/api/printf
