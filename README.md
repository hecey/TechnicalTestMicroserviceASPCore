# Microservice Sample

## https for you docker container on development environment

Create self signed certificate for each microservice:

```powershell
dotnet dev-certs https -ep $env:USERPROFILE\.aspnet\https\{NameOfService}.pfx -p pa55wOrd!
```

**Program.cs** must have an unique UserSecretsId

```C#
   <UserSecretsId>xxxx-xxx-xxx-xxx-xxx</UserSecretsId>
```

Run inside each microservice directory:

```powershell
dotnet user-secrets set "Kestrel:Certificates:Development:Password" "pa55wOrd!"
```

All about steps before docker compose build.

## Commands for running dockerfile for separate

```powershell
docker build --rm -t {NameOfService}:dev .
```

```powershell
docker run -p 5019:80 -p 7149:443 -e ASPNETCORE_URLS="https://+;http://+" -e ASPNETCORE_HTTPS_PORT=7149 -e ASPNETCORE_ENVIRONMENT=Development -v $env:APPDATA\microsoft\UserSecrets\:/root/.microsoft/usersecrets -v $env:USERPROFILE\.aspnet\https:/root/.aspnet/https/ {NameOfService}:dev
```

{NameOfService}
: Should be replace.

## Disable https verification for communication between docker containers using a HttpClientHandler

```C#
var isDevelopment=true;
//Be sure to remove this handler on production environment
builder.Services.AddHttpClient<RemoteClientService>(client => client.BaseAddress = new Uri($"https://{sqlServerSettings.Host}:7149/api"))
                .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler() {
                    // Return `true` to allow certificates that are untrusted/invalid
                    ServerCertificateCustomValidationCallback = (isDevelopment)?HttpClientHandler.DangerousAcceptAnyServerCertificateValidator:null
                });
```

This is not necessary when working outside docker in local environment. It was used to workaround an SSL validation error, otherwise you will need to use a CA-CERT to sign your certificates.

## Github packages configuration

Create a **Nuget.Config** file on each microservice root, so docker can used it for reaching github source

```C#
<?xml version="1.0" encoding="utf-8" ?>
<configuration>
  <packageSources>
    <clear />
    <add key="nuget.org" value="https://api.nuget.org/v3/index.json" protocolVersion="3" />
    <add key="github" value="https://nuget.pkg.github.com/{USER_NAME}/index.json"/>
  </packageSources>

  <packageSourceCredentials>
    <github>
      <add key="Username" value="{USER_NAME}"/>
      <add key="ClearTextPassword" value="{YOUR_KEY_VALUE}"/>
    </github>
  </packageSourceCredentials>
</configuration>
```

Inside your **Program.cs**

```C#
<ItemGroup>
    <PackageReference Include="FULL_QUALIFIED_PACKAGED_NAME" Version="1.0.0" />
    ...
</ItemGroup>
```

Further information about Github packages configuration: [Working with the NuGet registry](https://docs.github.com/en/packages/working-with-a-github-packages-registry/working-with-the-nuget-registry)