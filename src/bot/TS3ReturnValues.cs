using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace TS3Query
{
    public class TS3EntriesCollection<T> : TS3ErrorableResponse
    {
        internal T[] entries;

        public T[] Entries { get { return entries; } }
    }

    public class TS3Error
    {
        internal uint id;
        internal string msg;

        public uint ID { get { return id; } }
        public string Message { get { return msg; } }
    }

    public class TS3ErrorableResponse
    {
        internal TS3Error error;

        public TS3Error Error { get { return error; } }
    }

    public class TS3Ban : TS3ErrorableResponse // banadd, banclient
    {
        internal uint banid;

        public uint BanID { get { return banid; } }
    }

    public class TS3BanList : TS3EntriesCollection<TS3BanListEntry> { }

    public class TS3BanListEntry 
    {
        internal uint banid;
        internal string ip;
        internal long created;
        internal string invokername;
        internal uint invokercldbid;
        internal string invokeruid;
        internal string reason;
        internal uint enforcements;

        public uint BanID { get { return banid; } }
        public IPAddress BanIP { get { return string.IsNullOrEmpty(ip) ? null : IPAddress.Parse(ip); } }
        public DateTime Created { get { return DateTime.FromBinary(created); } }
        public string InvokerName { get { return invokername; } }
        public uint InvokerClientDatabaseID { get { return invokercldbid; } }
        public string InvokerUniqueID { get { return invokeruid; } }
        public string Reason { get { return reason; } }
        public uint Enforcements { get { return enforcements; } } // TODO: Check up what Enforcements is for
    }

    public class TS3ChannelClientList : TS3EntriesCollection<TS3ChannelClientListEntry> { }

    public class TS3ChannelClientListEntry 
    {
        internal uint clid;
        internal uint cid;
        internal uint client_database_id;
        internal string client_nickname;
        internal uint client_type; // TODO: Create enumerator for ClientType

        public uint ClientID { get { return clid; } }
        public uint ChannelID { get { return cid; } }
        public uint ClientDatabaseID { get { return client_database_id; } }
        public string ClientNickname { get { return client_nickname; } }
        public uint ClientType { get { return client_type; } }
    }

    public class TS3ChannelClientPermList : TS3EntriesCollection<TS3PermListEntry>
    {
        internal uint cid;
        internal uint cldbid;

        public uint ChannelID { get { return cid; } }
        public uint ClientDatabaseID { get { return cldbid; } }
    }

    public class TS3PermListEntry // channelclientpermlist, servergrouppermlist
    {
        internal uint permid;
        internal int permvalue;
        internal bool permnegated;
        internal bool permskip;

        public uint ID { get { return permid; } }
        public int Value { get { return permvalue; } }
        public bool Negated { get { return permnegated; } }
        public bool Skip { get { return permskip; } }
    }

    public class TS3ChannelConnectInfo : TS3ErrorableResponse 
    {
        internal string path;
        internal string password;

        public string Path { get { return path; } }
        public string Password { get { return password; } }
    }

    public class TS3CreatedChannelInfo : TS3ErrorableResponse // channelcreate
    {
        internal uint cid;

        public uint ChannelID { get { return cid; } }
    }

    public class TS3CreatedChannelGroupInfo : TS3ErrorableResponse // channelgroupadd
    {
        internal uint cgid;

        public uint ChannelGroupID { get { return cgid; } }
    }

    public class TS3ChannelGroupClientList : TS3EntriesCollection<TS3ChannelGroupClientListEntry> { }

    public class TS3ChannelGroupClientListEntry
    {
        internal uint cid;
        internal uint cldbid;
        internal uint cgid;

        public uint ChannelID { get { return cid; } }
        public uint ClientDatabaseID { get { return cldbid; } }
        public uint ChannelGroupID { get { return cgid; } }
    }

    public class TS3ChannelGroupList : TS3EntriesCollection<TS3ChannelGroupListEntry> { }

    public class TS3ChannelGroupListEntry
    {
        internal uint cgid;
        internal string name;
        internal uint type; // TODO: Implement enumerator TS3ChannelGroupType
        internal uint iconid;
        internal bool savedb;

        public uint ID { get { return cgid; } }
        public string Name { get { return name; } }
        public uint Type { get { return type; } }
        public uint IconID { get { return iconid; } }
        public bool SaveToDatabase { get { return savedb; } }
    }

    public class TS3ChannelGroupPermList : TS3EntriesCollection<TS3ChannelGroupPermListEntry> { }

    public class TS3ChannelGroupPermListEntry
    {
        internal uint permid;
        internal int permvalue;
        internal bool permnegated;
        internal bool permskip;

        public uint ID { get { return permid; } }
        public int Value { get { return permvalue; } }
        public bool Negated { get { return permnegated; } }
        public bool Skip { get { return permskip; } }
    }

    public class TS3ChannelList : TS3EntriesCollection<TS3ChannelListEntry> { }

    public class TS3ChannelListEntry
    {
        internal uint cid;
        internal uint pid;
        internal uint channel_order;
        internal string channel_name;
        internal string channel_topic;
        internal bool channel_flag_are_subscribed;

        public uint ID { get { return cid; } }
        public uint PID { get { return pid; } } // TODO: Check what PID is
        public uint Order { get { return channel_order; } }
        public string Name { get { return channel_name; } }
        public string Topic { get { return channel_topic; } }
        public bool Subscribed { get { return channel_flag_are_subscribed; } }
    }

    public class TS3ChannelPermList : TS3EntriesCollection<TS3ChannelPermListEntry> { }

    public class TS3ChannelPermListEntry
    {
        internal uint cid;
        internal uint permid;
        internal int permvalue;
        internal bool permnegated;
        internal bool permskip;

        public uint ChannelID { get { return cid; } }
        public uint ID { get { return permid; } }
        public int Value { get { return permvalue; } }
        public bool Negated { get { return permnegated; } }
        public bool Skip { get { return permskip; } }
    }

    public class TS3ClientPermList : TS3EntriesCollection<TS3ClientPermListEntry> { }

    public class TS3ClientPermListEntry
    {
        internal uint cldbid;
        internal uint permid;
        internal int permvalue;
        internal bool permnegated;
        internal bool permskip;

        public uint ClientDatabaseID { get { return cldbid; } }
        public uint ID { get { return permid; } }
        public int Value { get { return permvalue; } }
        public bool Negated { get { return permnegated; } }
        public bool Skip { get { return permskip; } }
    }

    public class TS3ChannelVariableResponse : TS3EntriesCollection<TS3ChannelVariableSingleResponse> { }

    public class TS3ChannelVariableSingleResponse : TS3ErrorableResponse
    {
        internal string channel_name;
        internal string channel_topic;
        internal string channel_description;
        internal uint channel_codec; // TODO: Implement enumerator TS3ChannelCodec
        internal uint channel_codec_quality; // Ranges from 0 ~ 10
        internal uint channel_order;
        internal int channel_maxclients; // -1 = unlimited
        internal int channel_maxfamilyclients; // -1 = unlimited
        internal bool channel_flag_permanent;
        internal bool channel_flag_semipermanent;
        internal bool channel_flag_default;
        internal bool channel_flag_password;
        internal uint channel_codec_latency_factor; // ... I just forgot what range this had. Oh well.
        internal bool channel_codec_is_unencrypted;
        internal bool channel_flag_maxclients_unlimited;
        internal bool channel_flag_maxfamilyclients_unlimited;
        internal bool channel_flag_maxfamilyclients_inherited;
        internal bool channel_flag_are_subscribed; // Meanwhile my finger hurts
        internal uint channel_needed_talk_power;
        internal bool channel_forced_silence;
        internal string channel_name_phonetic;
        internal uint channel_icon_id;

        // And now the whole thing again. This sucks. ._.
        public string ChannelName { get { return channel_name; } }
        public string ChannelTopic { get { return channel_topic; } }
        public string ChannelDescription { get { return channel_description; } }
        public string ChannelNamePhonetic { get { return channel_name_phonetic; } }
        public uint ChannelCodec { get { return channel_codec; } }
        public uint ChannelCodecQuality { get { return channel_codec_quality; } }
        public uint ChannelOrder { get { return channel_order; } }
        public uint ChannelCodecLatencyFactor { get { return channel_codec_latency_factor; } }
        public uint ChannelIconID { get { return channel_icon_id; } }
        public uint ChannelNeededTalkPower { get { return channel_needed_talk_power; } }
        public int ChannelMaxClients { get { return channel_maxclients; } }
        public int ChannelMaxFamilyClients { get { return channel_maxfamilyclients; } }
        public bool ChannelIsPermanent { get { return channel_flag_permanent; } }
        public bool ChannelIsSemiPermanent { get { return channel_flag_semipermanent; } }
        public bool ChannelIsDefault { get { return channel_flag_semipermanent; } }
        public bool ChannelNeedsPassword { get { return channel_flag_password; } }
        public bool ChannelMaxClientsIsUnlimited { get { return channel_flag_maxclients_unlimited; } }
        public bool ChannelMaxFamilyClientsIsUnlimited { get { return channel_flag_maxfamilyclients_unlimited; } }
        public bool ChannelMaxFamilyClientsIsInherited { get { return channel_flag_maxfamilyclients_inherited; } }
        public bool ChannelIsSubscribed { get { return channel_flag_are_subscribed; } }
        public bool ChannelForcedSilence { get { return channel_forced_silence; } }

    }

    public class TS3ClientDatabaseList : TS3EntriesCollection<TS3ClientDatabaseListEntry> { }

    public class TS3ClientDatabaseListEntry
    {
        internal uint cldbid;
        internal string client_unique_identifier;
        internal string client_nickname;
        // Documentation says "..." here. I guess those TS guys were just to
        // lazy to friggen finish the documentation. So expected.

        public uint DatabaseID { get { return cldbid; } }
        public string UniqueID { get { return client_unique_identifier; } }
        public string Nickname { get { return client_nickname; } }
        // ...

        // TODO: Look up the rest of this without the docs instead of trusting TSCQ devs any longer.
    }

    public class TS3DatabaseIDSearchResponse : TS3ErrorableResponse // clientgetdbidfromuid
    {
        internal string cluid;
        internal uint cldbid;

        public string UniqueID { get { return cluid; } }
        public uint DatabaseID { get { return cldbid; } }
    }

    public class TS3IDSearchResponse : TS3ErrorableResponse // clientgetids
    {
        // I don't quite understand the "multiple" IDs part in the documentation.
        // I mean how can there be multiple entries for a unique ID...? >_>

        internal string cluid;
        internal uint clid;
        internal string name;

        public string UniqueID { get { return cluid; } }
        public uint ID { get { return clid; } }
        public string Name { get { return name; } }
    }

    public class TS3NameSearchResponse : TS3ErrorableResponse // clientgetnamefromdbid, clientgetnamefromuid
    {
        internal string cluid;
        internal uint cldbid;
        internal string name;

        public string UniqueID { get { return cluid; } }
        public uint DatabaseID { get { return cldbid; } }
        public string Name { get { return name; } }
    }

    public class TS3UniqueIDSearchResponse : TS3ErrorableResponse // clientgetuidfromclid
    {
        internal string schandlerid; // damn, it's a notification... DAMN YOU ASYNC QUERIES
        internal uint clid;
        internal string cluid;
        internal string nickname;

        public string SCHandlerID { get { return schandlerid; } }
        public uint ClientID { get { return clid; } }
        public string ClientUniqueID { get { return cluid; } }
        public string ClientNickname { get { return nickname; } }
    }

    public class TS3ClientList : TS3EntriesCollection<TS3ClientListEntry> { }

    public class TS3ClientListEntry
    {
        internal uint clid;
        internal uint cid;
        internal uint client_database_id;
        internal string client_nickname;
        internal uint client_type; // TODO: Implement enumerator TS3ClientType

        // -uid
        internal string client_unique_identifier;

        // -away
        internal bool client_away;
        internal string client_away_message;

        // -voice
        internal bool client_flag_talking;
        internal bool client_input_muted;
        internal bool client_output_muted;
        internal bool client_input_hardware; // Not sure about this
        internal bool client_output_hardware; // Not sure about this either
        internal uint client_talk_power;
        internal bool client_is_talker;
        internal bool client_is_priority_speaker;
        internal bool client_is_recording;
        internal bool client_is_channel_commander;
        internal bool client_is_muted;

        // -groups
        internal string client_servergroups; // example: "client_servergroups=2,52" => convert this to multiple uint
        internal uint client_channel_group_id;

        // -icon
        internal uint client_icon_id;

        // -country
        internal string client_country; // example: "client_country=DE"

        public uint ID { get { return clid; } }
        public uint ChannelID { get { return cid; } }
        public uint DatabaseID { get { return client_database_id; } }
        public string Nickname { get { return client_nickname; } }
        public uint Type { get { return client_type; } } // TODO: Implement enumerator TS3ClientType

        public string UniqueID { get { return client_unique_identifier; } }

        public bool IsAway { get { return client_away; } }
        public string AwayMessage { get { return client_away_message; } }

        public bool IsTalking { get { return client_flag_talking; } }
        public bool IsInputMuted { get { return client_input_muted; } }
        public bool IsOutputMuted { get { return client_output_muted; } }
        public bool HasInputHardware { get { return client_input_hardware; } }
        public bool HasOutputHardware { get { return client_output_hardware; } }
        public uint TalkPower { get { return client_talk_power; } }
        public bool IsTalker { get { return client_is_talker; } }
        public bool IsPrioritySpeaker { get { return client_is_priority_speaker; } }
        public bool IsRecording { get { return client_is_recording; } }
        public bool IsChannelCommander { get { return client_is_channel_commander; } }
        public bool IsMuted { get { return client_is_muted; } }

        public uint[] ServerGroupIDs { get { return client_servergroups.Split(',').Select(i => uint.Parse(i)).ToArray(); } }
        public uint ChannelGroupID { get { return client_channel_group_id; } }

        public uint IconID { get { return client_icon_id; } }

        public string Country { get { return client_country; } }
    }

    public class TS3ClientVariableResponse : TS3EntriesCollection<TS3ClientVariableSingleResponse> { }

    public class TS3ClientVariableSingleResponse
    {
        internal string client_unique_identifier;
        internal string client_nickname;
        internal bool client_input_muted;
        internal bool client_output_muted;
        internal bool client_outputonly_muted;
        internal bool client_input_hardware;
        internal bool client_output_hardware;
        internal string client_meta_data; // TODO: Check what client_meta_data is
        internal bool client_is_recording;
        internal uint client_channel_group_id;
        internal string client_servergroups; // "client_servergroups=1,2,3,4,5"
        internal bool client_away;
        internal string client_away_message;
        internal uint client_type; // TODO: Implement enumerator TS3ClientType
        internal bool client_flag_avatar;
        internal uint client_talk_power;
        internal bool client_talk_request;
        internal string client_talk_request_msg;
        internal string client_description;
        internal bool client_is_talker;
        internal bool client_is_priority_speaker;
        internal uint client_unread_messages; // actually a counter
        internal string client_nickname_phonetic;
        internal uint client_needed_serverquery_view_power;
        internal uint client_icon_id;
        internal bool client_is_channel_commander;
        internal string client_country;
        internal uint client_channel_group_inherited_channel_id;
        internal bool client_flag_talking;
        internal bool client_is_muted;
        internal float client_volume_modificator; // or should we use a double here? I don't think so.
       
        // Only available for local client, need to be requested for other clients and
        // are therefore not available for them.
        internal string client_version;
        internal string client_platform;
        internal string client_login_name;
        internal long client_created; // binary time
        internal long client_lastconnected; // binary time
        internal long client_totalconnections;
        internal long client_month_bytes_uploaded;
        internal long client_month_bytes_downloaded;
        internal long client_total_bytes_uploaded;
        internal long client_total_bytes_downloaded;

        // Will always only be available for local client.
        internal bool client_input_deactivated;

        public string UniqueID { get { return client_unique_identifier; } }
        public string Nickname { get { return client_nickname; } }
        public bool IsInputMuted { get { return client_input_muted; } }
        public bool IsOutputMuted { get { return client_output_muted; } }
        public bool IsOutputOnlyMuted { get { return client_outputonly_muted; } }
        public bool HasInputHardware { get { return client_input_hardware; } }
        public bool HasOutputHardware { get { return client_output_hardware; } }
        public string MetaData { get { return client_meta_data; } }
        public bool IsRecording { get { return client_is_recording; } }
        public uint ChannelGroupID { get { return client_channel_group_id; } }
        public uint[] ServerGroups { get { return client_servergroups.Split(',').Select(i => uint.Parse(i)).ToArray(); } }
        public bool IsAway { get { return client_away; } }
        public string AwayMessage { get { return client_away_message; } }
        public uint Type { get { return client_type; } }
        public bool HasAvatar { get { return client_flag_avatar; } }
        public uint TalkPower { get { return client_talk_power; } }
        public bool IsTalkRequested { get { return client_talk_request; } }
        public string TalkRequestMsg { get { return client_talk_request_msg; } }
        public string Description { get { return client_description; } }
        public bool IsTalker { get { return client_is_talker; } }
        public bool IsPrioritySpeaker { get { return client_is_priority_speaker; } }
        public uint UnreadMessages { get { return client_unread_messages; } }
        public string NicknamePhonetic { get { return client_nickname_phonetic; } }
        public uint NeededServerqueryViewPower { get { return client_needed_serverquery_view_power; } }
        public uint IconID { get { return client_icon_id; } }
        public bool IsChannelCommander { get { return client_is_channel_commander; } }
        public string Country { get { return client_country; } }
        public uint ChannelGroupInheritedChannelID { get { return client_channel_group_inherited_channel_id; } }
        public bool IsTalking { get { return client_flag_talking; } }
        public bool IsMuted { get { return client_is_muted; } }
        public float VolumeModificator { get { return client_volume_modificator; } }

        // Only available for local client, need to be requested for other clients and
        // are therefore not available for them.
        public string Version { get { return client_version; } }
        public string Platform { get { return client_platform; } }
        public string LoginName { get { return client_login_name; } }
        public DateTime Created { get { return DateTime.FromBinary(client_created); } }
        public DateTime LastConnected { get { return DateTime.FromBinary(client_lastconnected); } }
        public long TotalConnections { get { return client_totalconnections; } }
        public long MonthBytesUploaded { get { return client_month_bytes_uploaded; } }
        public long MonthBytesDownloaded { get { return client_month_bytes_downloaded; } }
        public long TotalBytesUploaded { get { return client_total_bytes_uploaded; } }
        public long TotalBytesDownloaded { get { return client_total_bytes_downloaded; } }

        // Will always only be available for local client.
        public bool IsInputDeactivated { get { return client_input_deactivated; } }
    }

    public class TS3ComplainList : TS3EntriesCollection<TS3ComplainListEntry> { }

    public class TS3ComplainListEntry
    {
        internal uint tcldbid;
        internal string tname;
        internal uint fcldbid;
        internal string fname;
        internal string message;

        public uint TargetDatabaseID { get { return tcldbid; } }
        public string TargetName { get { return tname; } }
        public uint SourceDatabaseID { get { return fcldbid; } }
        public string SourceName { get { return fname; } }
        public string Message { get { return message; } }
    }

    public class TS3SCHandlerIDResponse : TS3ErrorableResponse // currentschandler, serverconnectionhandlerlist
    {
        internal uint schandlerid;

        public uint SCHandlerID { get { return schandlerid; } }
    }

    public class TS3FTFileInfo : TS3ErrorableResponse // ftgetfileinfo, ftgetfilelist
    {
        // Several fields in here have to be auto-filled with their previous
        // instance's field values if they get stacked to an array. This is
        // because the client query (and maybe even the server query) leaves
        // out repetitive data, for example if you request multiple files from
        // the same channel, the cid will only be sent for the first instance
        // in a row. It will only be sent again if it changes. Quite a weird
        // thing but it sounds logic, doesn't it?
        internal uint cid;
        internal string path;
        internal string name;
        internal long size;
        internal long datetime;
        internal uint type;
    }

    public class TS3FTDownloadInitInfo : TS3ErrorableResponse // ftinitdownload
    {
        internal uint clientftfid;
        internal uint serverftfid;
        internal string ftkey;
        internal ushort port;
        internal long size;

        public uint ClientFileTransferID { get { return clientftfid; } }
        public uint ServerFileTransferID { get { return serverftfid; } }
        public string Key { get { return ftkey; } }
        public ushort Port { get { return port; } }
        public long Size { get { return size; } }
    }

    public class TS3FTUploadInitInfo : TS3ErrorableResponse // ftinitupload
    {
        internal uint clientftfid;
        internal uint serverftfid;
        internal string ftkey;
        internal ushort port;
        internal long seekpos;

        public uint ClientFileTransferID { get { return clientftfid; } }
        public uint ServerFileTransferID { get { return serverftfid; } }
        public string Key { get { return ftkey; } }
        public ushort Port { get { return port; } }
        public long SeekPosition { get { return seekpos; } }
    }

    public class TS3FTList : TS3EntriesCollection<TS3FTListEntry> { }

    public class TS3FTListEntry
    {
        internal uint clid;
        internal string path;
        internal string name;
        internal long size;
        internal long sizedone;
        internal uint clientftfid;
        internal uint serverftfid;
        internal uint sender;
        internal uint status; // TODO: Implement enumerator TS3FTStatus

        public uint ClientID { get { return clid; } }
        public string Path { get { return path; } }
        public string Name { get { return name; } }
        public long Size { get { return size; } }
        public long SizeDone { get { return sizedone; } }
        public uint ClientFileTransferID { get { return clientftfid; } }
        public uint ServerFileTransferID { get { return serverftfid; } }
        public uint SenderID { get { return sender; } } // 0 = Server
        public uint Status { get { return status; } }
    }

    public class TS3PasswordHashResponse : TS3ErrorableResponse // hashpassword
    {
        internal string passwordhash;

        public string PasswordHash { get { return passwordhash; } }
    }

    public class TS3MessageResponse : TS3ErrorableResponse
    {
        internal uint msgid;
        internal string cluid;
        internal string subject;
        internal string message;

        public uint MessageID { get { return msgid; } }
        public string SenderUniqueID { get { return cluid; } }
        public string Subject { get { return subject; } }
        public string Message { get { return message; } }
    }

    public class TS3MessageList : TS3EntriesCollection<TS3MessageListEntry> { }

    public class TS3MessageListEntry
    {
        internal uint msgid;
        internal string cluid;
        internal string subject;
        internal bool flag_read;

        public uint MessageID { get { return msgid; } }
        public string SenderUniqueID { get { return cluid; } }
        public string Subject { get { return subject; } }
        public bool IsRead { get { return flag_read; } }
    }

    public class TS3PermOverview : TS3EntriesCollection<TS3PermOverviewEntry> { }

    public class TS3PermOverviewEntry
    {
        internal uint t; // TODO: Implement enumerator TS3PermGroupType
        internal uint id1;
        internal uint id2;
        internal uint p;
        internal int v;
        internal bool n;
        internal bool s;

        public uint PermGroupType { get { return t; } }
        public uint ServerGroupID
        {
            get
            {
                if (PermGroupType == 0)
                    return id1;
                else
                    //return null;
                    throw new InvalidOperationException("This permission does not apply to a server group.");
            }
        }
        public uint ClientDatabaseID
        {
            get
            {
                if (PermGroupType == 1)
                    return id1;
                else if (PermGroupType == 3)
                    return id2;
                else
                    throw new InvalidOperationException("This permission does neither apply to a global client nor to a channel group.");
            }
        }
        public uint ChannelID
        {
            get
            {
                if (PermGroupType >= 2)
                    return id1;
                else
                    throw new InvalidOperationException("This permission is not referring to anything channel-related.");
            }
        }
        public uint ChannelGroupID
        {
            get
            {
                if (PermGroupType == 3)
                    return id2;
                else
                    throw new InvalidOperationException("This permission does not apply to a channel group.");
            }
        }
        public uint ID { get { return p; } }
        public int Value { get { return v; } }
        public bool Negated { get { return n; } }
        public bool Skip { get { return s; } }
    }

    public class TS3ServerConnectInfo : TS3ErrorableResponse
    {
        internal string ip;
        internal ushort port;
        internal string password;

        public string IP { get { return ip; } }
        public ushort Port { get { return port; } }
        public string Password { get { return password; } } // ... sigh ...
    }

    public class TS3ServerGroupResponse : TS3ErrorableResponse // servergroupadd
    {
        internal uint sgid;

        public uint ServerGroupID { get { return sgid; } }
    }

    public class TS3ServerGroupClientList : TS3EntriesCollection<TS3ServerGroupClientListEntry> { }

    public class TS3ServerGroupClientListEntry
    {
        internal uint cldbid;

        public uint ClientDatabaseID { get; set; }
    }

    public class TS3ServerGroupList : TS3EntriesCollection<TS3ServerGroupListEntry> { }

    public class TS3ServerGroupListEntry
    {
        internal uint sgid;
        internal string name;
        internal uint type; // TODO: Implement enumerator TS3ServerGroupType
        internal uint iconid;
        internal bool savedb;

        public uint ID { get { return sgid; } }
        public string Name { get { return name; } }
        public uint Type { get { return type; } }
        public uint IconID { get { return iconid; } }
        public bool SaveToDatabase { get { return savedb; } }
    }

    public class TS3ServerGroupsSearchResponse : TS3ErrorableResponse
    {
        internal uint sgid;
        internal string name;
        internal uint cldbid;

        public uint ServerGroupID { get { return sgid; } }
        public string Name { get { return name; } }
        public uint ClientDatabaseID { get { return cldbid; } }
    }

    public class TS3ServerVariableResponse : TS3ErrorableResponse
    {
        internal string virtualserver_name;
        internal string virtualserver_platform;
        internal string virtualserver_version;
        internal long virtualserver_created;
        internal bool virtualserver_codec_encryption_mode;
        internal uint virtualserver_default_server_group;
        internal uint virtualserver_default_channel_group;
        internal string virtualserver_hostbanner_url;
        internal string virtualserver_hostbanner_gfx_url;
        internal int virtualserver_hostbanner_gfx_interval;
        internal float virtualserver_priority_speaker_dimm_modificator;
        internal uint virtualserver_id;
        internal string virtualserver_hostbutton_tooltip;
        internal string virtualserver_hostbutton_url;
        internal string virtualserver_hostbutton_gfx_url;
        internal string virtualserver_name_phonetic;
        internal uint virtualserver_icon_id;
        internal string virtualserver_ip;
        internal bool virtualserver_ask_for_privilegekey;
        internal bool virtualserver_hostbanner_mode;

        // Can not be requested for other clients than yours because
        // they need to be requested from other clients.
        internal int virtualserver_clientsonline;
        internal int virtualserver_channelsonline;
        internal long virtualserver_uptime;
        internal bool virtualserver_flag_password;
        internal uint virtualserver_default_channel_admin_group;
        internal long virtualserver_max_download_total_bandwidth;
        internal long virtualserver_max_upload_total_bandwidth;
        internal uint virtualserver_complain_autoban_count;
        internal long virtualserver_complain_autoban_time;
        internal long virtualserver_complain_remove_time;
        internal int virtualserver_min_clients_in_channel_before_forced_silence;
        internal uint virtualserver_antiflood_points_tick_reduce;
        internal uint virtualserver_antiflood_points_needed_warning;
        internal uint virtualserver_antiflood_points_needed_kick;
        internal uint virtualserver_antiflood_points_needed_ban;
        internal long virtualserver_antiflood_ban_time;
        internal int virtualserver_client_connections;
        internal uint virtualserver_query_client_connections;
        internal uint virtualserver_queryclientsonline;
        internal long virtualserver_download_quota;
        internal long virtualserver_upload_quota;
        internal long virtualserver_month_bytes_downloaded;
        internal long virtualserver_month_bytes_uploaded;
        internal long virtualserver_total_bytes_downloaded;
        internal long virtualserver_total_bytes_uploaded;
        internal ushort virtualserver_port;
        internal bool virtualserver_autostart;
        internal uint virtualserver_machine_id;
        internal uint virtualserver_needed_identity_security_level;
        internal bool virtualserver_log_client;
        internal bool virtualserver_log_query;
        internal bool virtualserver_log_channel;
        internal bool virtualserver_log_permissions;
        internal bool virtualserver_log_server;
        internal bool virtualserver_log_filetransfer;
        internal string virtualserver_min_client_version;
        internal int virtualserver_reserved_slots;
        internal float virtualserver_total_packetloss_speech;
        internal float virtualserver_total_packetloss_keepalive;
        internal float virtualserver_total_packetloss_control;
        internal float virtualserver_total_packetloss_total;
        internal int virtualserver_total_ping;
        internal bool virtualserver_weblist_enabled;

        public string Name { get { return virtualserver_name; } }
        public string Platform { get { return virtualserver_platform; } }
        public string Version { get { return virtualserver_version; } }
        public long Created { get { return virtualserver_created; } }
        public bool CodecEncryptionMode { get { return virtualserver_codec_encryption_mode; } }
        public uint DefaultServerGroup { get { return virtualserver_default_server_group; } }
        public uint DefaultChannelGroup { get { return virtualserver_default_channel_group; } }
        public string HostbannerUrl { get { return virtualserver_hostbanner_url; } }
        public string HostbannerGfxUrl { get { return virtualserver_hostbanner_gfx_url; } }
        public int HostbannerGfxInterval { get { return virtualserver_hostbanner_gfx_interval; } }
        public float PrioritySpeakerDimmModificator { get { return virtualserver_priority_speaker_dimm_modificator; } }
        public uint ID { get { return virtualserver_id; } }
        public string HostbuttonTooltip { get { return virtualserver_hostbutton_tooltip; } }
        public string HostbuttonUrl { get { return virtualserver_hostbutton_url; } }
        public string HostbuttonGfxUrl { get { return virtualserver_hostbutton_gfx_url; } }
        public string NamePhonetic { get { return virtualserver_name_phonetic; } }
        public uint IconID { get { return virtualserver_icon_id; } }
        public string IP { get { return virtualserver_ip; } }
        public bool AskForPrivilegeKey { get { return virtualserver_ask_for_privilegekey; } }
        public bool HostbannerMode;
        public int ClientsOnline;
        public int ChannelsOnline;
        public DateTime Uptime { get { return DateTime.FromBinary(virtualserver_uptime); } }
        public bool HasPassword { get { return virtualserver_flag_password; } }
        public uint DefaultChannelAdminGroup { get { return virtualserver_default_channel_admin_group; } }
        public long MaxDownloadTotalBandwidth { get { return virtualserver_max_download_total_bandwidth; } }
        public long MaxUploadTotalBandwidth { get { return virtualserver_max_upload_total_bandwidth; } }
        public uint ComplainAutobanCount { get { return virtualserver_complain_autoban_count; } }
        public DateTime ComplainAutobanTime { get { return DateTime.FromBinary(virtualserver_complain_autoban_time); } }
        public DateTime ComplainRemoveTime { get { return DateTime.FromBinary(virtualserver_complain_remove_time); } }
        public int MinClientsInChannelBeforeForcedSilence { get { return virtualserver_min_clients_in_channel_before_forced_silence; } }
        public uint AntifloodPointsTickReduce { get { return virtualserver_antiflood_points_tick_reduce; } }
        public uint AntifloodPointsNeededWarning { get { return virtualserver_antiflood_points_needed_warning; } }
        public uint AntifloodPointsNeededKick { get { return virtualserver_antiflood_points_needed_kick; } }
        public uint AntifloodPointsNeededBan { get { return virtualserver_antiflood_points_needed_ban; } }
        public DateTime AntifloodBanTime { get { return DateTime.FromBinary(virtualserver_antiflood_ban_time); } }
        public int ClientConnections { get { return virtualserver_client_connections; } }
        public uint QueryClientConnections { get { return virtualserver_query_client_connections; } }
        public uint QueryClientsOnline { get { return virtualserver_queryclientsonline; } }
        public long DownloadQuota { get { return virtualserver_download_quota; } }
        public long UploadQuota { get { return virtualserver_upload_quota; } }
        public long MonthBytesDownloaded { get { return virtualserver_month_bytes_downloaded; } }
        public long MonthBytesUploaded { get { return virtualserver_month_bytes_uploaded; } }
        public long TotalBytesDownloaded { get { return virtualserver_total_bytes_downloaded; } }
        public long TotalBytesUploaded { get { return virtualserver_total_bytes_uploaded; } }
        public ushort Port { get { return virtualserver_port; } }
        public bool Autostart { get { return virtualserver_autostart; } }
        public uint MachineID { get { return virtualserver_machine_id; } }
        public uint NeededIdentitySecurityLevel { get { return virtualserver_needed_identity_security_level; } }
        public bool LogClient { get { return virtualserver_log_client; } }
        public bool LogQuery { get { return virtualserver_log_query; } }
        public bool LogChannel { get { return virtualserver_log_channel; } }
        public bool LogPermissions { get { return virtualserver_log_permissions; } }
        public bool LogServer { get { return virtualserver_log_server; } }
        public bool LogFileTransfer { get { return virtualserver_log_filetransfer; } }
        public string MinClientVersion { get { return virtualserver_min_client_version; } }
        public int ReservedSlots { get { return virtualserver_reserved_slots; } }
        public float TotalPacketlossSpeech { get { return virtualserver_total_packetloss_speech; } }
        public float TotalPacketlossKeepAlive { get { return virtualserver_total_packetloss_keepalive; } }
        public float TotalPacketlossControl { get { return virtualserver_total_packetloss_control; } }
        public float TotalPacketlossTotal { get { return virtualserver_total_packetloss_total; } }
        public int TotalPing { get { return virtualserver_total_ping; } }
        public bool WeblistEnabled { get { return virtualserver_weblist_enabled; } }
    }

    public class TS3TokenResponse : TS3ErrorableResponse // tokenadd
    {
        internal string token;

        public string Token { get { return token; } }
    }

    public class TS3TokenList : TS3EntriesCollection<TS3TokenListEntry> { }

    public class TS3TokenListEntry
    {
        internal string token;
        internal uint token_type; // TODO: Implement enumerator TS3TokenType
        internal uint token_id1;
        internal uint token_id2;
        internal string tokendescription;
        internal string tokencustomset;

        public string Token { get { return token; } }
        public uint Type { get { return token_type; } }
        public uint ID1 { get { return token_id1; } }
        public uint ID2 { get { return token_id2; } }
        public string Description { get { return tokendescription; } }
        public Dictionary<string, string> CustomSet
        {
            get
            {
                return tokencustomset.Split(' ').Select(i =>
                {
                    var j = i.Split('=');
                    return new KeyValuePair<string, string>(j[0], j[1]);
                }).ToDictionary(i => i.Key, i => i.Value);
            }
        }
    }

    public class TS3SelectResponse : TS3ErrorableResponse // use => selected *
    {
        internal uint schandlerid;

        public uint SCHandlerID { get { return schandlerid; } }
    }

    public class TS3WhoAmI : TS3ErrorableResponse
    {
        internal uint clid;
        internal uint cid;

        public uint ClientID { get { return clid; } }
        public uint ChannelID { get { return cid; } }
    }
}
