# MedicalMask
- - -

## 以EF Core整合SQLite儲存口罩剩餘數量資訊  

* 建立API串接第三方平台
    1. 製作 Model
    2. 撰寫 Service
    3. 注冊 Service
    4. 呼叫 Service

* 以EF Core整合SQLite儲存口罩剩餘數量資訊
    * 安裝 SQLITE  
        ```csharp
        # PowerShell
        Install-Package Microsoft.EntityFrameworkCore.Sqlite -Version 5.0.1
        # .NET Core
        dotnet add package Microsoft.EntityFrameworkCore.Sqlite --version 5.0.1
        ```
    * 進行 MIGRATION  
        如果未安裝**EF Core 命令列工具 (dotnet ef)**，須額外安裝
        ```csharp
        # PowerShell
        Install-Package Microsoft.EntityFrameworkCore.Tools -Version 5.0.1
        Install-Package Microsoft.EntityFrameworkCore.Design -Version 5.0.1
        Add-Migration Initial
        Update-Database
        # .NET Core
        dotnet tool install --global dotnet-ef
        dotnet add package Microsoft.EntityFrameworkCore.Design
        dotnet ef migrations add Initial
        dotnet ef database update
        ```
        * 驗證安裝
            ```csharp
            # PowerShell
            Get-Help about_EntityFrameworkCore
            # .NET Core
            dotnet ef 
            ```
            輸出後會看到獨角獸！  
            ```csharp
                         _/\__  
                   ---==/    \\  
             ___  ___   |.    \|\  
            | __|| __|  |  )   \\\  
            | _| | _|   \_/ |  //|\\  
            |___||_|       /   \\\/\\  
            ```  
            

## 整合Hangfire來排程更新口罩剩餘數量資料  
* 安裝 HANGFIRE + SQLITE 套件  
    ```csharp
    # PowerShell
    Install-Package HangFire.Core -Version 1.7.18
    Install-Package Hangfire.AspNetCore -Version 1.7.18
    Install-Package Hangfire.Storage.SQLite -Version 0.2.4
    Install-Package sqlite-net-pcl -Version 1.7.335
    ```
* 加入 HANGFIRE 組態
    ```csharp
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddHangfire(configuration => configuration
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UseSQLiteStorage());
        services.AddHangfireServer();
    }
    ```
    先測試HangFire是否正常執行。  
    另外，recurringJob 支援Cron 設定。  
    ```csharp
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseHangfireDashboard();
        BackgroundJob.Enqueue(() => Console.WriteLine("Hello HangFire."));
        RecurringJob.AddOrUpdate("Hello", () => Console.WriteLine("Hello, recurringJob."), Cron.Minutely());
    }
    ```
    因為 Hangfire 走的是 MVC 路由，而我們開的是 Web API 專案，預設情況下吃不到 MVC 路由。
    ```csharp
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
        });
    }
    ```  
    
- - -  
### 參考資料：  

<a href="https://blog.kkbruce.net/2020/02/ef-core-sqlite.html#.X_LWB9j7SUk" target="_blank">簡單五步驟：以EF Core整合SQLite儲存口罩剩餘數量資訊</a>  
<a href="https://blog.kkbruce.net/2020/04/aspnet-core-hangfire-sqlite.html#.X_15Ouj7SUk" target="_blank">
簡單五步驟：ASP.NET CORE整合HANGFIRE來排程更新口罩剩餘數量資料</a>  
<a href="https://data.gov.tw/dataset/116285">政府資料開放平台：健保特約機構口罩剩餘數量明細清單</a>  
<a href="https://crontab.guru/">Crontab guru - 快速測試出所需的 Cron 設定</a>  
<a href="https://stackoverflow.com/questions/58986882/asyncenumerablereader-reached-the-configured-maximum-size-of-the-buffer-when-e">'AsyncEnumerableReader' reached the configured maximum size of the buffer when enumerating a value</a>
![](AsyncEnumerableReader%20reached%20the%20configured%20maximum%20size.png)
* 假設回傳資料量過大會跳出上述錯誤
    1. 別傳太多資料
    2. 修改回傳上限(預設8192)
        ```csharp
        services.AddControllers(options => options.MaxIAsyncEnumerableBufferLimit = N)
        ```
    3. 不要使用IAsyncEnumerable  
        用ToList()之類的回傳
