import { Heading } from "@chakra-ui/react";
import { CloseButton } from "./ui/close-button.tsx";
import { Message } from "./Message.jsx";

const Chat = ({ messages, chatRoom, closeChat }) => {
    return (
        <div className="w-1/2 p-8 rounded shadow-lg">
            <div className="flex flex-row justify-between mb-5">
                <Heading size={"lg"} className="text-2xl font-bold leading-tight">
                    {chatRoom}
                </Heading>
                <CloseButton onClick={closeChat} />
            </div>

            <div className="flex flex-col overflow-auto scroll scroll-smooth h-96 gap-3 pb-3">
                {messages.map((messageInfo, index) => (
                    <Message messageInfo={messageInfo} key={index} />
                ))}
            </div>
        </div>
    );
};

export default Chat; // Ensure you have a default export
