module Web.TodoList

open Bolero
open Bolero.Html
open Bolero.Remoting
open Elmish
open FSharpx
open Todo

type TaskService =
    { GetTasks : unit -> string array Async }
    interface IRemoteService with
        member this.BasePath = "/tasks"
        
type ShowMode =
    | All
    | Active
    | Completed

type Model =
    { TodoList : TodoList
      Show : ShowMode
      NewTask : string
      Editing : Task option }
    
type Message =
    | EditNewTask of string
    | AddTask of string
    | RemoveTask of TaskId
    | GotTasks of string array
    | ClearCompleted
    | SetAllCompleted of bool
    | SetShowMode of ShowMode
    | StartEdit of TaskId
    | Edit of string
    | CommitEdit of TaskId
    | CancelEdit
    | SetCompleted of TaskId * bool
    | Error of exn

type ViewTemplate = Template<"wwwroot/main.html">

let update (_: TaskService) (msg: Message) (model: Model) =
    let update f = { model with TodoList = f }
    match msg with
    | AddTask description -> // TODO: Implement
        model
    | RemoveTask id -> // TODO: Implement
        model
    | GotTasks ts -> // TODO: Implement
        model
    | ClearCompleted -> // TODO: Implement
        model
    | SetAllCompleted c -> // TODO: Implement
        model
    | SetCompleted (id, isCompleted) -> // TODO: Implement
        model
    | EditNewTask description -> // TODO: Implement
        model
    | SetShowMode s -> // TODO: Implement
        model
    | StartEdit id -> // TODO: Implement
        model
    | Edit description -> // TODO: Implement
        model
    | CommitEdit id -> // TODO: Implement
        model
    | CancelEdit -> // TODO: Implement
        model
    | Error e -> failwith e.Message
    , Cmd.none

let renderTask showMode dispatch listModel task =
    ViewTemplate.Entry()
        .Label(text task.Description)
        .CssAttrs(
            attr.classes [
                if task.IsCompleted then yield "completed"

                if listModel.Editing.IsSome then
                    if listModel.Editing.Value.Id = task.Id then
                        yield "editing"

                match showMode, task.IsCompleted with
                | ShowMode.Completed, false | ShowMode.Active, true ->
                    yield "hidden"
                | _ -> ()
            ]
        )
        .EditingTask(
            listModel.Editing |> Option.map (fun t -> t.Description) |> Option.defaultValue "",
            fun text -> dispatch (Edit text)
        )
        .EditBlur(fun _ -> dispatch <| CommitEdit task.Id)
        .EditKeyup(fun e ->
            match e.Key with
            | "Enter" -> dispatch <| CommitEdit task.Id
            | "Escape" -> dispatch CancelEdit
            | _ -> ()
        )
        .IsCompleted(
            task.IsCompleted,
            fun x -> dispatch <| SetCompleted (task.Id, x)
        )
        .Remove(fun _ -> dispatch <| RemoveTask task.Id)
        .StartEdit(fun _ -> dispatch <| StartEdit task.Id)
        .Elt()
        
let renderList listModel dispatch =
    let countNotCompleted =
        listModel.TodoList.Tasks
        |> List.filter (fun e -> not e.IsCompleted)
        |> List.length
    ViewTemplate()
        .HiddenIfNoEntries(if List.isEmpty listModel.TodoList.Tasks then "hidden" else "")
        .Entries(listModel.TodoList.Tasks
                 |> flip forEach (renderTask listModel.Show dispatch listModel))
        .ClearCompleted(fun _ -> dispatch ClearCompleted)
        .IsCompleted(
            (countNotCompleted = 0),
            fun c -> dispatch (SetAllCompleted c)
        )
        .Task(
            listModel.NewTask,
            fun text -> dispatch (EditNewTask text)
        )
        .Edit(fun e ->
            if e.Key = "Enter" && listModel.NewTask <> "" then
                dispatch <| AddTask listModel.NewTask
        )
        .ItemsLeft(
            match countNotCompleted with
            | 1 -> "1 item left"
            | n -> string n + " items left"
        )
        .CssFilterAll(attr.``class`` (if listModel.Show = ShowMode.All then "selected" else null))
        .CssFilterActive(attr.``class`` (if listModel.Show = ShowMode.Active then "selected" else null))
        .CssFilterCompleted(attr.``class`` (if listModel.Show = ShowMode.Completed then "selected" else null))
        .Elt()