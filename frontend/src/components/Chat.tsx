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
            e.preventDefault();  // Prevent form submission
            onSendMessage();  // Send the message
        }
    };

    return (
        <motion.div
            className="flex flex-col h-screen w-screen p-4 bg-gray-800 shadow-lg"
            initial={{ opacity: 0 }}
            animate={{ opacity: 1 }}
            transition={{ duration: 0.5 }}>

            <div className="flex justify-between items-center mb-5">
                <Heading size="lg" className="text-4xl font-bold text-cool-white">
                    {chatRoom}
                </Heading>
                <CloseButton onClick={closeChat} />
            </div>

            <div className="flex flex-col flex-grow gap-3 overflow-auto pb-3">
                {messages.map((messageInfo, index) => (
                    <Message messageInfo={messageInfo} key={index} />
                ))}
            </div>

            <div className="flex gap-3 items-center mt-5">
                <Input
                    type="text"
                    value={message}
                    onChange={(e) => setMessage(e.target.value)}
                    onKeyDown={handleKeyDown}  // Listen for Enter key press
                    placeholder="Write a message"
                    className="bg-gray-700 text-cool-white flex-grow text-xl py-3 px-4 md:text-2xl"
                />
                <Button
                    className="bg-soft-teal text-cool-white text-lg py-3 px-6 md:text-xl"
                    onClick={onSendMessage}>
                    Send
                </Button>
                <Button
                    className="bg-teal-dark text-cool-white text-lg py-3 px-6 md:text-xl"
                    onClick={onOpen}>
                    Cipher
                </Button>
            </div>

            <Modal isOpen={isOpen} onClose={onClose} isCentered>
                <ModalOverlay />
                <ModalContent bg="background-gray" borderRadius="md" maxW="sm" p={4}>
                    <ModalCloseButton color="cool-white" />
                    <ModalBody display="flex" flexDirection="column" alignItems="center" gap={3}>
                        <Heading size="md" color="cool-white" mb={3}>
                            Select a Cipher
                        </Heading>
                        <div className="grid grid-cols-3 gap-3">
                            {["Caesar", "Vigenere", "Playfair", "Polibius", "Soon", "Soon"].map((cipher, index) => (
                                <motion.div
                                    whileHover={{ scale: 1.1 }}
                                    key={index}
                                    className={`flex items-center justify-center text-gray-800 ${
                                        cipher === cipherType ? 'bg-teal-dark' : 'bg-text-primary'
                                    } text-white rounded p-4 cursor-pointer transition-colors duration-200`}
                                    onClick={() => handleCipherSelection(cipher)}>
                                    {cipher}
                                </motion.div>
                            ))}
                        </div>
                        <div className="flex gap-3 mt-4">
                            {["EN", "PL"].map((lang) => (
                                <Button
                                    key={lang}
                                    bg={language === lang ? "teal-dark" : "text-primary"}
                                    color={language === lang ? "white" : "gray.400"}
                                    _hover={{ bg: "" }}
                                    flex="1"
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
                                mt={4}
                                inputMode={cipherType === "Caesar" ? "numeric" : "text"}
                                pattern={cipherType === "Caesar" ? "[0-9]*" : "[A-Za-z]*"}
                            />
                        )}
                        <Button
                            className="mt-4 bg-teal-dark text-cool-white"
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
