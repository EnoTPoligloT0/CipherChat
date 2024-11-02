interface MessageProps {
    messageInfo: { userName: string; message: string };
}

export const Message: React.FC<MessageProps> = ({ messageInfo }) => {
    return (
        <div className="w-fit">
            <span className="text-sm text-cool-white">{messageInfo.userName}</span>
            <div className="p-2 bg-cool-white text-teal-950 rounded-lg shadow-md">
                {messageInfo.message}
            </div>
        </div>
    );
};
