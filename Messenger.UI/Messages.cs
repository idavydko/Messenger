
namespace Messenger.UI
{
    public static class Messages
    {
        public const string UnsuccessfulOperation = "Operation completed unsuccessfully";
        public const string NoUser = "User doesn't exist";
        public const string NoSmsUser = "No registered SMS users to send message";
        public const string NoFacebookUser = "No registered facebook users to send message";
        
        public const string UserExists = "User already exists";
        public const string MessageNotSend = "Message was not send to the user";

        public const string FbGetUserDataUrl = "{0}/{1}?access_token={2}";
        public const string FbSendMessageUrl = "{0}/v2.6/me/messages?access_token={1}";
    }
}
