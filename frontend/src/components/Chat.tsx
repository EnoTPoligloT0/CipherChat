import {
    Button,
    Heading,
    Input,
    Modal,
    ModalOverlay,
    ModalContent,
    ModalBody,
    ModalCloseButton,
    useDisclosure
} from "@chakra-ui/react";
import { CloseButton } from "./ui/close-button";
import { Message } from "./Message";
import { useState, useCallback } from "react";
import { motion } from "framer-motion";

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
    const { isOpen, onOpen, onClose } = useDisclosure();

    const onSendMessage = useCallback(() => {
        if (message.trim()) {
            sendMessage(message, cipherType, language, key);
            setMessage("");
            setKey("");
            setCipherType("");
        }
    }, [message, cipherType, language, key, sendMessage]);

    const handleCipherSelection = (cipher: string) => {
        if (cipher !== "Soon") {
            setCipherType(cipher);
            if (cipher === "Polibius") {
                setKey("1");
            } else {
                setKey("");
            }
        }
    };

    const handleKeyDown = (e: React.KeyboardEvent) => {
        if (e.key === "Enter") {
            e.preventDefault();
            onSendMessage();
        }
    };

    return (
        <motion.div
            className="flex flex-col h-screen w-screen p-6 bg-gray-800 shadow-lg"
            initial={{ opacity: 0 }}
            animate={{ opacity: 1 }}
            transition={{ duration: 0.5 }}>

            <div className="flex justify-between items-center mb-6">
                <Heading size="2xl" className="text-5xl font-bold text-cool-white">
                    {chatRoom}
                </Heading>
                <div className="flex items-center space-x-4 bg-cool-white rounded">
                    <CloseButton onClick={closeChat} className="text-2xl text-white" />
                </div>
            </div>

            <div className="flex flex-col flex-grow gap-4 overflow-auto pb-6">
                {messages.map((messageInfo, index) => (
                    <Message messageInfo={messageInfo} key={index} />
                ))}
            </div>

            <div className="flex gap-6 items-center mt-6 bg-gray-700 rounded-lg p-4">
                <Input
                    type="text"
                    value={message}
                    onChange={(e) => setMessage(e.target.value)}
                    onKeyDown={handleKeyDown}
                    placeholder="Write a message"
                    className="bg-gray-800 text-cool-white text-2xl py-6 px-8 flex-grow"
                />
                <Button
                    className="bg-soft-teal text-cool-white text-2xl py-6 px-10"
                    onClick={onSendMessage}>
                    Send
                </Button>
                <Button
                    className="bg-teal-dark text-cool-white text-2xl py-6 px-10"
                    onClick={onOpen}>
                    Cipher
                </Button>
            </div>

            <Modal isOpen={isOpen} onClose={onClose} isCentered>
                <ModalOverlay />
                <ModalContent bg="gray.800" borderRadius="md" maxW="sm" p={6}>
                    <ModalCloseButton color="cool-white" />
                    <ModalBody display="flex" flexDirection="column" alignItems="center" gap={6}>
                        <Heading size="lg" color="cool-white" mb={6}>
                            Select a Cipher
                        </Heading>
                        <div className="grid grid-cols-2 gap-4">
                            {["Caesar", "Vigenere", "Playfair", "Polibius", "Soon", "Soon"].map((cipher, index) => (
                                <motion.div
                                    whileHover={{ scale: 1.1 }}
                                    key={index}
                                    className={`flex items-center justify-center text-gray-800 ${
                                        cipher === cipherType ? 'bg-teal-dark' : 'bg-text-primary'
                                    } text-white rounded p-6 cursor-pointer transition-colors duration-200`}
                                    onClick={() => handleCipherSelection(cipher)}>
                                    {cipher}
                                </motion.div>
                            ))}
                        </div>
                        <div className="flex gap-4 mt-6">
                            {["EN", "PL"].map((lang) => (
                                <Button
                                    key={lang}
                                    bg={language === lang ? "teal-dark" : "text-primary"}
                                    color={language === lang ? "white" : "gray.400"}
                                    _hover={{ bg: "" }}
                                    flex="1"
                                    fontSize="xl"
                                    onClick={() => setLanguage(lang)}>
                                    {lang}
                                </Button>
                            ))}
                        </div>
                        {cipherType !== "Polibius" && (
                            <Input
                                placeholder={`Enter key (${cipherType === "Caesar" ? "Numbers only" : "Letters only"})`}
                                value={key}
                                onChange={(e) => setKey(e.target.value)}
                                bg="gray.800"
                                color="white"
                                mt={6}
                                inputMode={cipherType === "Caesar" ? "numeric" : "text"}
                                pattern={cipherType === "Caesar" ? "[0-9]*" : "[A-Za-z]*"}
                                className="text-2xl py-4 px-6"
                            />
                        )}
                        <Button
                            className="mt-6 bg-teal-dark text-cool-white text-xl"
                            onClick={onClose}>
                            OK
                        </Button>
                    </ModalBody>
                </ModalContent>
            </Modal>
        </motion.div>
    );
};

export default Chat;
