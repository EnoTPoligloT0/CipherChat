export const Message = ({messageInfo}) => {
    return (
        <div className="w-fit">
            <span className="text-sm text-cool-white">{messageInfo.userName}</span>
            <div className="p-2 bg-cool-white text-teal-950 rounded-lg shadow-md">
                {console.log(messageInfo)}
                {messageInfo.message}
            </div>
        </div>
    )


}