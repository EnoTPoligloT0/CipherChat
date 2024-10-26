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

        newConnection.on("ReceiveMessage", (userName: string, message: string) => {
            console.log(userName);
            console.log(message);
            setMessages(prevMessages => [...prevMessages, { userName, message }]);
        });

        try {
            await newConnection.start();
            await newConnection.invoke("JoinChat", { userName, chatRoom: room });
            console.log(newConnection);
            setConnection(newConnection);
            setChatRoom(room);
        } catch (error) {
            console.log(error);
        }
    };

    const sendMessage = (message:string) => {
        if (message.trim() === "") {
            return; // Prevent sending empty messages
        }
        connection?.invoke("SendMessage", message)
            .catch(err => console.error(err)); // Catch errors from invoking the method
    };

    const closeChat = () => {
        if (connection) {
            connection.stop();
            setConnection(null);
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
