import {Heading, Input, Text} from "@chakra-ui/react";

function WaitingRoom() {
    return (
        <form className="max-w-sm w-full bg-gray-400 p-8 rounded shadow-lg">
            <Heading> Cipher Chat</Heading>
            <div className="mb-4">
                <Text fontSize={"sm"}>Username</Text>
                <Input name={"userName"} placeholder={"Enter your name"}></Input>
            </div>
        </form>
    );
}

export default WaitingRoom;