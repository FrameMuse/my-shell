// simple component demonstrating custom JSX usage

export interface ButtonProps {
    text: string;
    onClick?: () => void;
}

export const Button = ({text, onClick}: ButtonProps) => (
    <Rectangle width={120} height={32} color="#50fa7b" radius={4}>
        <MouseArea {...{"anchors.fill": "parent", onClicked: onClick}} />
        <Text
            text={text}
            color="#282a36"
            {...{"anchors.centerIn": "parent"}}
            font={{ bold: true }}
        />
    </Rectangle>
);
