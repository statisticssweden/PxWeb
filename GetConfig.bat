@echo off
set mypath=%~dp0
echo Folder of executing script %mypath%

FOR %%f in (%mypath%TemplateConfigFiles\*.config) DO (
  IF EXIST "%mypath%PXWeb\%%~nxf" ( 
    echo File "%mypath%PXWeb\%%~nxf" exists,  so no copying. 
  ) else ( 
    echo Copying %%~nxf
    COPY "%mypath%TemplateConfigFiles\%%~nxf" "%mypath%PXWeb\%%~nxf" 
  )
)

echo "Done."


