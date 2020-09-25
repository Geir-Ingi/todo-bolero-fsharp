module Todo

open System

type TaskId = TaskId of Guid

type Task =
    { Id : TaskId
      Description : string
      IsCompleted : bool }
        
type TodoList =
    { Tasks : Task list }

let createNewTodoList = { Tasks = [] }

let createNewTask model description = 
    { model with
        Tasks = model.Tasks @ [ { Id = Guid.NewGuid() |> TaskId
                                  Description = description
                                  IsCompleted = false } ] }

let editTask model id description =
    { model with Tasks =
                    model.Tasks
                    |> List.map (fun t ->
                                 if t.Id = id
                                 then { t with Description = description }
                                 else t) }

let removeTask model id =
    { model with Tasks =
                    model.Tasks
                    |> List.filter (fun t -> t.Id <> id) }
        
let setTaskCompleteness model isComplete id =
    { model with Tasks =
                    model.Tasks
                    |> List.map (fun t ->
                        if t.Id = id
                        then { t with IsCompleted = isComplete }
                        else t) }
    
let setAllTasksCompleteness model isComplete =
    model.Tasks
    |> List.fold (fun l t -> setTaskCompleteness l isComplete t.Id) model
    
let clearCompleted model =
    { model with Tasks =
                    model.Tasks
                    |> List.filter (fun e -> not e.IsCompleted) }

