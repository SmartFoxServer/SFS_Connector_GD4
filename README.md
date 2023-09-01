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
* Handling bacis events
  - Connection event handler
  - Login event handler
  - Login Error event handler
  - Connection Lost event handler
* Notification Callback
* HTTP Tunneling
* Protocol Encryption

  
 and further **Resource Links**

