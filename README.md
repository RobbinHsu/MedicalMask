# MedicalMask


實作內容：  

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



---  
<a href="https://blog.kkbruce.net/2020/02/ef-core-sqlite.html#.X_LWB9j7SUk" target="_blank">KingKong Bruce記事的 - 簡單五步驟：以EF Core整合SQLite儲存口罩剩餘數量資訊</a>

<a href="https://data.gov.tw/dataset/116285">政府資料開放平台：健保特約機構口罩剩餘數量明細清單</a>
