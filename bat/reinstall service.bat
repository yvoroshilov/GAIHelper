if not defined DevEnvDir (
    call "C:\Program Files (x86)\Microsoft Visual Studio\2019\Community\VC\Auxiliary\Build\vcvarsall.bat" x86_amd64
)
sc stop GAIHelperWcfService
sc delete GAIHelperWcfService
cd /d D:\labs\kursach\GAIHelper\WcfServiceHost\bin\Debug
InstallUtil WcfServiceHost.exe
sc start GAIHelperWcfService
cd /d D:\labs\kursach\GAIHelper\bat