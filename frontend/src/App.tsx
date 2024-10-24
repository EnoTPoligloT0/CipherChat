import { HubConnectionBuilder } from "@microsoft/signalr";
import WaitingRoom from './components/WaitingRoom';

function App() {
    const joinChat = async (userName: string, chatRoom: string) => {
        const connection = new HubConnectionBuilder()
            .withUrl("http://localhost:5012/chat",{
            withCredentials: true
        })
            .withAutomaticReconnect()
            .build();

        try {
            await connection.start();
            await connection.invoke("JoinChat", { userName, chatRoom });
            console.log(connection);
        } catch (error) {
            console.log(error);
        }
    };

    return (
        <div className="flex items-center justify-center min-h-screen bg-background-gray text-teal-dark">
            <WaitingRoom joinChat={joinChat} />
        </div>
    );
}

export default App;
