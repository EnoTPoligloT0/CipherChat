import { HubConnectionBuilder, HubConnection } from "@microsoft/signalr";
import WaitingRoom from './components/WaitingRoom';
import Chat from "./components/Chat.jsx";
import { useState } from "react";

interface Message {
    userName: string;
    message: string;
}

function App() {
    const [connection, setConnection] = useState<HubConnection | null>(null);
    const [chatRoom, setChatRoom] = useState("");
    const [messages, setMessages] = useState<Message[]>([]);

    const joinChat = async (userName: string, room: string) => {
        const newConnection = new HubConnectionBuilder()
            .withUrl("http://localhost:5012/chat", {
                withCredentials: true
            })
            .withAutomaticReconnect()
            .build();

        newConnection.on("ReceiveMessage", (receivedUserName: string, receivedMessage: string) => {
            setMessages(prevMessages => [
                ...prevMessages,
                { userName: receivedUserName, message: receivedMessage }
            ]);
        });

        try {
            await newConnection.start();
            await newConnection.invoke("JoinChat", { userName, chatRoom: room });
            setConnection(newConnection);
            setChatRoom(room);
        } catch (error) {
            console.error("Error joining chat:", error);
        }
    };

    const sendMessage = (message: string) => {
        if (message.trim() !== "" && connection) {
            connection.invoke("SendMessage", message)
                .catch(err => console.error("SendMessage error:", err));
        }
    };


    const closeChat = () => {
        if (connection) {
            connection.stop();
            setConnection(null);
            setMessages([]); // Clear messages when leaving the chat
        }
    };


    return (
        <div className="flex items-center justify-center min-h-screen bg-background-gray text-teal-dark">
            {connection ?
                <Chat messages={messages} chatRoom={chatRoom} sendMessage={sendMessage} closeChat={closeChat} />
                :
                <WaitingRoom joinChat={joinChat} />
            }
        </div>
    );
}

export default App;
