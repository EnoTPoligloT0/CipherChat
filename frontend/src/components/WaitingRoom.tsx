import {Heading, Input, Text, Button, Box, Flex} from "@chakra-ui/react";
import {useState} from "react";

interface WaitingRoomProps {
    joinChat: (userName: string, chatRoom: string) => void;
}

function WaitingRoom({joinChat}: WaitingRoomProps) {
    const [userName, setUserName] = useState("");
    const [chatRoom, setChatRoom] = useState("");

    const onSubmit = (e: React.FormEvent) => {
        e.preventDefault();
        joinChat(userName, chatRoom);
    };

    return (
        <Flex className="min-h-screen bg-background-gray items-center justify-center font-mono">
            <Box className="max-w-lg w-full bg-gray-800 p-12 rounded-lg shadow-xl">
                <Heading className="text-cool-white mb-4 text-center text-5xl font-extrabold">
                    Cipher Chat
                </Heading>
                <Text className="text-cool-white text-center mb-6 text-lg">
                    Join the conversation
                </Text>
                <form onSubmit={onSubmit}>
                    <div className="mb-8">
                        <Text className="text-cool-white font-medium mb-2">Username</Text>
                        <Input
                            name="userName"
                            placeholder="Enter your name"
                            value={userName}
                            onChange={(e) => setUserName(e.target.value)}
                            className="border border-cool-white text-cool-white placeholder-opacity-70 bg-gray-700 rounded-md focus:outline-none focus:ring-2 focus:ring-soft-teal transition p-4"
                            size="lg"
                        />
                    </div>
                    <div className="mb-8">
                        <Text className="text-cool-white font-medium mb-2">Chatroom name</Text>
                        <Input
                            name="chatRoom"
                            placeholder="Enter chatroom name"
                            value={chatRoom}
                            onChange={(e) => setChatRoom(e.target.value)}
                            className="border border-cool-white text-cool-white placeholder-opacity-70 bg-gray-700 rounded-md focus:outline-none focus:ring-2 focus:ring-soft-teal transition p-4"
                            size="lg"
                        />
                    </div>
                    <Button
                        className="w-full bg-soft-teal text-gray-800 hover:bg-opacity-90 transition rounded-md shadow-md py-3 text-lg"
                        type="submit">
                        Join Room
                    </Button>
                </form>
            </Box>
        </Flex>
    );
}

export default WaitingRoom;
