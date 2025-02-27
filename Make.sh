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


PROJECT_NAME=$( cat "$SCRIPT_DIR/.meta" | grep -E "^PROJECT_NAME:" | sed  -n 1p | cut -d: -sf2 | xargs )
echo "${ANSI_PURPLE}Project name ...: ${ANSI_MAGENTA}$PROJECT_NAME${ANSI_RESET}"


GIT_VERSION=$( git tag --points-at HEAD | grep --color=always -E '^v[0-9]+\.[0-9]+\.[0-9]+$' | sed -n 1p | sed 's/^v//g' | xargs )
GIT_INDEX=$( git rev-list --count HEAD )
GIT_HASH=$( git log -n 1 --format=%h )

if [ "$GIT_VERSION" != "" ]; then
    echo "${ANSI_PURPLE}Tag version ....: ${ANSI_MAGENTA}$GIT_VERSION${ANSI_RESET}"
else
    echo "${ANSI_PURPLE}Tag version ....: ${ANSI_MAGENTA}-${ANSI_RESET}"
fi
echo "${ANSI_PURPLE}Revision .......: ${ANSI_MAGENTA}$GIT_HASH${ANSI_PURPLE} (${ANSI_MAGENTA}$GIT_INDEX${ANSI_PURPLE})${ANSI_RESET}"

PROJECT_START=$( cat "$SCRIPT_DIR/.meta" | grep -E "^PROJECT_START:" | sed  -n 1p | cut -d: -sf2 | xargs )
if [ -e "$SCRIPT_DIR/$PROJECT_START" ]; then
    echo "${ANSI_PURPLE}Start project ..: ${ANSI_MAGENTA}$PROJECT_START${ANSI_RESET}"
else
    echo "${ANSI_PURPLE}Start project ..: ${ANSI_RED}not found${ANSI_RESET}" >&2
    exit 113
fi

PROJECT_RUNTIMES=$( cat "$SCRIPT_DIR/.meta" | grep -E "^PROJECT_RUNTIMES:" | sed  -n 1p | cut -d: -sf2 | xargs )
if [ "$PROJECT_RUNTIMES" = "" ]; then
    PROJECT_RUNTIMES=current
fi
echo "${ANSI_PURPLE}Project runtimes: ${ANSI_MAGENTA}$PROJECT_RUNTIMES${ANSI_RESET}"

DOCKER_IMAGE=$( cat "$SCRIPT_DIR/.meta" | grep -E "^DOCKER_IMAGE:" | sed  -n 1p | cut -d: -sf2 | xargs )
if [ "$DOCKER_IMAGE" != "" ]; then
    DOCKER_IMAGE_ID=$( echo "$DOCKER_IMAGE" | cut -d/ -sf1 )
    DOCKER_IMAGE_NAME=$( echo "$DOCKER_IMAGE" | cut -d/ -f2 )
    if [ "$DOCKER_IMAGE_ID" != "" ] && [ "$DOCKER_IMAGE_NAME" != "" ]; then
        echo "${ANSI_PURPLE}Docker image ...: ${ANSI_MAGENTA}$DOCKER_IMAGE_ID/$DOCKER_IMAGE_NAME${ANSI_RESET}"
    else
        echo "${ANSI_PURPLE}Docker image ...: ${ANSI_RED}not found${ANSI_RESET}" >&2
        exit 113
    fi

    DOCKER_FILE="$(find "$SCRIPT_DIR/src" -type f -name "Dockerfile" -print | sed -n 1p)"
    if [ "$DOCKER_IMAGE_ID" != "" ] && [ "$DOCKER_IMAGE_NAME" != "" ]; then
        echo "${ANSI_PURPLE}Docker source ..: ${ANSI_MAGENTA}$DOCKER_FILE${ANSI_RESET}"
    else
        echo "${ANSI_PURPLE}Docker source ..: ${ANSI_RED}not found${ANSI_RESET}" >&2
        exit 113
    fi
fi


prereq_compile() {
    if ! command -v dotnet >/dev/null; then
        echo "${ANSI_RED}Missing dotnet command${ANSI_RESET}" >&2
        exit 113
    fi
}

prereq_package() {
    if [ "$DOCKER_IMAGE" != "" ]; then
        if ! command -v docker >/dev/null; then
            echo "${ANSI_RED}Missing docker command${ANSI_RESET}" >&2
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

    dotnet run --project "$SCRIPT_DIR/$PROJECT_START"
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

}

make_debug() {
    echo
    echo "${ANSI_MAGENTA}┏━━━━━━━┓${ANSI_RESET}"
    echo "${ANSI_MAGENTA}┃ DEBUG ┃${ANSI_RESET}"
    echo "${ANSI_MAGENTA}┗━━━━━━━┛${ANSI_RESET}"
    echo

    mkdir -p "$SCRIPT_DIR/bin"
    dotnet build "$SCRIPT_DIR/$PROJECT_START" --configuration Debug --output "$SCRIPT_DIR/bin"
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
            dotnet publish "$SCRIPT_DIR/$PROJECT_START"                               \
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
            dotnet publish "$SCRIPT_DIR/$PROJECT_START"                               \
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

    if [ "$DOCKER_IMAGE" != "" ]; then
        if [ "$GIT_VERSION" != "" ]; then
            docker build \
                -t $DOCKER_IMAGE_NAME:$GIT_VERSION \
                -t $DOCKER_IMAGE_NAME:latest \
                -t $DOCKER_IMAGE_NAME:unstable \
                -f "$DOCKER_FILE" . \
                && echo "${ANSI_CYAN}$DOCKER_IMAGE_NAME:$GIT_VERSION $DOCKER_IMAGE_NAME:latest $DOCKER_IMAGE_NAME:unstable${ANSI_RESET}"

            mkdir -p "$SCRIPT_DIR/dist"
            docker save \
                $DOCKER_IMAGE_NAME:$GIT_VERSION \
                | gzip > ./dist/$DOCKER_IMAGE_NAME.$GIT_VERSION.tgz \
                && echo "${ANSI_CYAN}dist/$DOCKER_IMAGE_NAME-$GIT_VERSION.tgz${ANSI_RESET}"
        else
            docker build \
                -t $DOCKER_IMAGE_NAME:unstable \
                -f "$DOCKER_FILE" . \
                && echo "${ANSI_CYAN}$DOCKER_IMAGE_NAME:unstable${ANSI_RESET}"
        fi
    else
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

    if [ "$DOCKER_IMAGE" != "" ]; then
        if [ "$GIT_VERSION" != "" ]; then
            docker tag \
                $DOCKER_IMAGE_NAME:$GIT_VERSION \
                $DOCKER_IMAGE_ID/$DOCKER_IMAGE_NAME:$GIT_VERSION
            docker push \
                $DOCKER_IMAGE_ID/$DOCKER_IMAGE_NAME:$GIT_VERSION \
            && echo "${ANSI_CYAN}$DOCKER_IMAGE_ID/$DOCKER_IMAGE_NAME:$GIT_VERSION${ANSI_RESET}"
            echo

            docker tag \
                $DOCKER_IMAGE_NAME:latest \
                $DOCKER_IMAGE_ID/$DOCKER_IMAGE_NAME:latest
            docker push \
                $DOCKER_IMAGE_ID/$DOCKER_IMAGE_NAME:latest \
            && echo "${ANSI_CYAN}$DOCKER_IMAGE_ID/$DOCKER_IMAGE_NAME:latest${ANSI_RESET}"
            echo
        fi

        docker tag \
            $DOCKER_IMAGE_NAME:unstable \
            $DOCKER_IMAGE_ID/$DOCKER_IMAGE_NAME:unstable
        docker push \
            $DOCKER_IMAGE_ID/$DOCKER_IMAGE_NAME:unstable \
            && echo "${ANSI_CYAN}$DOCKER_IMAGE_ID/$DOCKER_IMAGE_NAME:unstable${ANSI_RESET}"
    else
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
