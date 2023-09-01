# SFS_Connector_GD4
The Connector example shows how to setup and use the SmartFox client API object, establish a connection to the server and login. It also shows how to deal with the different requirements of the Godot build targets, making use of a few conditional compilation statements.

The example also features the logic needed to activate the protocol cryptography that was introduced in SmartFoxServer 2X starting from version 2.10. This will be discussed at the bottom of this tutorial.

The Godot project consists of a single scene with two control nodes representing the initial login view and a generic main view acting as a placeholder for what comes after a successful login. On the login view the user can enter their name and hit the button to connect to SmartFoxServer and execute the login process.

The example features a single script component. A number of properties exposed in the Editor's Inspector panel allow configuring the connection parameters and API logging behavior.

» Setup & run

In order to setup and run the example, follow these steps:

unzip the examples package;
launch the Godot, click on the Import button and navigate to the Connector folder;
click the Build button in the top right corner of the Godot editor before running the example;
