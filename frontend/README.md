# Luna Shell UI Frontend (TypeScript/JSX)

This folder contains the beginnings of an environment for authoring UI components
in **TypeScript with custom JSX**, compiled by [bun](https://bun.sh).
The intention is that you will add your own reactive runtime later; right now the
setup simply transpiles TSX to plain JavaScript and bundles it for consumption by
`qml`/`QtQuick`.

## Requirements

- [bun](https://bun.sh) installed on the development machine
  (`curl -fsSL https://bun.sh/install | bash`)
- `typescript` installed as a dev dependency (handled by `bun install`)

## Getting started

```sh
cd frontend
bun install

# compile once
bun run build

# or watch for changes
bun run watch
```

The compiled output will appear under `../build/ts/bundle.js`.  You can load this
file from your QML using a `Qt.include()` or `Loader`, for example:

```qml
Qt.include("qrc:/qml/bundle.js")
```

## Configuration

- `tsconfig.json` enables `jsx` support with a custom import source
  (`luna-jsx`).  You'll need to implement that module in JavaScript to interpret
  the JSX expressions and produce QtQuick `Item` instances.
- `package.json` provides convenient `build` and `watch` scripts.  The bundler
  currently uses `bun bun`, but you can replace it with another tool if desired.

## Next steps

- implement a runtime for `luna-jsx` that maps JSX tags to QtQuick type constructors
- add reactivity, e.g. a simple observable/`useState` system
- integrate the generated script into your `WaylandCompositor` shell
  (see `Shell.qml` for an example of loading external JS)

Good luck building your own custom UI layer!
