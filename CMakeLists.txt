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