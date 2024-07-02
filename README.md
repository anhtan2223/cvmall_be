## Stack

- .Net 6 (C# 10)
- Entity Framework 6.0.3
- Databse: PostgreSQL 14.02

## Design pattern

- Repository & UnitOfWork
- Clean Architecture

## IDE & Tools

- Visual Studio 2022 Pro
- DBeaver
- Postman

## App config

- ### For prod
  `DMS.Api\appsettings.json`
- ### For dev
  `DMS.Api\appsettings.Development.json`

## Migration

- Comment code below to run migration `Application\Application.Common\Extensions\AddApplicationCommonServices.cs`
  ```cs
  // c.RegisterValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());
  ```
- Then

  ```sh
  dotnet ef migrations add "init_databse" --project Infrastructure --startup-project Presentation/WebAPI --context "ApplicationDbContext"
  ```
  
  ```sh
  dotnet ef database update --project Infrastructure --startup-project Presentation/WebAPI --context "ApplicationDbContext"
  ```
- Remove migrations
  ```sh
  dotnet ef database drop --project Infrastructure --startup-project Presentation/WebAPI --context "ApplicationDbContext"
  ```
  ```sh
  dotnet ef migrations remove --project Infrastructure --startup-project Presentation/WebAPI --context "ApplicationDbContext"
  ```

  ## To run Project
  ```sh
  cd Presentation/WebAPI && dotnet run
  ```

## Create new API

1. Create new controller in `DMS.Api\Controllers`
2. Create new service in `DMS.Business\*****`
3. Register service `Business.****.Extensions`
   ```cs
   services.AddScoped<I*****Services, *****Services>();
   ```
4. Create new request model in `DMS.Business\Business.Core\*****Request`
5. Create model validation using `FluentValidation`

   ```cs
   public class UserRequest
   {
       public string code { get; set; }
   }

   public class UserRequestValidator : AbstractValidator<UserRequest>
       {
           public UserRequestValidator()
           {
               RuleFor(_ => _.code).NotEmpty().WithMessage("code must be set");
           }
       }
   ```

## Profile mapper

- Register new mapper in `DMS.Api\ProfileMapper`

  ```cs
  CreateMap<RequestDTO, ENTITY_NAME>();
  ```

## Register new entity model

1. Create class in `DMS.Model\Model.Core\Entities`
2. Register in `DMS.Model\Model.Core\CoreDbContext`

   ```cs
   public virtual DbSet<NEW_ETITY>? NEW_ETITYs { get; set; }
   ```

3. Add migration
   ```sh
   dotnet-ef migrations add "udpate_databse" --project .\DMS.Model\Model.Core --startup-project .\DMS.Api --context "CoreDbContext"
   ```
4. Update databse
   ```sh
   dotnet-ef database update --project .\DMS.Model\Model.Core --startup-project .\DMS.Api --context "CoreDbContext"
   ```

## Cache

```cs
private readonly ICachingService cachingService;

// CTOR
public ******Services(ICachingService _cachingService)
{
    cachingService = _cachingService;
}

string cacheKey = $"{table_name}_{params_key}";
var data = cachingService.Get(cacheKey, () =>
            {
                // Call back function
                return data.Count() > 0 ? data: null;
            });
```

## Authen & Author

- Add action filter before api controller

```cs
[BAuthorize]
[ApiController]

[BAuthorize]
[Route("refresh")]
```

## Locallize & translate message

- Resource is cached in memory cache

```cs
protected ILocalizeServices ls { get; set; }

public ******Controller(ILocalizeServices _localServices)
{
    ls = _localServices;
}

// ls.Get("Module name", "Screen name", "resource key")
message = ls.Get(Modules.CORE, "User", "CreateSuccess");
```

## Import excel

## Export excel

```cs
// Get data export
var dataResponse = await masterCodeServices.GetPaged(request);

// Define column export
List<ExcelItem> excelItems = new List<ExcelItem>()
{
    new ExcelItem() { header = ls.Get(Modules.CORE, Screen.MASTERCODE, MessageKey.TYPE), key = "type", type = DataType.TEXT, header_align = CellAlign.CENTER, content_align = CellAlign.LEFT },
    new ExcelItem() {  header = ls.Get(Modules.CORE, Screen.MASTERCODE, MessageKey.KEY), key = "key", type = DataType.TEXT, header_align = CellAlign.CENTER, content_align = CellAlign.LEFT },
    new ExcelItem() {  header = ls.Get(Modules.CORE, Screen.MASTERCODE, MessageKey.VALUE), key = "value", type = DataType.TEXT, header_align = CellAlign.CENTER, content_align = CellAlign.LEFT },
    new ExcelItem() {  header = ls.Get(Modules.CORE, "MasterCode", "asd"), key = "created_at", type = DataType.TEXT, header_align = CellAlign.CENTER, content_align = CellAlign.LEFT }
};

// Export data to byte array
// ExcelHelpers.Export(list_data, config_export);
var fileStream = await ExcelHelpers.Export(dataResponse.data.ToList(), excelItems);

// Return file
return File(fileStream, "application/octetstream", file_name);

```

## Import CSV

## Export CSV

```cs
// Get data export
var dataResponse = await masterCodeServices.GetPaged(request);

// Define column export
List<CsvItem> csvItems = new List<CsvItem>()
{
    new CsvItem() { header = ls.Get(Modules.CORE, Screen.MASTERCODE, MessageKey.TYPE), key = "type", type = DataType.TEXT },
    new CsvItem() {  header = ls.Get(Modules.CORE, Screen.MASTERCODE, MessageKey.KEY), key = "key", type = DataType.TEXT },
    new CsvItem() {  header = ls.Get(Modules.CORE, Screen.MASTERCODE, MessageKey.VALUE), key = "value", type = DataType.TEXT }
};

// Export data to byte array
// CsvHelpers.ExportCsv(list_data, config_export);
var fileStream = await CsvHelpers.ExportCsv(dataResponse.data.ToList(), csvItems);

// Return file
return File(fileStream, "application/octetstream", file_name);
```

# Build & deploy

## System info

- OS: Ubuntu 20.04
- Docker v20.10.16
- Docker-componse v2.5.0

## Directory layout

    .DMS
    ├── docker-compose.yml	# Compiled files
    ├── nginx
    │   └── conf.d
    │       ├── web_admin.conf   # config nginx web_admin
    │       └── web_api.conf   	 # config nginx web_api
    ├── web_admin              	 # source web admin
    └── web_api                  # source web api

## Command

```sh
# run all container
docker-compose up -d --build --force-recreate

# remove all container
docker rm -f $(docker ps -a -q)

# remove all volumns
docker volume rm $(docker volume ls -q)

# remove images
docker rmi $(docker images -a )

# restart specific container
docker restart ***container_name***

```

## Change environment

- Web admin

1. Create new env file \
   `.env.***BUILD_MODE***`
2. Update build mode in `Dockerfile` \
   `RUN yarn build --mode ***BUILD_MODE***`

- Web api

1. Create new setting file \
   `appsettings.***BUILD_MODE***.json`
2. Update build mode in `docker-compose.yml`
   `- ASPNETCORE_ENVIRONMENT=***BUILD_MODE***`

## folder upload environment

1. change environment folder upload
   `- ASPNETCORE_DIR_UPLOAD=***FOLDER_UPLOAD***`
