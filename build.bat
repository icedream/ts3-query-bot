@echo off

path %path%;C:\Windows\Microsoft.NET\Framework64\v4.0.30319
path %path%;C:\Windows\Microsoft.NET\Framework\v4.0.30319

msbuild Bot.sln /m /nologo /maxcpucount /v:m /p:Configuration="Debug"
msbuild Bot.sln /m /nologo /maxcpucount /v:m /p:Configuration="Release"