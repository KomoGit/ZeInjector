# ZeInjector

**ZeInjector** is a package I made to solve a specific problem when it comes to injecting services in various lifecycles (Singleton, Scoped, Transient). I really disliked using Autofac so I decided I should create my simplified version of it.

## How to use it:
**Program.cs**
```
using ZeInjector;
.
.
.
AccessPoint.ConfigureServices(builder.Services);
var app = builder.Build();
.
.
.
```
**When declaring repositories, services, etc.**
**For Scoped Lifecycle**
```
public interface ITestRepository : IScopedInjector<ITestRepository, TestRepository>
```
**For Singleton Lifecycle**
```
public interface ITestRepository : ISingletonInjector<ITestRepository, TestRepository>
```
**For Transient Lifecycle**
```
public interface ITestRepository : ITransientInjector<ITestRepository, TestRepository>
```
