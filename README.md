# SmartFoxServer Example Projects for Godot 4.x

This series of **C# examples** built for the Godot 4 engine have been developed with **Godot Mono 4.0.3**, but the concepts and the code to interact with the SFS2X API are valid for any version of Godot 4.x (unless otherwise noted).

Each of the tutorials in this series examine a single example, describing its objectives, **offering an insight into the SmartFoxServer features** it wants to highlight. This project includes all the assets required to compile and test the example (both client and — if existing — server side). If necessary, code excerpts are provided in the tutorial itself (see online documentation link below), in order to better explain the approach that was followed to implement a specific feature. At the bottom of the tutorial, additional resources are linked if available.

The tutorials have an increasing complexity, from basic server connection to a complete game with authoritative server code.

Specifically, the examples will showcase:

* **basic connection with optional protocol encryption**
* room management
* buddy list management
* game rooms and match-making
* authoritative server in a turn-based game

The Godot examples provided have been tested for exporting as native executables for Windows and macOS. At the time of writing this article (June 2023) Godot Mono does not yet support exporting for mobile platform or the browser.

# SFS_Connector_GD4
The **Connector** example shows how to setup and use the **SmartFox** client API object, establish a **connection** to the server and **login**. It also shows how to deal with the different requirements of the Godot build targets, making use of a few conditional compilation statements.

The example also features the logic needed to activate the **protocol cryptography** that was introduced in SmartFoxServer 2X starting from version 2.10. This will be discussed at the bottom of this tutorial.

The Godot project consists of a single scene with two control nodes representing the initial login view and a generic main view acting as a placeholder for what comes after a successful login. On the login view the user can enter their name and hit the button to connect to SmartFoxServer and execute the login process.
 <p align="center"> 
<img width="720" alt="connector-login" src="https://github.com/SmartFoxServer/SFS_Connector_GD4/assets/30838007/0def40f7-5d89-47b2-87b9-e5c4ae447bf9">
 </p>
The example features a single script component. A number of properties exposed in the Editor's Inspector panel allow configuring the connection parameters and API logging behavior.

## Setup & run
In order to setup and run the example, follow these steps:

1. unzip the examples package;
2. launch the Godot, click on the Import button and navigate to the Connector folder;
3. click the **Build button** in the top right corner of the Godot editor before running the example;

The client's C# code is in the Godot project's *res://scripts* folder, while the SmartFoxServer 2X client API DLLs are in the *res:// folder*.

## Online Tutorial and Documentation
The code for this example is contained in the script file called Connector in the /scripts folder, attached to the node called Connector in the scene.
The file is a **basic C# script** implementing the _Process method. Additionally, there's a few listeners for the events fired by UI components (buttons, input), some helper methods and the listeners for SmartFoxServer's client API events.

To learn more about this template and how it is configured for establishing a connection and handling basic SmartFoxServer events, go to the online documentation and tutorials linked below.

**SmartFoxServer Example Documentation**   

http://docs2x.smartfoxserver.com/ExamplesGodot/connector

This online documentation includes:
* Field Declaration
* Establishing a connection
* Handling basic events
  - Connection event handler
  - Login event handler
  - Login Error event handler
  - Connection Lost event handler
* Notification Callback
* HTTP Tunneling
* Protocol Encryption

  
 and further **Resource Links**

http://docs2x.smartfoxserver.com/ExamplesGodot/introduction

