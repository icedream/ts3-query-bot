using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace TS3Query
{
    public class TS3QueryBotPlugin
    {
        [Import(typeof(TS3Query))]
        internal TS3Query _query = null;
        public TS3Query Client { get { return _query; } }

        [Import(typeof(TS3QueryBot))]
        internal TS3QueryBot _host = null;
        public TS3QueryBot Host { get { return _host; } }

        protected string PluginFolderPath { get { return Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + Path.DirectorySeparatorChar, "plugins"); } }
        protected string ConfigFolderPath { get { return Path.Combine(PluginFolderPath, "config_" + Path.GetFileNameWithoutExtension(Assembly.GetCallingAssembly().Location).Substring(7).ToLower()); } }
        
        public virtual void HandleResponse(TS3QueryResponse response)
        {
            return;
        }

        public virtual void HandleRequest(TS3QueryRequest request)
        {
            return;
        }

        public virtual void HandleCommand(TS3BotCommand command)
        {
            return;
        }

        public virtual void Initialize()
        {
            return;
        }

        internal PluginMetadataAttribute Metadata
        {
            get
            {
                return GetType().GetCustomAttributes(
                    typeof(PluginMetadataAttribute), false
                ).First() as PluginMetadataAttribute;
            }
        }

        internal IEnumerable<TS3QueryBotPluginMethod> Commands
        {
            get
            {
                return GetType().GetMethods().Where(pc => pc.GetCustomAttributes(typeof(PluginCommandAttribute), true).Any()).Select(pc => new TS3QueryBotPluginMethod(pc));
            }
        }
    }

    internal class TS3QueryBotPluginMethod
    {
        public TS3QueryBotPluginMethod(MethodInfo methodInfo)
        {
            this.MethodInfo = methodInfo;
        }

        public MethodInfo MethodInfo { get; private set; }

        public PluginCommandAttribute Metadata
        {
            get
            {
                return MethodInfo.GetCustomAttributes(typeof(PluginCommandAttribute), false).First() as PluginCommandAttribute;
            }
        }
        public IEnumerable<TS3QueryBotPluginMethodParameter> Parameters
        {
            get
            {
                return MethodInfo.GetParameters()/*.Where(p => p.GetCustomAttributes(typeof(PluginCommandParameterAttribute), false).Any())*/.Select(p => new TS3QueryBotPluginMethodParameter(p));
            }
        }
    }

    internal class TS3QueryBotPluginMethodParameter
    {
        public TS3QueryBotPluginMethodParameter(ParameterInfo p)
        {
            ParameterInfo = p;
        }

        public ParameterInfo ParameterInfo { get; private set; }

        public PluginCommandParameterAttribute Metadata
        {
            get
            {
                return ParameterInfo.GetCustomAttributes(typeof(PluginCommandParameterAttribute), false).Single() as PluginCommandParameterAttribute;
            }
        }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple=false, Inherited=false)]
    public class PluginMetadataAttribute : Attribute
    {
        public PluginMetadataAttribute(string name, string author)
            : this(name, author, System.Reflection.Assembly.GetExecutingAssembly().GetName().Version)
        {
        }

        public PluginMetadataAttribute(string name, string author, Version version)
        {
            this.Name = name;
            this.Author = author;
            this.Version = version;
        }

        public string Name { get; private set; }
        public string Author { get; private set; }
        public Version Version { get; private set; }

        public string Description { get; set; }
        public string ShortDescription { get; set; }
        public Version CompatibilityVersion { get; set; }
    }

    public class PluginCommandAttribute : Attribute
    {
        public PluginCommandAttribute(string name)
        {
            this.Name = name;
            this.Deprecated = false;
            this.TargetMode = TS3MessageTargetMode.Channel;
        }

        public string Name { get; private set; }
        public TS3MessageTargetMode TargetMode { get; set; }

        public bool Deprecated { get; set; }
        public string Description { get; set; }
        public string ShortDescription { get; set; }
    }

    public class PluginCommandParameterAttribute : Attribute
    {
        public PluginCommandParameterAttribute(string name)
        {
            this.Name = name;
        }

        public string Name { get; private set; }

        public string Description { get; set; }
        public string ShortDescription { get; set; }
    }
}
