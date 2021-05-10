@echo off
set mypath=%~dp0
echo Folder of executing script %mypath%


echo cheching if %mypath%PXWeb\Web.config exists
if exist %mypath%PXWeb\Web.config (
  echo "Web.config already exist, so no copying."
) else (
  echo "Copying files:"
 copy %mypath%TemplateConfigFiles\*.config %mypath%PXWeb\.
) 

echo "Done."


