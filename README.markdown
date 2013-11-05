**Example of SignalR with F-Sharp (and Silverlight 5 or Javascript):**

- SignalR is a library (for web-based Publish/Subscribe -pattern) on top 
  of WebSockets, which is HTML5 API that enables bi-directional communication 
  between the browser and server. SignalR will fallback to other techniques and 
  technologies when WebSockets are not available.
  [http://signalr.net/][1]
  
- F-Sharp (F#) is multiparadigm (functional-first) programming language 
  mainly for .NET environment.
  [http://fsharp.org/][2]
  
- This sample uses Reactive Extensions 2.1 to communicate with SignalR, 
  on both client and server side. Reactive Extensions is a library to 
  compose asynchronous and event-based programs using observable 
  collections and LINQ-style query operators.
  [http://msdn.microsoft.com/en-us/data/gg577609.aspx][3]

- For server side:  
  This could work purely from F# as Owin ([http://owin.org/][4]) console application 
  but now F#-server-side is called from an empty ASP.NET C# Web Application. 
  (You can start OWin from F# Interactive...)

- For client side:
  Silverlight 5.0 application using F#-library
  and Silverlight 5.0 XAML/C# application to show the user interface.
  [http://www.silverlight.net][5]
  There is also jQuery/JavaScript test page, if you don't want to use Silverlight.
  
- This proof of concept/sample/tutorial/demo is developed with 
  Visual Studio 2012. References are resolved via NuGet. 
  F# compiler won't auto-restore packages on compile. 
  **So go first to Tools -> Library Package Manager -> NuGet
  and press "Restore" to restore all the references, and then rebuild.**

- SignalR supports two kinds of connections: Hub and PersistentConnection
  Both are working with Silverlight and JavaScript. 
  


  [1]: http://signalr.net/
  [2]: http://fsharp.org/
  [3]: http://msdn.microsoft.com/en-us/data/gg577609.aspx
  [4]: http://owin.org/
  [5]: http://www.silverlight.net
