@echo off
title jsowl cgi
mode con: cols=61 lines=25
cls
echo -------------------------------------------------------------
echo - jsowl        minimal apache2 configuration with jsowl cgi -
echo - Navigate to http://127.0.0.1:8080 to see the magic happen -
echo -------------------------------------------------------------
echo - Do not use this server for something serious!             -
echo - This is a very unsecure apache2 configuration,            -
echo - made to demonstrate the jsowl cgi application.            -
echo -------------------------------------------------------------
echo - https://github.com/codeaddicts/jsowl                      -
echo -------------------------------------------------------------
echo - Close this window to stop the server.                     -
echo -------------------------------------------------------------
@cd bin
@httpd.exe >NUL