all: clean build

build:
	@msbuild /p:Configuration=Debug /nologo /verbosity:quiet /property:GenerateFullPaths=true Astar.sln

test-watch:
	@ag -l | entr -s 'make test-raw | tap-notify | faucet'

test-vim:
	@make test-raw | tap-min

test:
	@mono --debug Astar.Tests/bin/Debug/Astar.Tests.exe | faucet

test-raw:
	@mono --debug Astar.Tests/bin/Debug/Astar.Tests.exe

clean:
	@find . -type d -name "bin" -o -name "obj" | xargs rm -fr

list:
	@$(MAKE) -pRrq -f $(lastword $(MAKEFILE_LIST)) : 2>/dev/null | awk -v RS= -F: '/^# File/,/^# Finished Make data base/ {if ($$1 !~ "^[#.]") {print $$1}}' | sort | egrep -v -e '^[^[:alnum:]]' -e '^$@$$'

.PHONY: all build test-watch test test-raw clean list
