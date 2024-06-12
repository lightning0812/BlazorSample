using Blazor_GetAppSettingsJson.Data;
using Blazor_GetAppSettingsJson.Entities;

// .NET の WebApplication.CreateBuilder(args) メソッドが自動的に環境に応じた設定ファイル
// （appsettings.Development.jsonなど）を読み込む
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();

// ↓↓↓　【ここにコードを追加】　　　　　　　　　　　　　　　　　　　　　　　　　↓↓↓
// ↓↓↓　appsettings.jsonからSettingの構造を読み込み、Settingクラスにマッピング　↓↓↓
builder.Services.Configure<Setting>(builder.Configuration.GetSection("Setting"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
