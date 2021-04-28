using System.Text.RegularExpressions;
using Godot;

public class EscapeMenuMultiplayer : Container
{
    private const ushort DEFAULT_PORT = 42005;

    [Export] public NodePath StatusPath { get; set; }
    [Export] public NodePath ServerOpenClosePath { get; set; }
    [Export] public NodePath ServerPortPath { get; set; }
    [Export] public NodePath ClientDisConnectPath { get; set; }
    [Export] public NodePath ClientAddressPath { get; set; }

    public Label Status { get; private set; }
    public Button ServerOpenClose { get; private set; }
    public LineEdit ServerPort { get; private set; }
    public Button ClientDisConnect { get; private set; }
    public LineEdit ClientAddress { get; private set; }

    public IntegratedServer Server { get; private set; }

    public override void _Ready()
    {
        Status           = GetNode<Label>(StatusPath);
        ServerOpenClose  = GetNode<Button>(ServerOpenClosePath);
        ServerPort       = GetNode<LineEdit>(ServerPortPath);
        ClientDisConnect = GetNode<Button>(ClientDisConnectPath);
        ClientAddress    = GetNode<LineEdit>(ClientAddressPath);

        ServerPort.PlaceholderText    = DEFAULT_PORT.ToString();
        ClientAddress.PlaceholderText = $"localhost:{DEFAULT_PORT}";

        CallDeferred(nameof(SetupIntegratedServer));
    }
    private void SetupIntegratedServer()
    {
        Server = new IntegratedServer();
        this.GetClient().AddChild(Server);
        CallDeferred(nameof(StartIntegratedServerAndConnect));
    }
    private void StartIntegratedServerAndConnect()
    {
        Server.Start(DEFAULT_PORT);
        this.GetClient().Connect("localhost", DEFAULT_PORT);
    }


    // private void OnNetworkStatusChanged(NetworkStatus status)
    // {
    //     switch (status) {
    //         case NetworkStatus.NoConnection:
    //             Status.Text     = "No Connection";
    //             Status.Modulate = Colors.Red;
    //             break;
    //         case NetworkStatus.ServerRunning:
    //             Status.Text     = "Server Running";
    //             Status.Modulate = Colors.Green;
    //             break;
    //         case NetworkStatus.Connecting:
    //             Status.Text     = "Connecting ...";
    //             Status.Modulate = Colors.Yellow;
    //             break;
    //         case NetworkStatus.Authenticating:
    //             Status.Text     = "Authenticating ...";
    //             Status.Modulate = Colors.YellowGreen;
    //             break;
    //         case NetworkStatus.ConnectedToServer:
    //             Status.Text     = "Connected to Server";
    //             Status.Modulate = Colors.Green;
    //             break;
    //     }

    //     var noConnection = status == NetworkStatus.NoConnection;
    //     ServerPort.Editable = noConnection;
    //     ServerOpenClose.Text     = (status == NetworkStatus.ServerRunning) ? "Stop Server" : "Start Server";
    //     ServerOpenClose.Disabled = status > NetworkStatus.ServerRunning;
    //     ClientAddress.Editable = noConnection;
    //     ClientDisConnect.Text     = (status < NetworkStatus.Connecting) ? "Connect" : "Disconnect";
    //     ClientDisConnect.Disabled = status == NetworkStatus.ServerRunning;
    // }


    #pragma warning disable IDE0051
    #pragma warning disable IDE1006

    private static readonly Regex INVALID_CHARS = new Regex(@"[^0-9]");
    private void _on_ServerPort_text_changed(string text)
    {
        var validText = INVALID_CHARS.Replace(text, "");
        validText = validText.TrimStart('0');
        if (validText != text) {
            var previousCaretPos = ServerPort.CaretPosition;
            ServerPort.Text = validText;
            ServerPort.CaretPosition = previousCaretPos - (text.Length - validText.Length);
        }
    }


    private void _on_ServerOpenClose_pressed()
    {
        // if (GetTree().NetworkPeer == null) {
        //     var port = Network.DEFAULT_PORT;
        //     if (ServerPort.Text.Length > 0)
        //         port = ushort.Parse(ServerPort.Text);
        //     Network.Instance.StartServer(port);
        // } else Network.Instance.StopServer();
    }

    private void _on_ClientDisConnect_pressed()
    {
        // if (GetTree().NetworkPeer == null) {
        //     var address = "localhost";
        //     var port    = DEFAULT_PORT;
        //     if (ClientAddress.Text.Length > 0) {
        //         // TODO: Verify input some more, support IPv6?
        //         var split = ClientAddress.Text.Split(':');
        //         address = (split.Length > 1) ? split[0] : ClientAddress.Text;
        //         port    = (split.Length > 1) ? ushort.Parse(split[1]) : port;
        //     }
        //     Network.Instance.ConnectToServer(address, port);
        // } else Network.Instance.DisconnectFromServer();
    }

    private void _on_HideAddress_toggled(bool pressed)
        => ClientAddress.Secret = pressed;
}
