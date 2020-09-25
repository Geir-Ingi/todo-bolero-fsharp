module Tests

open Xunit
open FsCheck.Xunit
open Todo
open FsUnit.Xunit
open FSharpx

[<Property>]
let ``Tasks are created`` (tasks : string list) =
    let todoList =
        tasks
        |> List.fold createNewTask createNewTodoList
        
    tasks
    |> List.iter (fun task ->
        todoList.Tasks
        |> List.map (fun t -> t.Description)
        |> should contain task)

[<Fact>]
let ``Tasks can be removed from the todo list`` () =
    let todoList =
        [ "Task1"; "Task2"; "Task3"]
        |> List.fold createNewTask createNewTodoList

    let taskToRemove = todoList.Tasks.[1]
    
    let todoList = removeTask todoList taskToRemove.Id
    
    todoList.Tasks
    |> List.map (fun t -> t.Id)
    |> should not' (contain taskToRemove.Id)
    
[<Fact>]
let ``Completed tasks are removed when todo list is cleared`` () =
    let todoList =
        [ "Task1"; "Task2"; "Task3"; "Task4"; "Task5"]
        |> List.fold createNewTask createNewTodoList

    let tasksToComplete =
        [todoList.Tasks.[1].Id; todoList.Tasks.[2].Id]

    let todoList =
            setTaskCompleteness todoList true tasksToComplete.[0]
            |> flip3 setTaskCompleteness true tasksToComplete.[1]
            |> clearCompleted
        
    let taskIds =
        todoList.Tasks
        |> List.map (fun t -> t.Id)
    taskIds |> should not' (contain tasksToComplete.[0])
    taskIds |> should not' (contain tasksToComplete.[1])
    
[<Property>]
let ``All tasks in the list gets completed`` (tasks : string list) =
    let todoList =
        tasks
        |> List.fold createNewTask createNewTodoList
        |> flip setAllTasksCompleteness true
    
    todoList.Tasks
    |> List.iter (fun t ->
        t.IsCompleted |> should equal true)
    
[<Fact>]
let ``Can edit a task`` () =
    let todoList =
        createNewTask createNewTodoList "Some task"

    let todoList = editTask todoList todoList.Tasks.[0].Id "Still some task"
    
    todoList.Tasks.[0].Description
    |> should equal "Still some task"
    
        