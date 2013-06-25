@echo off

path %path%;C:\Windows\Microsoft.NET\Framework64\v4.0.30319
path %path%;C:\Windows\Microsoft.NET\Framework\v4.0.30319

echo Building debug binaries...
msbuild Bot.sln /m /nologo /maxcpucount /v:m /p:Configuration="Debug"
echo Building release binaries...
msbuild Bot.sln /m /nologo /maxcpucount /v:m /p:Configuration="Release"