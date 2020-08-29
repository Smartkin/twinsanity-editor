@ECHO OFF
MKDIR Libraries
MKDIR x64
MKDIR x86
COPY /Y ..\..\packages\SharpFont.Dependencies.2.6\bin\msvc12\x64 x64
COPY /Y ..\..\packages\SharpFont.Dependencies.2.6\bin\msvc12\x86 x86
MOVE /Y *.dll Libraries
COPY /Y %1\Libx64\*.dll x64
COPY /Y %1\Libx86\*.dll x86
echo "Renaming 64-bit libraries"
SETLOCAL enabledelayedexpansion
SET FOLDER_PATH="x64\*64.dll"
for %%f in (%FOLDER_PATH%) do (
    echo %%f
    SET "filename=%%~nf"
    MOVE /Y "%%f" "x64\!filename:~0,-2!%%~xf"
)
RMDIR /S /Q packages
echo Finished... 