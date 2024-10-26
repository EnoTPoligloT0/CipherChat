import {Heading} from "@chakra-ui/react";
import {CloseButton} from "./ui/close-button.tsx";

export const Chat = ({chatRoom, closeChat}) => {
    return (
        <div className="w-1/2 soft-teal p-8 rounded shadow-lg">
            <div className="flex flex-row justify-between mb-5">
                <Heading size={"lg"} className="text-2xl font-bold leading-tight">{chatRoom}</Heading>
                <CloseButton onClick={closeChat} />
            </div>

        </div>

    )

}