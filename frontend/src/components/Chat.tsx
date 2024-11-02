import { Button, Heading, Input } from "@chakra-ui/react";
import { CloseButton } from "./ui/close-button";
import { Message } from "./Message";
import { useState } from "react";

interface ChatProps {
    messages: { userName: string; message: string }[];
    chatRoom: string;
    sendMessage: (message: string, cipherType: string, language: string, key: string) => void;
    closeChat: () => void;
}

const Chat: React.FC<ChatProps> = ({ messages, chatRoom, sendMessage, closeChat }) => {
    const [message, setMessage] = useState("");
    const [cipherType, setCipherType] = useState("");
    const [language, setLanguage] = useState("EN");
    const [key, setKey] = useState("");

    const onSendMessage = () => {
        if (message.trim()) {
            sendMessage(message, cipherType, language, key);
            setMessage("");
            setKey("");
            setCipherType("");
        }
    };

    return (
        <div className="w-1/2 p-8 bg-gray-800 rounded shadow-lg">
            <div className="flex justify-between items-center mb-5">
                <Heading size="lg" className="text-2xl font-bold text-white">
                    {chatRoom}
                </Heading>
                <CloseButton onClick={closeChat} />
            </div>

            <div className="flex flex-col overflow-auto h-96 gap-3 pb-3">
                {messages.map((messageInfo, index) => (
                    <Message messageInfo={messageInfo} key={index} />
                ))}
            </div>

            <div className="flex gap-3 mt-5">
                <div className="flex flex-col">
                    <label className="text-sm text-gray-300">Cipher Type</label>
                    <select
                        value={cipherType}
                        onChange={(e) => setCipherType(e.target.value)}
                        className="bg-gray-700 text-white p-2 rounded"
                    >
                        <option value="">Select cipher</option>
                        <option value="Caesar">Caesar</option>
                        <option value="Vigenere">Vigenere</option>
                        <option value="Playfair">Playfair</option>
                        <option value="Polibius">Polibius</option>
                    </select>
                </div>
                <div className="flex flex-col">
                    <label className="text-sm text-gray-300">Language</label>
                    <select
                        value={language}
                        onChange={(e) => setLanguage(e.target.value)}
                        className="bg-gray-700 text-white p-2 rounded"
                    >
                        <option value="EN">English</option>
                        <option value="PL">Polish</option>
                    </select>
                </div>
                <div className="flex flex-col flex-grow">
                    <label className="text-sm text-gray-300">Key (optional)</label>
                    <Input
                        type="text"
                        value={key}
                        onChange={(e) => setKey(e.target.value)}
                        placeholder="Enter key"
                        className="bg-gray-700 text-white"
                    />
                </div>
                <div className="flex flex-col flex-grow">
                    <label className="text-sm text-gray-300">Message</label>
                    <Input
                        type="text"
                        value={message}
                        onChange={(e) => setMessage(e.target.value)}
                        placeholder="Write a message"
                        className="bg-gray-700 text-white"
                    />
                </div>
                <Button colorScheme="blue" onClick={onSendMessage}>
                    Send
                </Button>
            </div>
        </div>
    );
};

export default Chat;
