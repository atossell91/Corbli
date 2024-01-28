dotnet publish --configuration release Corbli
cp -r Corbli/bin/Release/net8.0/publish/* $CorbliDir
systemctl restart $CorbliService
