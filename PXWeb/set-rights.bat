icacls C:\inetpub\wwwroot\PXWeb\Resources\PX\Databases /grant "IIS APPPOOL\DefaultAppPool":(OI)(CI)M
icacls C:\inetpub\wwwroot\PXWeb\Resources\Languages /grant "IIS APPPOOL\DefaultAppPool":(OI)(CI)M
icacls C:\inetpub\wwwroot\PXWeb\Logs /grant "IIS APPPOOL\DefaultAppPool":(OI)(CI)M
icacls C:\inetpub\wwwroot\PXWeb\setting.config /grant "IIS APPPOOL\DefaultAppPool":(OI)(CI)M
icacls C:\inetpub\wwwroot\PXWeb\setting.config /grant "IIS APPPOOL\DefaultAppPool":M
icacls C:\inetpub\wwwroot\PXWeb\ /grant "IIS APPPOOL\DefaultAppPool":(OI)(CI)M
