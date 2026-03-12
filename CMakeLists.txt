cmake_minimum_required(VERSION 3.16)
project(LunaShell VERSION 0.1 LANGUAGES CXX)

set(CMAKE_CXX_STANDARD 17)
set(CMAKE_CXX_STANDARD_REQUIRED ON)

# Enable automatic Qt tools
set(CMAKE_AUTOMOC ON)
set(CMAKE_AUTORCC ON)

# 1. Find the necessary Qt 6 components
find_package(Qt6 REQUIRED COMPONENTS Core Gui Qml WaylandCompositor)

# 2. Define the executable (Renamed to 'luna-shell' to avoid directory conflict)
qt_add_executable(luna-shell
    main.cpp
)

# 3. Register the QML module
qt_add_qml_module(luna-shell
    URI LunaShell
    VERSION 1.0
    QML_FILES Shell.qml
)

# 4. Link the libraries to the new 'luna-shell' target
target_link_libraries(luna-shell PRIVATE
    Qt6::Core
    Qt6::Gui
    Qt6::Qml
    Qt6::WaylandCompositor
)

# --------------------------------------------------
# optional TypeScript/JSX frontend build (bun)
# --------------------------------------------------

# A custom target which invokes bun to install dependencies and build the
# TypeScript/JSX sources. The generated bundle.js ends up in
# frontend/build/ts/bundle.js, which can be imported from QML via
# Qt.include("qrc:/qml/bundle.js").
add_custom_target(ts_frontend_install_only
    COMMAND cd ${CMAKE_SOURCE_DIR}/frontend && bun install
    WORKING_DIRECTORY ${CMAKE_SOURCE_DIR}/frontend
    COMMENT "Installing frontend dependencies with bun (no TS/JSX build)"
)

# Do not run a bundling/tsc step at CMake time — the frontend runs TSX natively
# with bun during development. The install-only target is available if you
# want CMake to ensure node deps are present, but it is not required for
# building the C++/QML application and does not block the build.
