declare global {
  namespace JSX {
    interface Element {
      type: any
      props?: any
      children?: any
    }

    interface ElementTypeConstructor { }
    interface ElementTypeConstructor {
      (this: never, props: never): unknown
    }
    type ElementType = string | Element | ElementTypeConstructor

    // eslint-disable-next-line @typescript-eslint/no-empty-object-type
    interface ElementChildrenAttribute { children: {} }
    // eslint-disable-next-line @typescript-eslint/no-empty-object-type
    interface ElementAttributesProperty { props: {} }

    interface IntrinsicAttributes {}
    interface IntrinsicElements {
      text: any
      rectangle: any
    }
  }
}

export {}