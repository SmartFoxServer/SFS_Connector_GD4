using Godot;

using Sfs2X;
using Sfs2X.Core;
using Sfs2X.Logging;
using Sfs2X.Requests;
using Sfs2X.Util;

using System.ComponentModel;
using System.Diagnostics;

/**
 * Script attached to the Controller object in the scene.
 */
public partial class connector : Control
{
    //----------------------------------------------------------
    // Editor public properties
    //----------------------------------------------------------

    [ExportCategory("SFS2X Connection Settings")]

    [Export, Description("IP address or domain name of the SmartFoxServer instance; if encryption is enabled, a domain name must be entered")]
    public string host = "127.0.0.1";
    [Export, Description("TCP listening port of the SmartFoxServer instance, used for TCP socket connection in all builds except HTML5")]
    public int tcpPort = 9933;
    [Export, Description("HTTP listening port of the SmartFoxServer instance, used for WebSocket (WS) connections in HTML5 build")]
    public int httpPort = 8080;
    [Export, Description("HTTPS listening port of the SmartFoxServer instance, used for WebSocket Secure (WSS) connections in HTML5 build and connection encryption in all other builds")]
    public int httpsPort = 8443;
    [Export, Description("Use SmartFoxServer's HTTP tunneling (BlueBox) if TCP socket connection can't be established; not available in HTML5 builds")]
    public bool useHttpTunnel = false;
    [Export, Description("Enable SmartFoxServer protocol encryption; 'host' must be a domain name and an SSL certificate must have been deployed")]
    public bool encrypt = false;
    [Export, Description("Name of the SmartFoxServer Zone to join")]
    public string zone = "BasicExamples";
    [Export, Description("Display SmartFoxServer client debug messages")]
    public bool debug = false;
    [Export, Description("Client-side SmartFoxServer logging level")]
    public LogLevel logLevel = LogLevel.INFO;

    //----------------------------------------------------------
    // UI elements
    //----------------------------------------------------------

    [ExportCategory("UI Settings")]

    [Export]
    public Control loginPanel;
    [Export]
    public LineEdit errorText;
    [Export]
    public LineEdit nameInput;
    [Export]
    public Control mainPanel;

    private SmartFox sfs;

    //----------------------------------------------------------
    // Callback Methods
    //----------------------------------------------------------
    #region
    public override void _Process(double delta)
    {
        // Process the SmartFox events queue
        if (sfs != null)
            sfs.ProcessEvents();
    }

    public override void _Notification(int what)
    {
        if (what == NotificationWMCloseRequest)
        {
            if (sfs != null && sfs.IsConnected)
                sfs.Disconnect();

            GD.Print("Application Quit");
        }
    }
    #endregion

    //----------------------------------------------------------
    // UI event listeners
    //----------------------------------------------------------
    #region
    /**
	 * On Login button click, connect to SmartFoxServer.
	 */
    public void OnLoginButtonClick()
    {
        Connect();
    }

    /**
	 * On Logout button click, disconnect from SmartFoxServer.
	 */
    public void OnLogoutButtonClick()
    {
        sfs.Disconnect();
        GD.Print("Logout successful");

        // Update view
        mainPanel.Visible = false;
        loginPanel.Visible = true;

    }
    #endregion

    //----------------------------------------------------------
    // Helper methods
    //----------------------------------------------------------
    #region
    private void Connect()
    {
        GD.Print("Attempting connection to SFS2X...");

        // Clear any previour error message
        errorText.Text = "";

        // Initialize SmartFox client

        sfs = new SmartFox();


        // Add event listeners
        sfs.AddEventListener(SFSEvent.CONNECTION, OnConnection);
        sfs.AddEventListener(SFSEvent.CONNECTION_LOST, OnConnectionLost);
        sfs.AddEventListener(SFSEvent.CRYPTO_INIT, OnCryptoInit);
        sfs.AddEventListener(SFSEvent.LOGIN, OnLogin);
        sfs.AddEventListener(SFSEvent.LOGIN_ERROR, OnLoginError);

        // Configure internal SFS2X logger
        sfs.Logger.EnableConsoleTrace = true;
        sfs.Logger.LoggingLevel = logLevel;

        // Set connection parameters
        ConfigData cfg = new ConfigData();
        cfg.Host = host;
        cfg.Port = tcpPort;
        cfg.Zone = zone;
        cfg.HttpPort = httpPort;
        cfg.HttpsPort = httpsPort;
        cfg.BlueBox.IsActive = useHttpTunnel;
        cfg.BlueBox.UseHttps = encrypt;
        cfg.Debug = debug;


        // Connect to SmartFoxServer
        sfs.Connect(cfg);
    }

    private void Login()
    {
        GD.Print("Performing login...");

        // Login
        sfs.Send(new LoginRequest(nameInput.Text));
    }
    #endregion

    //----------------------------------------------------------
    // SmartFoxServer event listeners
    //----------------------------------------------------------
    #region
    private void OnConnection(BaseEvent evt)
    {
        // Check if the conenction was established or not
        if ((bool)evt.Params["success"])
        {
            GD.Print("Connection established successfully");
            GD.Print("SFS2X API version: " + sfs.Version);
            GD.Print("Connection mode is: " + sfs.ConnectionMode);



            if (encrypt)
            {
                GD.Print("Initializing encryption...");

                // Initialize encryption
                sfs.InitCrypto();
            }
            else
            {
                // Attempt login
                Login();
            }
        }
        else
        {
            // Show error message
            errorText.Text = "Connection failed; is the server running at all?";

        }
    }

    private void OnConnectionLost(BaseEvent evt)
    {
        // Remove SFS listeners
        sfs.RemoveEventListener(SFSEvent.CONNECTION, OnConnection);
        sfs.RemoveEventListener(SFSEvent.CONNECTION_LOST, OnConnectionLost);
        sfs.RemoveEventListener(SFSEvent.CRYPTO_INIT, OnCryptoInit);
        sfs.RemoveEventListener(SFSEvent.LOGIN, OnLogin);
        sfs.RemoveEventListener(SFSEvent.LOGIN_ERROR, OnLoginError);

        sfs = null;

        // Show error message
        string reason = (string)evt.Params["reason"];
        //
        GD.Print("Connection to SmartFoxServer lost; reason is: " + reason);

        if (reason != ClientDisconnectionReason.MANUAL)
        {
            // Show error message
            string connLostMsg = "An unexpected disconnection occurred; ";

            if (reason == ClientDisconnectionReason.IDLE)
                connLostMsg += "you have been idle for too much time";
            else if (reason == ClientDisconnectionReason.KICK)
                connLostMsg += "you have been kicked";
            else if (reason == ClientDisconnectionReason.BAN)
                connLostMsg += "you have been banned";
            else
                connLostMsg += "reason is unknown.";

            errorText.Text = connLostMsg;
        }


    }

    private void OnCryptoInit(BaseEvent evt)
    {
        if ((bool)evt.Params["success"])
        {
            GD.Print("Encryption initialized successfully");

            // Attempt login
            Login();
        }
        else
        {
            GD.Print("Encryption initialization failed: " + (string)evt.Params["errorMessage"]);

            // Disconnect
            // NOTE: this causes a CONNECTION_LOST event with reason "manual", which in turn removes all SFS listeners
            sfs.Disconnect();

            // Show error message
            errorText.Text = "Encryption initialization failed";

        }
    }

    private void OnLogin(BaseEvent evt)
    {
        GD.Print("Login successful");

        // Update view
        loginPanel.Visible = false;
        mainPanel.Visible = true;
    }


    private void OnLoginError(BaseEvent evt)
    {
        GD.Print("Login failed");

        // Disconnect
        // NOTE: this causes a CONNECTION_LOST event with reason "manual", which in turn removes all SFS listeners
        sfs.Disconnect();

        // Show error message
        errorText.Text = "Login failed due to the following error:\n" + (string)evt.Params["errorMessage"];
    }

    #endregion
}