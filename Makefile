#~ .NET Project

.SILENT:
.NOTPARALLEL:
.ONESHELL:

clean run test benchmark examples debug release package publish &:
	./Make.sh $(MAKECMDGOALS)
