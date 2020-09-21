module Todo

open System

type TaskId = TaskId of Guid

type Task =
    { Id : TaskId
      Description : string
      IsCompleted : bool }
        
type TodoList =
    { Tasks : Task list }

// TODO: Write a `todo` application