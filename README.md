# 锦客铁路管理系统 (DMY Railway System / TieLu System)

[![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE)

## 项目概述 (Project Overview)

**dmy-railway-system** 是一个完整的铁路运营管理系统，专为锦州客运段 (Jinzhou Railway Passenger Segment) 设计。主要用于干部考核、公文流转、任务管理、在线考试、规章查询、职工诉求等铁路业务管理。

- **移动端 (Mobile)**: Android 原生应用 \"锦客手机端\" (`com.damytech.TieLu`)
- **Web 端 (Web)**: ASP.NET MVC 管理平台 `TLSite`
- **服务端 (Backend)**: `TLWebService` Web 服务
- **数据库 (Database)**: SQL Server `Tielu.mdf`

系统支持离线查询、在线考核、文档流转等功能。

## 系统架构 (Architecture)

```
Android App <--> TLWebService <--> TLSite (ASP.NET MVC) <--> Tielu DB (SQL Server)
                        |
                  Documents / Prototypes
```

## 主要功能 (Key Features)

### Android 移动端功能:
- **干部考核 (Performance Assessment)**: 考核录入、查询、分析
- **公文流转 (Document Circulation)**: 发布、待签、已收、查询、流转
- **任务管理 (Task Management)**: 添加、查看、报告完成
- **规章查询 (Rules Query)**: 规章浏览、详情
- **在线考试 (Online Exams)**: 段级/车队考试、结果分析
- **职工诉求 (Employee Appeals)**: 提交、查看处理
- **通讯录/邮箱 (Contacts/Email)**: 查询、发送
- **积分/自控率查询 (Credit/Self-Control Rate Query)**

### Web 管理端功能 (TLSite):
- 用户/角色管理 (UserController)
- 检查/考核 (Check/CheckInfoController)
- 积分管理 (CreditController)
- 文档管理 (DocumentController)
- 考试管理 (ExamController)
- 判断/评分 (JudgeController)
- 邮件/意见 (Mail/OpinionController)
- 车次/线路/车队 (TrainNo/Route/TeamController)
- 任务/规章/部门 (Task/Rule/SectorController)

## 技术栈 (Tech Stack)

| Component | Technologies |
|-----------|--------------|
| Android | Java, minSdk 11, AChartEngine charts |
| Web | ASP.NET MVC 3 (.NET 4.0), LINQ to SQL, jQuery, jqGrid, Bootstrap, Ace Admin, KindEditor |
| Backend | C#, WCF/SOAP WebService |
| DB | SQL Server (Tielu.mdf) |
| UI | DataTables, FullCalendar, Colorbox |

## 环境要求 (Prerequisites)

- **Android**: Eclipse/ADT 或 Android Studio (legacy support), SDK API 11+
- **Web**: Visual Studio 2010+, IIS 或 IIS Express, .NET Framework 4.0
- **DB**: SQL Server 2008+ (Express ok)
- **Service**: Visual Studio 或 IIS hosting

## 安装与运行 (Setup & Run)

### 1. 数据库 (Database)
```
# 1. Attach DB files
# 使用 SSMS: 右键 Databases > Attach > 添加 Tielu.mdf / Tielu_log.ldf (Database/)
```
`cd Database && sqlcmd -S . -Q "CREATE DATABASE Tielu ON (FILENAME = 'Tielu.mdf'), (FILENAME = 'Tielu_log.ldf') FOR ATTACH"`

查看设计: `Document/Develop/铁路_数据库设计书_CHN.xlsx`

### 2. WebSite (TLSite)
```
# Visual Studio:
devenv WebSite/TLSite/TLSite.sln

# 或 IIS Express:
cd WebSite/TLSite
# 编辑 ConnStrings.config (连接字符串)
# F5 或 Ctrl+F5 运行 (默认 http://localhost:12645)
```

配置: Web.config (authentication Forms, zh-CN culture)

### 3. WebService
```
devenv WebService/TLWebService/TLWebService.sln
# Build & Run (WCF 测试客户端)
```

### 4. Android App
```
# Eclipse (legacy):
# Import Android/ as project
# Build APK (AndroidManifest.xml version 1.0)
# Install: adb install bin/*.apk

# 或 Android Studio:
# 导入项目, 更新 Gradle (old project)
```

**注意**: 项目较旧 (2012), 可能需兼容模式运行。


## 贡献 (Contributing)
1. Fork 项目
2. 创建 feature 分支
3. Commit & PR

## 许可证 (License)
MIT License (推测, 无 LICENSE 文件)

---

**English Version Above / 中文版本如上**

