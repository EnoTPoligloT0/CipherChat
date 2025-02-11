import {
    Button,
    Heading,
    Input,
    Modal,
    ModalOverlay,
    ModalContent,
    ModalBody,
    ModalCloseButton
} from "@chakra-ui/react";
import { motion } from "framer-motion";

interface CipherOptionsModalProps {
    cipherType: string;
    language: string;
    cipherKey: string;
    setCipherType: (cipher: string) => void;
    setLanguage: (lang: string) => void;
    setCipherKey: (key: string) => void;
    onSubmit: () => Promise<void>;
    isOpen: boolean;
    onClose: () => void;
}

const CipherOptionsModal: React.FC<CipherOptionsModalProps> = ({
                                                                   cipherType,
                                                                   language,
                                                                   cipherKey,
                                                                   setCipherType,
                                                                   setLanguage,
                                                                   setCipherKey,
                                                                   onSubmit,
                                                                   isOpen,
                                                                   onClose,
                                                               }) => {
    const handleCipherSelection = (cipher: string) => {
        setCipherType(cipher);
        const newKey = cipher === "Polibius" ? "1" : "";
        setCipherKey(newKey); // Set cipherKey based on cipher type
        console.log("Selected Cipher:", cipher, "Key:", newKey);
    };

    return (
        <Modal isOpen={isOpen} onClose={onClose} isCentered>
            <ModalOverlay />
            <ModalContent bg="background-gray" borderRadius="md" maxW="sm" p={4}>
                <ModalCloseButton color="cool-white" onClick={onClose} />
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
                                onClick={() => handleCipherSelection(cipher)}
                            >
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
                                onClick={() => setLanguage(lang)}
                            >
                                {lang}
                            </Button>
                        ))}
                    </div>
                    {cipherType !== "Polibius" && (
                        <Input
                            placeholder={`Enter key (${cipherType === "Caesar" ? "Numbers only" : "Letters only"})`}
                            value={cipherKey}
                            onChange={(e) => setCipherKey(e.target.value)}
                            bg="gray.800"
                            color="white"
                            mt={4}
                            inputMode={cipherType === "Caesar" ? "numeric" : "text"}
                            pattern={cipherType === "Caesar" ? "[0-9]*" : "[A-Za-z]*"} // Validation based on cipher type
                        />
                    )}
                    <Button
                        className="mt-4 bg-teal-dark text-cool-white"
                        onClick={() => {
                            onSubmit();
                            onClose();
                        }}
                    >
                        OK
                    </Button>
                </ModalBody>
            </ModalContent>
        </Modal>
    );
};

export default CipherOptionsModal;
