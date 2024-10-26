import { Button, Heading, Input } from "@chakra-ui/react";
import { CloseButton } from "./ui/close-button.tsx";
import { Message } from "./Message.jsx";
import { useState } from "react";

const Chat = ({ messages, chatRoom, sendMessage, closeChat }) => {
    const [message, setMessage] = useState(""); // Initialized with an empty string

    const onSendMessage = () => {
        sendMessage(message);
        setMessage("");
    };

    return (
        <div className="w-1/2 p-8 bg-gray-800 rounded shadow-lg">
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

            <div className="flex gap-3">
                <Input
                    type="text"
                    value={message}
                    onChange={(e) => setMessage(e.target.value)}
                    placeholder={"Write a message"}
                />
                <Button colorScheme="blue" onClick={onSendMessage}>Send</Button>
            </div>

        </div>
    );
};

export default Chat;
