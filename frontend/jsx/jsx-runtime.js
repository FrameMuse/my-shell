// Minimal JSX runtime for development. Returns lightweight vnode objects.
// The user will replace this with a runtime that creates QtQuick Items.

export const Fragment = Symbol.for("luna.fragment");

function normalizeProps(props) {
  return props || {};
}

export function createElement(type, props, ...children) {
  return { type, props: normalizeProps(props), children };
}

// Automatic JSX runtime entry points
export function jsx(type, props, key) {
  return createElement(type, props, ...(props && props.children ? [props.children] : []));
}

export function jsxs(type, props, key) {
  return createElement(type, props, ...(props && props.children ? [props.children] : []));
}

export const jsxDEV = jsx;

// Provide a shallow Component placeholder for runtime imports
export const Component = (fn) => fn;

export default { createElement, jsx, jsxs, Fragment };
