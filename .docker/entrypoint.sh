#!/bin/bash
cd /home/node/app
dotnet tool restore
dotnet paket install
dotnet saturn migration
dotnet fake build -t Run