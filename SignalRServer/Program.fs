
namespace SignalRServer

open Microsoft.AspNet.SignalR
open Microsoft.AspNet.SignalR.Hubs

open System
open System.Runtime
open System.Reactive.Concurrency
open System.Reactive.Linq
open System.Web
open System.Web.Routing

open Dynamic
open TaskHelper

module MyServer =

    //SignalR supports two kinds of connections: Hub and PersistentConnection

    type MyConnection() as this = 
        inherit Microsoft.AspNet.SignalR.PersistentConnection()
        override x.OnConnected(req,id) =
            this.Connection.Send(id, "Welcome!") |> ignore
            base.OnConnected(req,id)

    [<HubName("myhub")>]
    type MyHub() = 
        inherit Microsoft.AspNet.SignalR.Hub()
        let caller = GlobalHost.ConnectionManager.GetHubContext<MyHub>().Clients?Caller
        override x.OnConnected() =
            base.OnConnected()
        member x.MyCustomFunction() =
            let i = caller?state
            "Hello"

    let sendAll msg = 
            GlobalHost.ConnectionManager.GetConnectionContext<MyConnection>().Connection.Broadcast(msg) 
                |> Async.awaitPlainTask |> ignore
            GlobalHost.ConnectionManager.GetHubContext<MyHub>().Clients.All?broadcastMessage(msg)
                |> Async.awaitPlainTask |> ignore
            

    //Send some responses from server to client...
    let SignalRCommunication() =

        //Send first message
        sendAll("Hello world!")

        //Send ping on every 5 seconds
        let pings = Observable.Interval(TimeSpan.FromSeconds(5.0), Scheduler.Default)
        pings.Subscribe(fun s -> sendAll("ping!")) |> ignore

    // Hosting is in WebApp project, this is called from Global.asax.cs
    let SetupRouting() =
        RouteTable.Routes.MapConnection<MyConnection>("signalrConn", "signalrConn") |> ignore
        
        let hubc = new HubConfiguration()
        hubc.EnableDetailedErrors <- true
        hubc.EnableCrossDomain <- true
        RouteTable.Routes.MapHubs("/signalrHub", hubc) |> ignore

        SignalRCommunication()

    // We could host this also as command-line application with Owin server...
    // Then you would need to supply clientaccesspolicy.xml and crossdomain.xml to Silverlight client
    //    open Microsoft.Owin.Hosting
    //
    //    [<EntryPoint>]
    //    let main argv = 
    //        //Note that server and client has to use the same port
    //        let url = "http://localhost:8080"
    //        // Here you would need new empty C#-class just for configuration: ServerStartup.MyWebStartup:
    //        use app =  WebApplication.Start<ServerStartup.MyWebStartup>(url) 
    //        SignalRCommunication()
    //        Console.WriteLine "Server running..."
    //        Console.ReadLine() |> ignore
    //        app.Dispose()
    //        0
