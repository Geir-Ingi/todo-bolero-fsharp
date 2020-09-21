namespace Server

open Microsoft.AspNetCore.Hosting
open Microsoft.AspNetCore.Builder
open Microsoft.Extensions.DependencyInjection
open Bolero.Remoting.Server
open Bolero.Templating.Server
open Bolero.Server.RazorHost

type Startup() =
    member this.ConfigureServices(services: IServiceCollection) =
        services.AddMvc().AddRazorRuntimeCompilation() |> ignore
        services.AddServerSideBlazor() |> ignore
        services
            .AddRemoting<TaskServiceHandler>()
            .AddBoleroHost()
#if DEBUG
            .AddHotReload(templateDir = __SOURCE_DIRECTORY__ + "/../Web")
#endif
        |> ignore

    member this.Configure(app: IApplicationBuilder, _: IWebHostEnvironment) =
        app
            .UseAuthentication()
            .UseRemoting()
            .UseStaticFiles()
            .UseRouting()
            .UseBlazorFrameworkFiles()
            .UseEndpoints(fun endpoints ->
#if DEBUG
                endpoints.UseHotReload()
#endif
                endpoints.MapBlazorHub() |> ignore
                endpoints.MapFallbackToPage("/_Host") |> ignore)
        |> ignore