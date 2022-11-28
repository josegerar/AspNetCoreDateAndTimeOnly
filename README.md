[![Nuget version](https://www.nuget.org/Content/gallery/img/default-package-icon.svg)](https://www.nuget.org/packages/AspNetCoreDateAndTimeOnly/)
[![Nuget downloads](https://www.nuget.org/Content/gallery/img/default-package-icon.svg)](https://www.nuget.org/packages/AspNetCoreDateAndTimeOnly/)

# AspNetCoreDateAndTimeOnly
A library that supports DateOnly and TimeOnly data types for AspNetCore.

## Installing
You can also install via the .NET CLI with the following command:
```
dotnet add package AspNetCoreDateAndTimeOnly
```
If you're using Visual Studio you can also install via the built in NuGet package manager.

## Usage
To support DateOnly and TimeOnly in SqlServer EntitityFrameworkCore add the `AddSuportDateAndTimeSqlServer` extension after `UseSqlServer`.

```csharp
builder.Services.AddDbContext<TContext>(options =>
        {
            options.UseSqlServer(conection);
            options.AddSuportDateAndTimeSqlServer();
        });
```
## .Net 6
In net 6 you have to add a TypeDescriptor to the MvcBuilder with the `UseDateOnlyTimeOnlyStringConverters` extension and converters with the `AddDateAndTimeJsonConverters` extension in `AddJsonOptions` of the MvcBuilder.

```csharp
builder.Services.AddControllers(options =>
        {
            options.UseDateOnlyTimeOnlyStringConverters();
        })
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.Converters.AddDateAndTimeJsonConverters();
        });
```
