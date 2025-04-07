# Thao tác sử dụng ef tool tương tác với csdl

## Database Fisrt

### Tạo DbContext và các Models dựa trên DB đã có sẵn

Chuẩn bị: cài tool ef-core v7*

Các thư viện cần thiết

```powershell
# powerahell
dotnet ef dbcontext scaffold `
  "Server=dev.i-soft.com.vn;Port=5432;Database=iMagDb;User Id=iMag_dev;Password=P@ssw0rd;" `
  Npgsql.EntityFrameworkCore.PostgreSQL `
  --context iMagDbContext `
  --context-dir "DbContexts\" `
  --output-dir "Models\" `
  --namespace SourceBaseBE.Database.Models `
  --context-namespace SourceBaseBE.Database.DbContexts
```

```shell
# CMD
dotnet ef dbcontext scaffold ^
  "Server=dev.i-soft.com.vn;Port=5432;Database=test_iMag_conn;User Id=postgres;Password=P@ssw0rd;" ^
  Npgsql.EntityFrameworkCore.PostgreSQL ^
  --context iMagDbContext ^
  --context-dir "DbContexts\" ^
  --output-dir "Models\" ^
  --namespace SourceBaseBE.Database.Models ^
  --context-namespace SourceBaseBE.Database.DbContexts
```

## Code First

### Tạo các table dựa trên DbContext và các class Models đã có

```powershell
# powershell

# tạo migrations 
dotnet ef migrations add "migration_name" `
    --context "iMag.Database.DbContexts.iMagDbContext" `
    --output "Migrations/"


# cập nhật DB
dotnet ef database update <migration_file_name> `
    --context "iMag.Database.DbContexts.iMagDbContext"
```

```shell
# CMD
dotnet ef migrations add "migration_name" ^
--context "iMag.Database.DbContexts.iMagDbContext" ^
--output "Migrations/"


# cập nhật DB
dotnet ef database update <migration_file_name> ^
    --context "iMag.Database.DbContexts.iMagDbContext"
```
