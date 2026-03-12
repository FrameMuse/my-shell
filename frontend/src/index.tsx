// convenience helpers from Bun runtime
import fs from "fs";

export function Desktop({children}: {children?: any}) {
  return (
    <rectangle width={1280} height={720} color="#282a36">
      {children}
    </rectangle>
  );
}

export function Panel() {
  return (
    <rectangle width={1280} height={40} color="#44475a" {...{"anchors.bottom": "parent.bottom"}}>
      <text text="Panel" color="white" anchors.centerIn="parent" />
    </rectangle>
  );
}

export function FileExplorer({path = '.'}: {path?: string}) {
  let entries = [];
  try {
    entries = new Bun.Glob("*").scanSync(path);
  } catch (e) {
    entries = ["(error)" + e];
  }
  return (
    <column anchors.left="parent.left" anchors.top="parent.top" anchors.margins={16} spacing={4}>
      {entries.map((e: string) => <text text={e} color="white" />)}
    </column>
  );
}

export function Terminal() {
  return (
    <rectangle width={400} height={200} color="#000000" anchors.centerIn="parent">
      <text text="[Terminal placeholder]" color="white" anchors.centerIn="parent" />
    </rectangle>
  );
}

export function App() {
  return (
    <Desktop>
      <Panel />
      <FileExplorer path="." />
      <Terminal />
    </Desktop>
  );
}

export function init() {
  console.log("TSX UI bundle initialized");
}
