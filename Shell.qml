import QtQuick
import QtWayland.Compositor

WaylandCompositor {
    id: comp

    // name the socket file created in XDG_RUNTIME_DIR
    socketName: "wayland-0"

    WaylandOutput {
        window: Window {
            width: 1280
            height: 720
            visible: true
            Rectangle {
                anchors.fill: parent
                color: "#282a36"
                Text {
                    anchors.centerIn: parent
                    text: "Luna Shell Active"
                    color: "white"
                    font.pixelSize: 32
                }
            }
        }
    }
}