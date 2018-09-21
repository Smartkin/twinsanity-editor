@ECHO OFF
MKDIR Libraries
MKDIR x64
MKDIR x86
COPY /Y ..\..\packages\SharpFont.Dependencies.2.6\bin\msvc12\x64 x64
COPY /Y ..\..\packages\SharpFont.Dependencies.2.6\bin\msvc12\x86 x86
MOVE /Y *.dll Libraries
RMDIR /S /Q packages
echo Finished...