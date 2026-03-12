import QtQuick 2.15
import QtQuick.Controls 2.15
import QtQuick.Layouts 1.15
import QtWayland.Compositor 6.5

WaylandCompositor {
    id: compositor
    socketName: "wayland-0"          // XDG_RUNTIME_DIR/wayland-0

    WaylandOutput {
        id: output

        window: Window {
            width: 1280
            height: 720
            visible: true

            // === desktop background + icons ===
            Rectangle {
                id: desktop
                anchors.fill: parent
                color: "#282a36"

                // very simple “icons”
                Column {
                    anchors.left: parent.left
                    anchors.top: parent.top
                    anchors.margins: 16
                    spacing: 8

                    Text { text: "My Computer"; color: "white" }
                    Text { text: "Home";         color: "white" }
                }

                // === bottom panel ===
                Rectangle {
                    id: panel
                    anchors.left: parent.left
                    anchors.right: parent.right
                    anchors.bottom: parent.bottom
                    height: 40
                    color: "#44475a"
                    border.color: "#6272a4"
                    border.width: 1

                    RowLayout {
                        anchors.fill: parent
                        anchors.margins: 8
                        spacing: 12

                        Button { text: "\u2630" /* ☰ launcher */ }

                        // spacer grows
                        Item { Layout.fillWidth: true }

                        Text {
                            id: clock
                            color: "white"
                               font.pixelSize: 16
                        }

                        Timer {
                            interval: 1000
                            running: true
                            repeat: true
                            onTriggered: clock.text = Qt.formatTime(new Date(), "hh:mm:ss")
                        }
                    }
                }

                // === very‑basic window manager ===
                Item {
                    id: wmLayer
                    anchors.fill: parent

                    property var surfaceItems: []

                    Component.onCompleted: {
                        output.surfaceAdded.connect(onSurfaceAdded)
                        output.surfaceRemoved.connect(onSurfaceRemoved)

                        // Attempt to include a dev-generated JS bridge (created by bun)
                        try {
                            Qt.include("frontend/build/dev/frontend_vm.js")
                            if (typeof createFrontend === "function") {
                                createFrontend(wmLayer)
                            }
                        } catch (e) {
                            // not present in production; safe to ignore
                        }
                    }

                    function onSurfaceAdded(surface) {
                        var surfItem = surface.createItem()
                        surfItem.width = surface.width
                        surfItem.height = surface.height
                        surfItem.x = 100
                        surfItem.y = 100
                        surfItem.z = wmLayer.children.length

                        // add a “title bar” so the user can move the window
                        var deco = Qt.createQmlObject(
                            'import QtQuick 2.15; Rectangle { color: "#21222c"; height: 24; anchors.left: parent.left; anchors.right: parent.right; ' +
                            'Text { anchors.verticalCenter: parent.verticalCenter; anchors.left: parent.left; anchors.leftMargin: 4; color: "white"; text: "' + surface.class_ + '" } ' +
                            'MouseArea { anchors.fill: parent; drag.target: parent.parent; onPressed: parent.parent.z = wmLayer.children.length } }',
                            surfItem)

                        // make body draggable as well
                        var drag = Qt.createQmlObject('import QtQuick 2.15; MouseArea { anchors.fill: parent; drag.target = parent; }', surfItem)

                        wmLayer.surfaceItems.push(surfItem)
                    }

                    function onSurfaceRemoved(surface) {
                        for (var i = 0; i < wmLayer.surfaceItems.length; ++i) {
                            if (wmLayer.surfaceItems[i].surface === surface) {
                                wmLayer.surfaceItems[i].destroy()
                                wmLayer.surfaceItems.splice(i, 1)
                                break
                            }
                        }
                    }

                }
            }
        }
    }
}