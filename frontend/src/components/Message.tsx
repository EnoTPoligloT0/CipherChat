import { useState, useEffect, useCallback } from 'react';
import { HubConnectionBuilder, HubConnection } from '@microsoft/signalr';
import { FaLock, FaUnlock } from 'react-icons/fa';
import CipherOptionsModal from './CipherModal';

interface MessageProps {
    messageInfo: { userName: string; message: string };
}

export const Message: React.FC<MessageProps> = ({ messageInfo }) => {
    const [isLocked, setIsLocked] = useState<boolean>(true);
    const [isModalOpen, setIsModalOpen] = useState<boolean>(false);
    const [decryptedMessage, setDecryptedMessage] = useState<string>('');
    const [connection, setConnection] = useState<HubConnection | null>(null);

    const [cipherType, setCipherType] = useState<string>('');
    const [language, setLanguage] = useState<string>('EN');
    const [key, setKey] = useState<string>('');

    useEffect(() => {
        const newConnection = new HubConnectionBuilder()
            .withUrl("http://localhost:5012/chat") // Adjust URL as needed
            .withAutomaticReconnect()
            .build();

        newConnection.start()
            .then(() => console.log('Connected to SignalR'))
            .catch(err => console.log('Connection failed: ', err));

        setConnection(newConnection);

        return () => {
            newConnection.stop();
        };
    }, []);

    const toggleLock = () => {
        if (isLocked) {
            setIsModalOpen(true);
        } else {
            if (decryptedMessage) {
                setIsLocked(true);
                setDecryptedMessage('');
            }
        }
    };

    const handleModalSubmit = useCallback(async () => {
        if (connection) {
            try {
                const decrypted = await connection.invoke<string>(
                    "DecryptMessage",
                    messageInfo.message,
                    cipherType,
                    language,
                    key
                );
                setDecryptedMessage(decrypted);
                setIsLocked(false); // Unlock after decryption
            } catch (err) {
                console.error("Decryption failed:", err);
            } finally {
                setIsModalOpen(false); // Close the modal after the decryption is done
            }
        } else {
            console.error("SignalR connection is not available.");
        }
    }, [connection, cipherType, language, key, messageInfo.message]);

    return (
        <div className="w-full flex items-start mb-4">
            <div className="flex flex-col items-start w-full">
                <span className="text-sm text-cool-white">{messageInfo.userName}</span>
                <div className="p-2 bg-cool-white text-teal-950 rounded-lg shadow-md mt-1 flex items-center justify-between">
                    <div>{isLocked ? messageInfo.message : decryptedMessage}</div>
                    <div
                        className="w-8 h-8 flex items-center justify-center bg-gray-200 rounded-full cursor-pointer hover:bg-gray-300 ml-2"
                        onClick={toggleLock}
                    >
                        {isLocked ? (
                            <FaLock className="text-gray-600" />
                        ) : (
                            <FaUnlock className="text-green-600" />
                        )}
                    </div>
                </div>
            </div>

            {isModalOpen && (
                <CipherOptionsModal
                    cipherType={cipherType}
                    language={language}
                    key={key}
                    setCipherType={setCipherType}
                    setLanguage={setLanguage}
                    setKey={setKey}
                    onSubmit={handleModalSubmit}
                    onClose={() => setIsModalOpen(false)}
                    isOpen={isModalOpen}
                />
            )}
        </div>
    );
};

export default Message;
