# Halcyon .NET Api

A .NET Core REST api project template.

[https://halcyon-dotnet-api.chrispoulter.com](https://halcyon-dotnet-api.chrispoulter.com)

**Technologies used:**

- .NET Core
  [https://dotnet.microsoft.com](https://dotnet.microsoft.com)
- Entity Framework Core
  [https://docs.microsoft.com/en-us/ef/core](https://docs.microsoft.com/en-us/ef/core)

**External authentication providers:**

- Facebook
  [https://developers.facebook.com](https://developers.facebook.com)
- Google
  [https://console.developers.google.com](https://console.developers.google.com)

#### Custom Settings

Create `appsettings.Development.json` file in project directory or configure user secrets.

```
{
	"ConnectionStrings": {
		"DefaultConnection": ""
	},
	"Jwt": {
		"SecurityKey": ""
	},
	"Seed": {
		"EmailAddress": "",
		"Password": ""
	},
	"Email": {
		"Host": "",
		"Port": "",
		"Username": "",
		"Password": ",
		"NoReply": ""
	},
	"Authentication": {
		"Facebook": {
			"AppId": "",
			"AppSecret": "
		},
		"Google": {
			"ClientId": ""
		}
	}
}
```
