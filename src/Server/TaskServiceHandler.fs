namespace Server

open Bolero.Remoting.Server
open Microsoft.AspNetCore.Hosting
open Web.TodoList
       
type TaskServiceHandler(_ctx: IRemoteContext, _env: IWebHostEnvironment) =
    inherit RemoteHandler<TaskService>()

    override this.Handler =
        { GetTasks = fun() -> async {
            return [|
                "Do this"
                "Do that"
                "Finish with this"
            |]
        } }
