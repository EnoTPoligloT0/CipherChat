import React from "react";
import {
    Button,
    Heading,
    Text,
    Flex,
    VStack,
    Box,
    SimpleGrid,
    Icon,
} from "@chakra-ui/react";
import { motion } from "framer-motion";
import { FaLock, FaKey, FaComments } from "react-icons/fa";

const HomePage: React.FC = () => {
    const redirectToChat = () => {
        window.location.href = "http://localhost:5173/";
    };

    return (
        <Flex
            className="min-h-screen"
            bg="gray.900"
            align="center"
            justify="center"
            direction="column"
            fontFamily="Poppins, sans-serif"
            color="white"
            px={6}
            textAlign="center"
        >
            <VStack spacing={8} maxW="800px" mb={12}>
                <Heading
                    size="2xl"
                    bgGradient="linear(to-r, teal.400, purple.500)"
                    bgClip="text"
                    fontWeight="extrabold"
                >
                    Cipher Chat
                </Heading>
                <Text fontSize="xl" opacity={0.8}>
                    Welcome to Cipher Chat – a secure and fun messaging platform where you can encrypt your messages with classic ciphers and share them with friends!
                </Text>
            </VStack>

            <Box bg="gray.800" p={8} borderRadius="lg" mb={12} shadow="md">
                <Heading size="xl" mb={4} color="teal.400">
                    About Cipher Chat
                </Heading>
                <Text fontSize="lg" opacity={0.8} lineHeight="1.8">
                    Cipher Chat allows you to communicate securely using classic ciphers.
                    Ciphers are methods of encrypting messages to make them unreadable
                    without the proper key or decryption method. Whether you're a
                    cryptography enthusiast or just looking for a fun way to send secret
                    messages, Cipher Chat has you covered!
                </Text>
            </Box>

            <SimpleGrid columns={[1, 2, 3]} spacing={8} mb={12}>
                {[
                    { name: "Caesar Cipher", description: "Shift letters by a fixed number.", icon: FaLock },
                    { name: "Vigenère Cipher", description: "Use a keyword for encryption.", icon: FaKey },
                    { name: "Playfair Cipher", description: "Encrypt digraphs (pairs of letters).", icon: FaComments },
                    { name: "Polybius Cipher", description: "Map letters to grid coordinates.", icon: FaKey },
                ].map((cipher, index) => (
                    <motion.div
                        key={index}
                        whileHover={{ scale: 1.05 }}
                        style={{
                            padding: "1rem",
                            backgroundColor: "teal.500",
                            color: "white",
                            borderRadius: "0.5rem",
                            cursor: "pointer",
                            textAlign: "center",
                            boxShadow: "0 4px 6px rgba(0, 0, 0, 0.2)",
                        }}
                    >
                        <Icon as={cipher.icon} w={10} h={10} mb={4} />
                        <Heading size="md" mb={2}>
                            {cipher.name}
                        </Heading>
                        <Text fontSize="sm" opacity={0.9}>
                            {cipher.description}
                        </Text>
                    </motion.div>
                ))}
            </SimpleGrid>

            <Button
                size="lg"
                bg="purple.500"
                _hover={{ bg: "purple.600" }}
                color="white"
                onClick={redirectToChat}
                shadow="md"
            >
                Go to Chat
            </Button>
        </Flex>
    );
};

export default HomePage;
