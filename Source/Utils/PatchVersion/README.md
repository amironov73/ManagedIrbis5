### PatchVersion

Простая консольная утилита для патчинга версии в файле проекта. Командная строка:

```
PatchVersion <project> <version>
```

где 

* `project` - имя файла проекта,
* `version` - номер версии в формате, совместимом с .NET.

Пример:

```bash
PatchVersion ManagedIrbis.csproj 1.2.3.400
```

Утилита предназначена для применения в среде Continuous Integration, например, AppVeyor.com:

```bash
dotnet run --project Source/Utils/PatchVersion/PatchVersion.csproj Source/Common/ArsMagna.targets %APPVEYOR_BUILD_VERSION%
```

