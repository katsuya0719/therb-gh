@echo off
set SELF_PATH=%~dp0
cd /d %SELF_PATH%

cd %1\Grasshopper\Libraries
call :SuccessOrDie
copy %2%3*.gha .\
call :SuccessOrDie

goto end

:SuccessOrDie
if not %errorlevel% == 0 (
    echo [ERROR]
    exit 1
)
exit /b 0

:end
echo [SUCCESS]
echo on
exit 0