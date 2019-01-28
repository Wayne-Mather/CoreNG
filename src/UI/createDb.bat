@echo off
del *.db

dotnet ef database update --context ApplicationDbContext
dotnet ef database update --context CoreNgDbContext
