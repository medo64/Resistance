#~ .NET Project

.PHONY: default clean run test release docker publish
default: release

clean:                  # Clean temporary files
	@./Make.sh clean

run:                    # Run the project
	@./Make.sh run

test:                   # Run the tests
	@./Make.sh test

debug:                  # Build the debug version and place it into the bin folder
	@./Make.sh debug

release:                # Build the release version and place it into the bin folder
	@./Make.sh release

package:                 # Build packages
	@./Make.sh package

publish:                # Publish the packages
	@./Make.sh publish
