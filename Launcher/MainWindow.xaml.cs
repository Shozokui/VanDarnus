using System;
using System.Windows;
using CefSharp;
using CefSharp.Handler;

namespace Launcher
{

    public class XivRequestHandler : RequestHandler
    {
        protected override void OnDocumentAvailableInMainFrame(IWebBrowser chromiumWebBrowser, IBrowser browser)
        {
            base.OnDocumentAvailableInMainFrame(chromiumWebBrowser, browser);
        }
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static string GAME_LOCATION = @"D:\Games\SquareEnix\FINAL FANTASY XIV";
        string SessionId { get; set; }

        string EncryptionToken => string.Format("{0:X8}", Environment.TickCount & ~0xFFFF);
        string CommandFormat => string.Format(" T ={0} /LANG =en-us /REGION =2 /SERVER_UTC =1356916742 /SESSION_ID ={1}", Environment.TickCount, SessionId);


        public MainWindow()
        {
            InitializeComponent();

            

        }

    }
}
