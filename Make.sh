#!/bin/sh
#~ .NET Project
SCRIPT_DIR="$( cd -- "$(dirname "$0")" >/dev/null 2>&1 ; pwd -P )"
SCRIPT_NAME=`basename $0`

if [ -t 1 ]; then
    ANSI_RESET="$(tput sgr0)"
    ANSI_RED="`[ $(tput colors) -ge 16 ] && tput setaf 9 || tput setaf 1 bold`"
    ANSI_YELLOW="`[ $(tput colors) -ge 16 ] && tput setaf 11 || tput setaf 3 bold`"
    ANSI_MAGENTA="`[ $(tput colors) -ge 16 ] && tput setaf 13 || tput setaf 5 bold`"
    ANSI_PURPLE="$(tput setaf 5)"
    ANSI_CYAN="`[ $(tput colors) -ge 16 ] && tput setaf 14 || tput setaf 6 bold`"
fi

if [ "$1" = "" ]; then ACTIONS="release"; else ACTIONS="$@"; fi


if ! [ -e "$SCRIPT_DIR/.meta" ]; then
    echo "${ANSI_RED}Meta file not found${ANSI_RESET}" >&2
    exit 113
fi

if ! command -v git >/dev/null; then
    echo "${ANSI_YELLOW}Missing git command${ANSI_RESET}"
fi


HAS_CHANGES=$( git status -s 2>/dev/null | wc -l )
if [ "$HAS_CHANGES" -gt 0 ]; then
    echo "${ANSI_YELLOW}Uncommitted changes present${ANSI_RESET}"
fi


PROJECT_NAME=$( cat "$SCRIPT_DIR/.meta" | grep -E "^PROJECT_NAME:" | sed  -n 1p | cut -d: -sf2- | xargs )
if [ "$PROJECT_NAME" = "" ]; then
    echo "${ANSI_PURPLE}Project name ........: ${ANSI_RED}not found${ANSI_RESET}"
    exit 113
fi
echo "${ANSI_PURPLE}Project name ........: ${ANSI_MAGENTA}$PROJECT_NAME${ANSI_RESET}"

GIT_VERSION=$( git tag --points-at HEAD | grep --color=always -E '^v[0-9]+\.[0-9]+\.[0-9]+$' | sed -n 1p | sed 's/^v//g' | xargs )
GIT_INDEX=$( git rev-list --count HEAD )
GIT_HASH=$( git log -n 1 --format=%h )

if [ "$GIT_VERSION" != "" ]; then
    echo "${ANSI_PURPLE}Git tag version .....: ${ANSI_MAGENTA}$GIT_VERSION${ANSI_RESET}"
else
    echo "${ANSI_PURPLE}Git tag version .....: ${ANSI_MAGENTA}-${ANSI_RESET}"
fi
echo "${ANSI_PURPLE}Git revision ........: ${ANSI_MAGENTA}$GIT_HASH${ANSI_PURPLE} (${ANSI_MAGENTA}$GIT_INDEX${ANSI_PURPLE})${ANSI_RESET}"

PROJECT_ENTRYPOINT=$( cat "$SCRIPT_DIR/.meta" | grep -E "^PROJECT_ENTRYPOINT:" | sed  -n 1p | cut -d: -sf2- | xargs )
if [ -e "$SCRIPT_DIR/$PROJECT_ENTRYPOINT" ]; then
    echo "${ANSI_PURPLE}Project entry point .: ${ANSI_MAGENTA}$PROJECT_ENTRYPOINT${ANSI_RESET}"
else
    echo "${ANSI_PURPLE}Project entry point .: ${ANSI_RED}not found${ANSI_RESET}" >&2
    exit 113
fi

PROJECT_RUNTIMES=$( cat "$SCRIPT_DIR/.meta" | grep -E "^PROJECT_RUNTIMES:" | sed  -n 1p | cut -d: -sf2- | xargs )
if [ "$PROJECT_RUNTIMES" = "" ]; then
    PROJECT_RUNTIMES=current
fi
echo "${ANSI_PURPLE}Project runtimes ....: ${ANSI_MAGENTA}$PROJECT_RUNTIMES${ANSI_RESET}"


DOCKER_FILE="$(find "$SCRIPT_DIR/src" -type f -name "Dockerfile" -print | sed -n 1p)"

PACKAGE_LINUX_DOCKER=$( cat "$SCRIPT_DIR/.meta" | grep -E "^PACKAGE_LINUX_DOCKER:" | sed  -n 1p | cut -d: -sf2- | xargs )
if [ "$PACKAGE_LINUX_DOCKER" = "" ] && [ "$DOCKER_FILE" != "" ]; then
    PACKAGE_LINUX_DOCKER=$PROJECT_NAME
fi
if [ "$PACKAGE_LINUX_DOCKER" != "" ]; then
    if [ "$DOCKER_FILE" != "" ]; then
        echo "${ANSI_PURPLE}Docker source .......: ${ANSI_MAGENTA}$DOCKER_FILE${ANSI_RESET}"
    else
        echo "${ANSI_PURPLE}Docker source .......: ${ANSI_RED}not found${ANSI_RESET}" >&2
        exit 113
    fi
    echo "${ANSI_PURPLE}Docker local image ..: ${ANSI_MAGENTA}$PACKAGE_LINUX_DOCKER${ANSI_RESET}"

    PUBLISH_LINUX_DOCKER=$( cat "$SCRIPT_DIR/.meta.private" 2>/dev/null | grep -E "^PUBLISH_LINUX_DOCKER:" | sed  -n 1p | cut -d: -sf2- | xargs )
    if [ "$PUBLISH_LINUX_DOCKER" != "" ]; then
        if [ "$PACKAGE_LINUX_DOCKER" = "" ]; then
            echo "${ANSI_PURPLE}Docker remote image .: ${ANSI_RED}not found${ANSI_RESET}" >&2
            exit 113
        fi

        DOCKER_IMAGE_ID=$( echo "$PUBLISH_LINUX_DOCKER" | cut -d/ -f1 )
        DOCKER_IMAGE_NAME=$( echo "$PUBLISH_LINUX_DOCKER" | cut -d/ -sf2 )
        if [ "$DOCKER_IMAGE_ID" != "" ] && [ "$DOCKER_IMAGE_NAME" = "" ]; then
            DOCKER_IMAGE_NAME="$PACKAGE_LINUX_DOCKER"
        fi
        if [ "$DOCKER_IMAGE_ID" != "" ] && [ "$DOCKER_IMAGE_NAME" != "" ]; then
            echo "${ANSI_PURPLE}Docker remote image .: ${ANSI_MAGENTA}$DOCKER_IMAGE_ID/$DOCKER_IMAGE_NAME${ANSI_RESET}"
        else
            echo "${ANSI_PURPLE}Docker remote image .: ${ANSI_RED}not found${ANSI_RESET}" >&2
            exit 113
        fi
    fi
fi


PACKAGE_LINUX_APPIMAGE=$( cat "$SCRIPT_DIR/.meta" | grep -E "^PACKAGE_LINUX_APPIMAGE:" | sed  -n 1p | cut -d: -sf2- | xargs )
if [ "$PACKAGE_LINUX_APPIMAGE" = "" ]; then  # auto-detect
    if [ -d "$SCRIPT_DIR/packaging/linux-appimage" ] && [ -d "$SCRIPT_DIR/packaging/linux-deb" ]; then
        PACKAGE_LINUX_APPIMAGE=$(basename "$SCRIPT_DIR/packaging/linux-deb/usr/share/applications"/*.desktop .desktop)
    fi
fi
if [ "$PACKAGE_LINUX_APPIMAGE" != "" ]; then
    echo "${ANSI_PURPLE}AppImage ............: ${ANSI_MAGENTA}$PACKAGE_LINUX_APPIMAGE${ANSI_RESET}"

    PUBLISH_LINUX_APPIMAGE=$( cat "$SCRIPT_DIR/.meta.private" 2>/dev/null | grep -E "^PUBLISH_LINUX_APPIMAGE:" | sed  -n 1p | cut -d: -sf2- | xargs )
    if [ "$PUBLISH_LINUX_APPIMAGE" = "" ]; then
        echo "${ANSI_PURPLE}AppImage remote .....: ${ANSI_MAGENTA}(not configured)${ANSI_RESET}" >&2
    else
        echo "${ANSI_PURPLE}AppImage remote .....: ${ANSI_MAGENTA}$PUBLISH_LINUX_APPIMAGE${ANSI_RESET}"
    fi
fi


PACKAGE_LINUX_DEB=$( cat "$SCRIPT_DIR/.meta" | grep -E "^PACKAGE_LINUX_DEB:" | sed  -n 1p | cut -d: -sf2- | xargs )
if [ "$PACKAGE_LINUX_DEB" = "" ]; then  # auto-detect
    if [ -d "$SCRIPT_DIR/packaging/linux-deb" ]; then
        PACKAGE_LINUX_DEB=$PROJECT_NAME
    fi
fi
if [ "$PACKAGE_LINUX_DEB" != "" ]; then
    echo "${ANSI_PURPLE}Debian package ......: ${ANSI_MAGENTA}$PACKAGE_LINUX_DEB${ANSI_RESET}"

    PUBLISH_LINUX_DEB=$( cat "$SCRIPT_DIR/.meta.private" 2>/dev/null | grep -E "^PUBLISH_LINUX_DEB:" | sed  -n 1p | cut -d: -sf2- | xargs )
    if [ "$PUBLISH_LINUX_DEB" = "" ]; then
        echo "${ANSI_PURPLE}Debian package remote: ${ANSI_MAGENTA}(not configured)${ANSI_RESET}" >&2
    else
        echo "${ANSI_PURPLE}Debian package remote: ${ANSI_MAGENTA}$PUBLISH_LINUX_APPIMAGE${ANSI_RESET}"
    fi
fi


prereq_compile() {
    if ! command -v dotnet >/dev/null; then
        echo "${ANSI_RED}Missing dotnet command${ANSI_RESET}" >&2
        exit 113
    fi
}

prereq_package() {
    if [ "$PACKAGE_LINUX_DOCKER" != "" ]; then
        if ! command -v docker >/dev/null; then
            echo "${ANSI_RED}Missing docker command${ANSI_RESET}" >&2
            exit 113
        fi
    fi

    if [ "$PACKAGE_LINUX_APPIMAGE" != "" ]; then
        if ! [ -d "$SCRIPT_DIR/packaging/linux-appimage" ]; then
            echo "${ANSI_RED}Missing linux-appimage directory${ANSI_RESET}" >&2
            exit 113
        fi
        if ! [ -d "$SCRIPT_DIR/packaging/linux-deb" ]; then
            echo "${ANSI_RED}Missing linux-deb directory${ANSI_RESET}" >&2
            exit 113
        fi
        if ! command -v appimagetool-x86_64.AppImage >/dev/null; then
            echo "${ANSI_RED}Missing appimagetool-x86_64.AppImage${ANSI_RESET}" >&2
            exit 113
        fi
    fi

    if [ "$PACKAGE_LINUX_DEB" != "" ]; then
        if ! [ -d "$SCRIPT_DIR/packaging/linux-deb" ]; then
            echo "${ANSI_RED}Missing linux-deb directory${ANSI_RESET}" >&2
            exit 113
        fi
        if ! [ -e "$SCRIPT_DIR/packaging/linux-deb/usr/share/applications"/*.desktop ]; then
            echo "${ANSI_RED}Missing desktip file${ANSI_RESET}" >&2
            exit 113
        fi
        if ! [ -e "$SCRIPT_DIR/packaging/linux-deb/usr/share/icons/hicolor/128x128/apps"/*.png ]; then
            echo "${ANSI_RED}Missing icon files${ANSI_RESET}" >&2
            exit 113
        fi
        if ! command -v dpkg-deb >/dev/null; then
            echo "${ANSI_RED}Missing dpkg-deb command (dpkg-deb package)${ANSI_RESET}" >&2
            exit 113
        fi
        if ! command -v fakeroot >/dev/null; then
            echo "${ANSI_RED}Missing fakeroot command${ANSI_RESET}" >&2
            exit 113
        fi
        if ! command -v gzip >/dev/null; then
            echo "${ANSI_RED}Missing gzip command${ANSI_RESET}" >&2
            exit 113
        fi
        if ! command -v lintian >/dev/null; then
            echo "${ANSI_RED}Missing lintian command (lintian package)${ANSI_RESET}" >&2
            exit 113
        fi
        if ! command -v strip >/dev/null; then
            echo "${ANSI_RED}Missing strip command${ANSI_RESET}" >&2
            exit 113
        fi
    fi
}

make_clean() {
    echo
    echo "${ANSI_MAGENTA}┏━━━━━━━┓${ANSI_RESET}"
    echo "${ANSI_MAGENTA}┃ CLEAN ┃${ANSI_RESET}"
    echo "${ANSI_MAGENTA}┗━━━━━━━┛${ANSI_RESET}"

    mkdir -p "$SCRIPT_DIR/bin"
    find "$SCRIPT_DIR/bin" -mindepth 1 -delete
    find "$SCRIPT_DIR/src" -type d -name "bin" -exec rm -rf {} +
}

make_run() {
    echo
    echo "${ANSI_MAGENTA}┏━━━━━┓${ANSI_RESET}"
    echo "${ANSI_MAGENTA}┃ RUN ┃${ANSI_RESET}"
    echo "${ANSI_MAGENTA}┗━━━━━┛${ANSI_RESET}"
    echo

    dotnet run --project "$SCRIPT_DIR/$PROJECT_ENTRYPOINT"
}

make_test() {
    echo
    echo "${ANSI_MAGENTA}┏━━━━━━┓${ANSI_RESET}"
    echo "${ANSI_MAGENTA}┃ TEST ┃${ANSI_RESET}"
    echo "${ANSI_MAGENTA}┗━━━━━━┛${ANSI_RESET}"
    echo

    find "$SCRIPT_DIR/tests" -name "*.csproj" -print0 \
        | xargs -0 -I{} \
        dotnet test -l  "console;verbosity=detailed" {}
    echo
}

make_debug() {
    echo
    echo "${ANSI_MAGENTA}┏━━━━━━━┓${ANSI_RESET}"
    echo "${ANSI_MAGENTA}┃ DEBUG ┃${ANSI_RESET}"
    echo "${ANSI_MAGENTA}┗━━━━━━━┛${ANSI_RESET}"
    echo

    mkdir -p "$SCRIPT_DIR/bin"
    dotnet build "$SCRIPT_DIR/$PROJECT_ENTRYPOINT" --configuration Debug --output "$SCRIPT_DIR/bin"
}

make_release() {
    echo
    echo "${ANSI_MAGENTA}┏━━━━━━━━━┓${ANSI_RESET}"
    echo "${ANSI_MAGENTA}┃ RELEASE ┃${ANSI_RESET}"
    echo "${ANSI_MAGENTA}┗━━━━━━━━━┛${ANSI_RESET}"
    echo

    if [ "$GIT_VERSION" != "" ]; then
        ASSEMBLY_VERSION="$GIT_VERSION.$GIT_INDEX"
    else
        ASSEMBLY_VERSION="0.0.0.$GIT_INDEX"
    fi

    mkdir -p "$SCRIPT_DIR/bin"
    for RUNTIME in $PROJECT_RUNTIMES; do
        echo "${ANSI_MAGENTA}$RUNTIME${ANSI_RESET}"
        if [ "$RUNTIME" = "current" ]; then
            dotnet publish "$SCRIPT_DIR/$PROJECT_ENTRYPOINT"                          \
                --configuration Release --output "$SCRIPT_DIR/bin"                    \
                --self-contained true --use-current-runtime                           \
                -p:GenerateDocumentationFile=false                                    \
                -p:AssemblyVersion=$ASSEMBLY_VERSION -p:FileVersion=$ASSEMBLY_VERSION \
                -p:Version=$ASSEMBLY_VERSION+$GIT_HASH                                \
                -p:PublishReadyToRun=true -p:PublishSingleFile=true                   \
            && echo "${ANSI_CYAN}$SCRIPT_DIR/bin${ANSI_RESET}"
            echo
        else
            mkdir -p "$SCRIPT_DIR/bin/$RUNTIME"
            dotnet publish "$SCRIPT_DIR/$PROJECT_ENTRYPOINT"                          \
                --configuration Release --output "$SCRIPT_DIR/bin/$RUNTIME"           \
                --self-contained true --runtime $RUNTIME                              \
                -p:GenerateDocumentationFile=false                                    \
                -p:AssemblyVersion=$ASSEMBLY_VERSION -p:FileVersion=$ASSEMBLY_VERSION \
                -p:Version=$ASSEMBLY_VERSION+$GIT_HASH                                \
                -p:PublishReadyToRun=true -p:PublishSingleFile=true                   \
            && echo "${ANSI_CYAN}$SCRIPT_DIR/bin/$RUNTIME${ANSI_RESET}"
            echo
        fi
    done
}

make_package() {
    echo
    echo "${ANSI_MAGENTA}┏━━━━━━━━━┓${ANSI_RESET}"
    echo "${ANSI_MAGENTA}┃ PACKAGE ┃${ANSI_RESET}"
    echo "${ANSI_MAGENTA}┗━━━━━━━━━┛${ANSI_RESET}"
    echo

    ANYTHING_DONE=0

    if [ "$PACKAGE_LINUX_DOCKER" != "" ]; then
        ANYTHING_DONE=1
        echo "${ANSI_MAGENTA}docker${ANSI_RESET}"

        if [ "$GIT_VERSION" != "" ]; then
            docker build \
                -t $PACKAGE_LINUX_DOCKER:$GIT_VERSION \
                -t $PACKAGE_LINUX_DOCKER:latest \
                -t $PACKAGE_LINUX_DOCKER:unstable \
                -f "$DOCKER_FILE" .  || exit 113
            echo "${ANSI_CYAN}$PACKAGE_LINUX_DOCKER:$GIT_VERSION $PACKAGE_LINUX_DOCKER:latest $PACKAGE_LINUX_DOCKER:unstable${ANSI_RESET}"

            mkdir -p "$SCRIPT_DIR/dist"
            docker save \
                $PACKAGE_LINUX_DOCKER:$GIT_VERSION \
                | gzip > ./dist/$PACKAGE_LINUX_DOCKER.$GIT_VERSION.tgz || exit 113
            echo "${ANSI_CYAN}dist/$PACKAGE_LINUX_DOCKER-$GIT_VERSION.tgz${ANSI_RESET}"
        else
            docker build \
                -t $PACKAGE_LINUX_DOCKER:unstable \
                -f "$DOCKER_FILE" . || exit 113
            echo "${ANSI_CYAN}$PACKAGE_LINUX_DOCKER:unstable${ANSI_RESET}"
        fi
        echo
    fi

    if [ "$PACKAGE_LINUX_APPIMAGE" != "" ]; then
        ANYTHING_DONE=1
        echo "${ANSI_MAGENTA}appimage (linux-x64)${ANSI_RESET}"

        if [ "$GIT_VERSION" != "" ]; then
            APPIMAGE_NAME="$PROJECT_NAME-$GIT_VERSION.AppImage"
        else
            APPIMAGE_NAME="$PROJECT_NAME-0.0.0+$GIT_HASH.AppImage"
        fi

        mkdir -p "$SCRIPT_DIR/build/AppDir"
        find "$SCRIPT_DIR/build/AppDir" -mindepth 1 -delete

        cp "$SCRIPT_DIR/packaging/linux-appimage/AppRun" "$SCRIPT_DIR/build/AppDir/" || exit 113

        mkdir -p "$SCRIPT_DIR/build/AppDir/opt/$PROJECT_NAME"
        rsync -a "$SCRIPT_DIR/bin/linux-x64/" "$SCRIPT_DIR/build/AppDir/opt/$PROJECT_NAME/" || exit 113

        rsync -a "$SCRIPT_DIR/packaging/linux-deb/usr/" "$SCRIPT_DIR/build/AppDir/usr/" || exit 113

        cp "$SCRIPT_DIR/packaging/linux-deb/usr/share/applications"/*.desktop "$SCRIPT_DIR/build/AppDir/" || exit 113
        cp "$SCRIPT_DIR/packaging/linux-deb/usr/share/icons/hicolor/128x128/apps"/*.png "$SCRIPT_DIR/build/AppDir/" || exit 113
        cp "$SCRIPT_DIR/packaging/linux-deb/usr/share/icons/hicolor/128x128/apps"/*.png "$SCRIPT_DIR/build/AppDir/.DirIcon" || exit 113

        if [ -e "$SCRIPT_DIR/packaging/linux-deb/etc/" ]; then
            rsync -a "$SCRIPT_DIR/packaging/linux-deb/etc/" "$SCRIPT_DIR/build/AppDir/etc/" || exit 113
        fi

        mkdir -p "dist"
        rm "dist/$APPIMAGE_NAME" 2>/dev/null
        appimagetool-x86_64.AppImage "$SCRIPT_DIR/build/AppDir/" "dist/$APPIMAGE_NAME" || exit 113

        echo "${ANSI_CYAN}dist/$APPIMAGE_NAME${ANSI_RESET}"
        echo
    fi

    if [ "$PACKAGE_LINUX_DEB" != "" ]; then
        for RUNTIME in $PROJECT_RUNTIMES; do
            case $RUNTIME in
                linux-x64)   DEB_ARCHITECTURE=amd64 ;;
                linux-arm64) DEB_ARCHITECTURE=arm64 ;;
                *)           continue ;;
            esac

            ANYTHING_DONE=1
            echo "${ANSI_MAGENTA}deb ($RUNTIME: $DEB_ARCHITECTURE)${ANSI_RESET}"

            if [ "$GIT_VERSION" != "" ]; then
                DEB_VERSION=$GIT_VERSION
                DEB_PACKAGE_NAME="${PROJECT_NAME}_${GIT_VERSION}_${DEB_ARCHITECTURE}.deb"
            else
                DEB_VERSION=0.0.0
                DEB_PACKAGE_NAME="${PROJECT_NAME}_0.0.0+${GIT_HASH}_${DEB_ARCHITECTURE}.deb"
            fi

            mkdir -p "$SCRIPT_DIR/build/$DEB_PACKAGE_NAME"
            find "$SCRIPT_DIR/build/$DEB_PACKAGE_NAME/" -mindepth 1 -delete

            rsync -a "$SCRIPT_DIR/packaging/linux-deb/DEBIAN/" "$SCRIPT_DIR/build/$DEB_PACKAGE_NAME/DEBIAN/" || exit 113
            sed -i "s/<DEB_VERSION>/$DEB_VERSION/" "$SCRIPT_DIR/build/$DEB_PACKAGE_NAME/DEBIAN/control" || exit 113
            sed -i "s/<DEB_ARCHITECTURE>/amd64/" "$SCRIPT_DIR/build/$DEB_PACKAGE_NAME/DEBIAN/control" || exit 113

            rsync -a "$SCRIPT_DIR/packaging/linux-deb/usr/" "$SCRIPT_DIR/build/$DEB_PACKAGE_NAME/usr/" || exit 113

            mkdir -p  "$SCRIPT_DIR/build/$DEB_PACKAGE_NAME/opt/$PROJECT_NAME/"
            rsync -a "$SCRIPT_DIR/bin/linux-x64/" "$SCRIPT_DIR/build/$DEB_PACKAGE_NAME/opt/$PROJECT_NAME/" || exit 113

            if [ -e "$SCRIPT_DIR/packaging/linux-deb/copyright" ]; then
                mkdir -p "$SCRIPT_DIR/build/$DEB_PACKAGE_NAME/usr/share/doc/$PROJECT_NAME/"
                cp "$SCRIPT_DIR/packaging/linux-deb/copyright" "$SCRIPT_DIR/build/$DEB_PACKAGE_NAME/usr/share/doc/$PROJECT_NAME/copyright" || exit 113
            fi

            find "$SCRIPT_DIR/build/$DEB_PACKAGE_NAME/" -type d -exec chmod 755 {} + || exit 113
            find "$SCRIPT_DIR/build/$DEB_PACKAGE_NAME/" -type f -exec chmod 644 {} + || exit 113
            find "$SCRIPT_DIR/build/$DEB_PACKAGE_NAME/opt/" -type f -name "$PROJECT_NAME" -exec chmod 755 {} + || exit 113
            chmod 755 "$SCRIPT_DIR/build/$DEB_PACKAGE_NAME/DEBIAN"/config || exit 113
            chmod 755 "$SCRIPT_DIR/build/$DEB_PACKAGE_NAME/DEBIAN"/p*inst || exit 113
            chmod 755 "$SCRIPT_DIR/build/$DEB_PACKAGE_NAME/DEBIAN"/p*rm || exit 113

            fakeroot dpkg-deb -Z gzip --build "$SCRIPT_DIR/build/$DEB_PACKAGE_NAME/" > /dev/null || exit 113
            mv "$SCRIPT_DIR/build/$DEB_PACKAGE_NAME.deb" "dist/$DEB_PACKAGE_NAME.deb" || exit 113
            lintian --suppress-tags dir-or-file-in-opt,embedded-library "dist/$DEB_PACKAGE_NAME.deb"

            case $RUNTIME in
                linux-x64)   DEB_PACKAGE_AMD64=$DEB_PACKAGE_NAME.deb ;;
                linux-arm64) DEB_PACKAGE_ARM64=$DEB_PACKAGE_NAME.deb ;;
                *)           continue ;;
            esac

            echo "${ANSI_CYAN}dist/$DEB_PACKAGE_NAME.deb${ANSI_RESET}"
            echo
        done
    fi

    if [ "$ANYTHING_DONE" -eq 0 ]; then
        echo "${ANSI_RED}Nothing to package${ANSI_RESET}" >&2
        exit 113
    fi
}

make_publish() {
    echo
    echo "${ANSI_MAGENTA}┏━━━━━━━━━┓${ANSI_RESET}"
    echo "${ANSI_MAGENTA}┃ PUBLISH ┃${ANSI_RESET}"
    echo "${ANSI_MAGENTA}┗━━━━━━━━━┛${ANSI_RESET}"
    echo

    ANYTHING_DONE=0

    if [ "$PUBLISH_LINUX_DOCKER" != "" ]; then
        ANYTHING_DONE=1
        echo "${ANSI_MAGENTA}docker${ANSI_RESET}"

        if [ "$GIT_VERSION" != "" ]; then
            docker tag \
                $PACKAGE_LINUX_DOCKER:$GIT_VERSION \
                $DOCKER_IMAGE_ID/$DOCKER_IMAGE_NAME:$GIT_VERSION || exit 113
            docker push \
                $DOCKER_IMAGE_ID/$DOCKER_IMAGE_NAME:$GIT_VERSION || exit 113
            echo "${ANSI_CYAN}$DOCKER_IMAGE_ID/$DOCKER_IMAGE_NAME:$GIT_VERSION${ANSI_RESET}"
            echo

            docker tag \
                $PACKAGE_LINUX_DOCKER:latest \
                $DOCKER_IMAGE_ID/$DOCKER_IMAGE_NAME:latest || exit 113
            docker push \
                $DOCKER_IMAGE_ID/$DOCKER_IMAGE_NAME:latest || exit 113
            echo "${ANSI_CYAN}$DOCKER_IMAGE_ID/$DOCKER_IMAGE_NAME:latest${ANSI_RESET}"
            echo
        fi

        docker tag \
            $PACKAGE_LINUX_DOCKER:unstable \
            $DOCKER_IMAGE_ID/$DOCKER_IMAGE_NAME:unstable || exit 113
        docker push \
            $DOCKER_IMAGE_ID/$DOCKER_IMAGE_NAME:unstable || exit 113
            echo "${ANSI_CYAN}$DOCKER_IMAGE_ID/$DOCKER_IMAGE_NAME:unstable${ANSI_RESET}"
        echo
    fi

    if [ "$PUBLISH_LINUX_APPIMAGE" != "" ]; then
        ANYTHING_DONE=1
        echo "${ANSI_MAGENTA}appimage (linux-x64)${ANSI_RESET}"
        rsync --no-g --no-o --progress "dist/$APPIMAGE_NAME" $PUBLISH_LINUX_APPIMAGE || exit 113
        echo "${ANSI_CYAN}$PUBLISH_LINUX_APPIMAGE${ANSI_RESET}"
        echo
    fi

    if [ "$PUBLISH_LINUX_DEB" != "" ]; then
        for RUNTIME in $PROJECT_RUNTIMES; do
            case $RUNTIME in
                linux-x64)   DEB_ARCHITECTURE=amd64 ; DEB_PACKAGE_CURR=$DEB_PACKAGE_AMD64 ;;
                linux-arm64) DEB_ARCHITECTURE=arm64 ; DEB_PACKAGE_CURR=$DEB_PACKAGE_ARM64 ;;
                *)           continue ;;
            esac

            ANYTHING_DONE=1
            echo "${ANSI_MAGENTA}deb ($RUNTIME: $DEB_ARCHITECTURE)${ANSI_RESET}"

            PUBLISH_LINUX_DEB_CURR="$( echo "$PUBLISH_LINUX_DEB" | sed "s/<DEB_ARCHITECTURE>/$DEB_ARCHITECTURE/g" )"

            rsync --no-g --no-o --progress "dist/$DEB_PACKAGE_CURR" $PUBLISH_LINUX_DEB_CURR || exit 113
            echo "${ANSI_CYAN}$PUBLISH_LINUX_DEB_CURR${ANSI_RESET}"
            echo
        done
    fi

    if [ "$ANYTHING_DONE" -eq 0 ]; then
        echo "${ANSI_RED}Nothing to publish${ANSI_RESET}" >&2
        exit 113
    fi
}


for ACTION in $ACTIONS; do
    case $ACTION in
        clean)                                       make_clean                                                              || exit 113 ;;
        run)     prereq_compile &&                                              make_run                                     || exit 113 ;;
        test)    prereq_compile &&                                              make_test                                    || exit 113 ;;
        debug)   prereq_compile &&                   make_clean &&              make_debug                                   || exit 113 ;;
        release) prereq_compile &&                   make_clean && make_test && make_release                                 || exit 113 ;;
        package) prereq_compile && prereq_package && make_clean && make_test && make_release && make_package                 || exit 113 ;;
        publish) prereq_compile && prereq_package && make_clean && make_test && make_release && make_package && make_publish || exit 113 ;;

        *) echo "Unknown action $ACTION" >&2 || exit 113 ;;
    esac
done

exit 0
