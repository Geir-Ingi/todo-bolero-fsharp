namespace Web

open Microsoft.AspNetCore.Components.WebAssembly.Hosting
open Bolero
open Bolero.Remoting
open Bolero.Remoting.Client
open Bolero.Templating.Client
open Elmish
open Todo
open TodoList

module Program =
    type EndPoint =
        | [<EndPoint "/">]
          All
        | [<EndPoint "/active">]
          Active
        | [<EndPoint "/completed">]
          Completed
    
    let initialModel =
        { TodoList = { Tasks = [] }
          Show = ShowMode.All
          NewTask = ""
          Editing = None }
    
    type Component() =
        inherit ProgramComponent<Model, Message>()

        override this.Program =
            let taskService = this.Remote<TaskService>()
            Program.mkProgram (fun (_) -> initialModel, (Cmd.OfAsync.either taskService.GetTasks () GotTasks Error))
                             (update taskService)
                             renderList
            |> Program.withRouter
                    (Router.infer
                        (fun ep -> match ep with
                                   | EndPoint.All -> SetShowMode ShowMode.All
                                   | EndPoint.Active -> SetShowMode ShowMode.Active
                                   | EndPoint.Completed -> SetShowMode ShowMode.Completed)
                        (fun model -> match model.Show with
                                      | ShowMode.All -> EndPoint.All
                                      | ShowMode.Active -> EndPoint.Active
                                      | ShowMode.Completed -> EndPoint.Completed))
            #if DEBUG
            |> Program.withConsoleTrace
            |> Program.withHotReload
            #endif

    [<EntryPoint>]
    let main args =
        let builder = WebAssemblyHostBuilder.CreateDefault(args)
        builder.RootComponents.Add<Component>(".todoapp")
        builder.Services.AddRemoting(builder.HostEnvironment) |> ignore
        builder.Build().RunAsync() |> ignore
        0
