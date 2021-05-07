@echo off
if exist PXWeb\Web.config (
  echo "Web.config already exist, so no copying."
) else (
  echo "Copying files:"
  copy TemplateConfigFiles\*.config PXWeb\.
) 

echo "Done."


