# CleanArchitectureGym
This is an example that from a CleanArchitecture Course.


# Update Sqlite DB schema

Create migration
```
dotnet ef migrations add <name> -p Code/GymManagement.Infrastructure -s Code/GymManagement.Api
```

Update Sqlite file

``` 
dotnet ef database update -p Code/GymManagement.Infrastructure -s Code/GymManagement.Api
```
