# Halcyon .NET Api

A .NET Core api project template.

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
		"DefaultConnection": "Server=tcp:localhost,1433;Initial Catalog=halcyon;Persist Security Info=False;User ID=sa;Password=yourStrong(!)Password;MultipleActiveResultSets=False;Connection Timeout=30;"
	},
	"Jwt": {
		"SecurityKey": "nIgsA9xbC7Ts256l"
	},
	"Seed": {
		"EmailAddress": "admin@chrispoulter.com",
		"Password": "Testing123"
	},
	"Email": {
		"Host": "smtp.mailgun.org",
		"Port": "587",
		"Username": "#REQUIRED#",
		"Password": "#REQUIRED#",
		"NoReplyAddress": "noreply@chrispoulter.com"
	},
	"Authentication": {
		"Facebook": {
			"AppId": "#REQUIRED#",
			"AppSecret": "#REQUIRED#"
		},
		"Google": {
			"ClientId": "#REQUIRED#"
		}
	}
}
```