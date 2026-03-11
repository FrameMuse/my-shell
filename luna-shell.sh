#!/bin/bash
# Luna Shell Startup Script for Ubuntu Server with Wayland Support

set -e

# Get script directory (works with both bash and sh)
SCRIPT_DIR="$(cd "$(dirname "$0")" && pwd)"
BINARY="$SCRIPT_DIR/build/luna-shell"

# Ensure the binary exists
if [ ! -f "$BINARY" ]; then
    echo "Error: luna-shell binary not found at $BINARY"
    echo "Please build the project first: cd $SCRIPT_DIR && ./build.sh"
    exit 1
fi

# Colors for output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
NC='\033[0m' # No Color

echo -e "${GREEN}=== Luna Shell Startup ===${NC}"

# Check if running in a graphical environment
if [ -z "$DISPLAY" ] && [ -z "$WAYLAND_DISPLAY" ]; then
    echo -e "${YELLOW}No display detected. Configuring for server environment...${NC}"
    
    # Option 1: Use offscreen rendering (headless)
    if [ "$1" = "--offscreen" ] || [ "$1" = "-o" ]; then
        echo -e "${YELLOW}Using offscreen (headless) mode${NC}"
        export QT_QPA_PLATFORM=offscreen
        export QT_QPA_PLATFORM_PLUGIN_PATH=$(python3 -c "from PyQt5.QtCore import QLibraryInfo; print(QLibraryInfo.location(QLibraryInfo.PluginsPath))" 2>/dev/null || echo "")
        "$BINARY"
        exit $?
    fi
    
    # Option 2: Use VNC for remote display
    if [ "$1" = "--vnc" ] || [ "$1" = "-v" ]; then
        echo -e "${YELLOW}Starting with VNC server on port 5900${NC}"
        export QT_QPA_PLATFORM=vnc
        export QT_QPA_PLATFORM_PLUGIN_PATH=$(python3 -c "from PyQt5.QtCore import QLibraryInfo; print(QLibraryInfo.location(QLibraryInfo.PluginsPath))" 2>/dev/null || echo "")
        "$BINARY" -platform vnc
        exit $?
    fi
    
    # Option 3: Try eglfs for framebuffer rendering (best for Wayland needs)
    if [ "$1" = "--eglfs" ] || [ "$1" = "-e" ] || [ -z "$1" ]; then
        echo -e "${YELLOW}Configuring Wayland environment (eglfs rendering)...${NC}"
        
        # Set minimal Wayland configuration
        export XDG_RUNTIME_DIR="${XDG_RUNTIME_DIR:-/run/user/$(id -u)}"
        export WAYLAND_DISPLAY=wayland-0
        export QT_QPA_PLATFORM=eglfs  # Use EGLFS for framebuffer rendering
        
        # Create XDG_RUNTIME_DIR if it doesn't exist
        if [ ! -d "$XDG_RUNTIME_DIR" ]; then
            mkdir -p "$XDG_RUNTIME_DIR"
            chmod 700 "$XDG_RUNTIME_DIR"
        fi
        
        # Optional: Start weston in the background if available
        if command -v weston &> /dev/null; then
            echo -e "${YELLOW}Weston compositor available. Consider running in another terminal:${NC}"
            echo -e "${YELLOW}  weston --backend=headless-backend.so --socket=wayland-0 &${NC}"
        fi
        
        echo -e "${GREEN}Starting Luna Shell...${NC}"
        "$BINARY"
        exit $?
    fi
else
    echo -e "${GREEN}Display environment detected${NC}"
fi


echo -e "${GREEN}Setting QT_QPA_PLATFORM=eglfs"
export QT_QPA_PLATFORM=eglfs

# If we reach here, use the detected environment
echo -e "${GREEN}Starting Luna Shell with current environment...${NC}"
exec "$BINARY"
