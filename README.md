# Bolero TodoMVC

This is an implementation of the [TodoMVC](http://todomvc.com/) sample application using [Bolero](https://github.com/fsbolero/Bolero).

[See it running live here.](https://fsbolero.github.io/TodoMVC/)

# Getting started
```
dotnet tool restore
dotnet paket install
dotnet run -p src\Server
```

# TODO
Your mission is to implement the todo application:
* Write unit tests in the `Todo.Tests` project
* Implement the application in the `Todo` project
* Implement the frontend in the `Web` project
* Implement a persisting backend in the `Server` project
