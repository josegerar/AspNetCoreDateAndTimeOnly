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
You can use the `ToDateOnly` extension function in queries to compare Datetime with DateOnly.

```csharp
queryable.Where(t => t.DatetimeProperty.ToDateOnly() == dateOnlyParameter)
```
You can use the `ToDateTime` extension function in queries to compare Datetime with DateOnly.

```csharp
queryable.Where(t => t.DateOnlyProperty.ToDateTime() == dateTimeParameter)
```
## .Net 6
In net 6 you have to add a TypeDescriptor to the MvcBuilder with the `UseDateOnlyTimeOnlyStringConverters` extension of the MvcBuilder.

```csharp
builder.Services.AddControllers(options =>
        {
            options.UseDateOnlyTimeOnlyStringConverters();
        });
```
And converters with the `AddDateAndTimeJsonConverters` extension in `AddJsonOptions` of the MvcBuilder of the nuget library `AspNetCoreDateAndTimeOnly.Json`.

```csharp
builder.Services.AddControllers()
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.Converters.AddDateAndTimeJsonConverters();
        });
```
## Swagger
Add DateOnly and TimeOnly support for Swagger with the `UseDateOnlyTimeOnlyStringConverters` extension method.
```csharp
builder.Services.AddSwaggerGen(c => c.UseDateOnlyTimeOnlyStringConverters());
```
