using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TS3Query
{
    public partial class TS3Query
    {
        /// <summary>
        /// Sends a text message to the current channel the client is in.
        /// </summary>
        /// <param name="message">The message.</param>
        public void SendChannelTextMessage(string message)
        {
            Send(new TS3QueryRequest("sendtextmessage", new Dictionary<string, string>
            {
                { "targetmode", ((byte)TS3MessageTargetMode.Channel).ToString() },
                { "msg", message }
            }));
        }

        /// <summary>
        /// Sends a private text message to a specific target.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="message">The message.</param>
        public void SendTextMessage(string target, string message)
        {
            Send(new TS3QueryRequest("sendtextmessage", new Dictionary<string, string>
            {
                { "targetmode", ((byte)TS3MessageTargetMode.Private).ToString() },
                { "target", target },
                { "msg", message }
            }));
        }

        /// <summary>
        /// Sends a text message to the server.
        /// </summary>
        /// <param name="message">The message.</param>
        public void SendServerTextMessage(string message)
        {
            Send(new TS3QueryRequest("sendtextmessage", new Dictionary<string, string>
            {
                { "targetmode", ((byte)TS3MessageTargetMode.Server).ToString() },
                { "msg", message }
            }));
        }

        /// <summary>
        /// This command allows you to listen to ALL events that the client encounters. Events
        /// are things like people starting or stopping to talk, people joining or leaving,
        /// new channels being created and many more.
        /// It registers for client notifications for the specified
        /// serverConnectionHandlerID.  If the serverConnectionHandlerID is set to zero it
        /// applies to all server connection handlers.
        /// </summary>
        /// <param name="scHandlerId"></param>
        public void ClientNotifyRegister(string scHandlerId)
        {
            Send(new TS3QueryRequest("clientnotifyregister", new Dictionary<string, string>
            {
                { "schandlerid", scHandlerId },
                { "event", "any" }
            }));
        }

        /// <summary>
        /// This command allows you to listen to events that the client encounters. Events
        /// are things like people starting or stopping to talk, people joining or leaving,
        /// new channels being created and many more.
        /// It registers for client notifications for the specified
        /// serverConnectionHandlerID. If the serverConnectionHandlerID is set to zero it
        /// applies to all server connection handlers.
        /// </summary>
        /// <param name="serverConnectionHandlerID">The server connection handler ID.</param>
        /// <param name="eventName">The event's name.</param>
        public void ClientNotifyRegister(string serverConnectionHandlerID, string eventName)
        {
            Send(new TS3QueryRequest("clientnotifyregister", new Dictionary<string, string>
            {
                { "schandlerid", serverConnectionHandlerID },
                { "event", eventName }
            }));
        }

        /// <summary>
        /// Unregisters from all previously registered client notifications.
        /// </summary>
        public void ClientNotifyUnregister()
        {
            Send(new TS3QueryRequest("clientnotifyunregister");
        }

        /// <summary>
        /// Create a new token for a specific server group.
        /// </summary>
        /// <param name="groupID">The server group ID</param>
        public void TokenAddForServerGroup(string groupID)
        {
            Send(new TS3QueryRequest("tokenadd", new Dictionary<string, string>
            {
                { "tokentype", ((byte)TS3TokenType.ServerGroupID).ToString() },
                { "tokenid1", groupID }
            }));
        }

        /// <summary>
        /// Create a new token for a specific server group.
        /// </summary>
        /// <param name="groupID">The server group ID</param>
        /// <param name="description">The description for the token</param>
        public void TokenAddForServerGroup(string groupID, string description)
        {
            Send(new TS3QueryRequest("tokenadd", new Dictionary<string, string>
            {
                { "tokentype", ((byte)TS3TokenType.ServerGroupID).ToString() },
                { "tokenid1", groupID },
                { "tokendescription", description }
            }));
        }

        /// <summary>
        /// Create a new token for a specific server group.
        /// </summary>
        /// <param name="groupID">The server group ID</param>
        /// <param name="description">The description for the token</param>
        /// <param name="customSet">
        ///     This parameter allows you to specify a set of custom client properties.
        ///     This feature can be used when generating tokens to combine a website account
        ///     database with a TeamSpeak user.
        /// </param>
        public void TokenAddForServerGroup(string groupID, string description, Dictionary<string, string> customSet)
        {
            Send(new TS3QueryRequest("tokenadd", new Dictionary<string, string>
            {
                { "tokentype", ((byte)TS3TokenType.ServerGroupID).ToString() },
                { "tokenid1", groupID },
                { "tokendescription", description },
                { "tokencustomset", string.Join("|", customSet.Select(i => string.Format("ident={0} value={1}", i.Key, i.Value))) }
            }));
        }

        /// <summary>
        /// Create a new token for a specific server group.
        /// </summary>
        /// <param name="groupID">The server group ID</param>
        /// <param name="customSet">
        ///     This parameter allows you to specify a set of custom client properties.
        ///     This feature can be used when generating tokens to combine a website account
        ///     database with a TeamSpeak user.
        /// </param>
        public void TokenAddForServerGroup(string groupID, Dictionary<string, string> customSet)
        {
            Send(new TS3QueryRequest("tokenadd", new Dictionary<string, string>
            {
                { "tokentype", ((byte)TS3TokenType.ServerGroupID).ToString() },
                { "tokenid1", groupID },
                { "tokencustomset", string.Join("|", customSet.Select(i => string.Format("ident={0} value={1}", i.Key, i.Value))) }
            }));
        }

        /// <summary>
        /// Create a new token for a specific channel group.
        /// </summary>
        /// <param name="groupID">The channel group ID</param>
        /// <param name="channelID">The ID of the channel in which to search for the specific group</param>
        public void TokenAddForChannelGroup(string groupID, string channelID)
        {
            Send(new TS3QueryRequest("tokenadd", new Dictionary<string, string>
            {
                { "tokentype", ((byte)TS3TokenType.ChannelGroupID).ToString() },
                { "tokenid1", groupID },
                { "tokenid2", channelID }
            }));
        }

        /// <summary>
        /// Create a new token for a specific channel group.
        /// </summary>
        /// <param name="groupID">The server group ID</param>
        /// <param name="channelID">The ID of the channel in which to search for the specific group</param>
        /// <param name="description">The description for the token</param>
        public void TokenAddForChannelGroup(string groupID, string channelID, string description)
        {
            Send(new TS3QueryRequest("tokenadd", new Dictionary<string, string>
            {
                { "tokentype", ((byte)TS3TokenType.ChannelGroupID).ToString() },
                { "tokenid1", groupID },
                { "tokenid2", channelID },
                { "tokendescription", description }
            }));
        }

        /// <summary>
        /// Create a new token for a specific channel group.
        /// </summary>
        /// <param name="groupID">The server group ID</param>
        /// <param name="channelID">The ID of the channel in which to search for the specific group</param>
        /// <param name="description">The description for the token</param>
        /// <param name="customSet">
        ///     This parameter allows you to specify a set of custom client properties.
        ///     This feature can be used when generating tokens to combine a website account
        ///     database with a TeamSpeak user.
        /// </param>
        public void TokenAddForChannelGroup(string groupID, string channelID, string description, Dictionary<string, string> customSet)
        {
            Send(new TS3QueryRequest("tokenadd", new Dictionary<string, string>
            {
                { "tokentype", ((byte)TS3TokenType.ChannelGroupID).ToString() },
                { "tokenid1", groupID },
                { "tokenid2", channelID },
                { "tokendescription", description },
                { "tokencustomset", string.Join("|", customSet.Select(i => string.Format("ident={0} value={1}", i.Key, i.Value))) }
            }));
        }

        /// <summary>
        /// Create a new token for a specific channel group.
        /// </summary>
        /// <param name="groupID">The server group ID</param>
        /// <param name="channelID">The ID of the channel in which to search for the specific group</param>
        /// <param name="customSet">
        ///     This parameter allows you to specify a set of custom client properties.
        ///     This feature can be used when generating tokens to combine a website account
        ///     database with a TeamSpeak user.
        /// </param>
        public void TokenAddForChannelGroup(string groupID, string channelID, Dictionary<string, string> customSet)
        {
            Send(new TS3QueryRequest("tokenadd", new Dictionary<string, string>
            {
                { "tokentype", ((byte)TS3TokenType.ChannelGroupID).ToString() },
                { "tokenid1", groupID },
                { "tokenid2", channelID },
                { "tokencustomset", string.Join("|", customSet.Select(i => string.Format("ident={0} value={1}", i.Key, i.Value))) }
            }));
        }

        /// <summary>
        /// Deletes an existing token matching the token key specified with token.
        /// </summary>
        /// <param name="tokenKey">The token to delete.</param>
        public void TokenDelete(string tokenKey)
        {
            Send(new TS3QueryRequest("tokendelete", new Dictionary<string, string> {
                { "token", tokenKey }
            }));
        }

        /// <summary>
        /// Request a list of tokens available including their type and group IDs. Tokens 
        /// can be used to gain access to specified server or channel groups.
        /// 
        /// A token is similar to a client with administrator privileges that adds you to 
        /// a certain permission group, but without the necessity of a such a client with 
        /// administrator privileges to actually exist. It is a long (random looking) 
        /// string that can be used as a ticket into a specific server group.
        /// </summary>
        public void TokenList()
        {
            Send(new TS3QueryRequest("tokenlist"));
        }

        // TODO: TokenUse
        // TODO: Use
        // TODO: VerifyChannelPassword
        // TODO: VerifyServerPassword
        // TODO: WhoAmI
        // TODO: BanAdd
        // TODO: BanClient
        // TODO: BanDel
        // TODO: BanDelAll
        // TODO: BanList
        // TODO: ChannelAddPerm
        // TODO: ChannelClientAddPerm
        // TODO: ChannelClientDelPerm
        // TODO: ChannelClientList
        // TODO: ChannelClientPermList
        // TODO: ChannelConnectInfo
        // TODO: ChannelCreate
        // TODO: ChannelDelete
        // TODO: ChannelDelPerm
        // TODO: ChannelEdit
        // TODO: ChannelGroupAdd
        // TODO: ChannelGroupAddPerm
        // TODO: ChannelGroupClientList
        // TODO: ChannelGroupDel
        // TODO: ChannelGroupDelPerm
        // TODO: ChannelGroupList
        // TODO: ChannelGroupPermList
        // TODO: ChannelList
        // TODO: ChannelMove
        // TODO: ChannelPermList
        // TODO: ChannelVariable
        // TODO: ClientAddPerm
        // TODO: ClientDBDelete
        // TODO: ClientDBEdit
        // TODO: ClientDBList
        // TODO: ClientDelPerm
        // TODO: ClientGetDBIDfromUID
        // TODO: ClientGetIDs
        // TODO: ClientGetNameFromDBID
        // TODO: ClientGetNameFromUID
        // TODO: ClientGetUIDFromClID
        // TODO: ClientKick
        // TODO: ClientList
        // TODO: ClientMove
        // TODO: ClientMute
        // TODO: ClientPermList
        // TODO: ClientPoke
        // TODO: ClientUnmute
        // TODO: ClientUpdate
        // TODO: ClientVariable
        // TODO: ComplainAdd
        // TODO: ComplainDel
        // TODO: ComplainDelAll
        // TODO: ComplainList
        // TODO: FTCreateDir
        // TODO: FTDeleteFile
        // TODO: FTGetFileInfo
        // TODO: FTGetFileList
        // TODO: FTInitDownload
        // TODO: FTInitUpload
        // TODO: FTList
        // TODO: FTRenameFile
        // TODO: FTStop
        // TODO: HashPassword
        // TODO: Help
        // TODO: MessageAdd
        // TODO: MessageDel
        // TODO: MessageGet
        // TODO: MessageList
        // TODO: MessageUpdateFlag
        // TODO: PermOverview
        // TODO: Quit (integrate in Disconnect()?)
        // TODO: ServerConnectInfo
        // TODO: ServerConnectionHandlerList
        // TODO: ServerGroupAdd
        // TODO: ServerGroupAddClient
        // TODO: ServerGroupAddPerm
        // TODO: ServerGroupClientList
        // TODO: ServerGroupDel
        // TODO: ServerGroupDelClient
        // TODO: ServerGroupDelPerm
        // TODO: ServerGroupList
        // TODO: ServerGroupPermList
        // TODO: ServerGroupsByClientID
        // TODO: ServerVariable
        // TODO: SetClientChannelGroup

    }
}
