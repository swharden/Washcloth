@echo off

echo ### DELETING OLD PACKAGES ###
del ..\..\src\Washcloth\bin\Release\*.nupkg
del ..\..\src\Washcloth\bin\Release\*.snupkg

echo.
echo ### BUILDING NEW PACKAGES ###
dotnet build ..\..\src --verbosity quiet --configuration Release

echo.
echo ### RUNNING TESTS ###
dotnet test ..\..\src --verbosity quiet --configuration Release

echo.
echo ### WARNING! ### 
echo This script will UPLOAD packages to nuget.org
echo.
echo press ENTER 3 times to proceed...
pause
pause
pause

echo.
echo ### UPDATING NUGET ###
nuget update -self

echo.
echo ### UPLOADING TO NUGET ###
nuget push ..\..\src\Washcloth\bin\Release\*.nupkg -Source https://api.nuget.org/v3/index.json

pause