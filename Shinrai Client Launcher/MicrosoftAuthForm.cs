using Microsoft.Web.WebView2.Core;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Shinrai_Client_Launcher
{
    public partial class MicrosoftAuthForm : Form
    {
        private readonly TaskCompletionSource<string> _tcsCode = new();

        private readonly string redirectUri;
        private readonly string authUrl;

        public MicrosoftAuthForm(string authUrl, string redirectUri)
        {
            InitializeComponent();

            this.authUrl = authUrl ?? throw new ArgumentNullException(nameof(authUrl));
            this.redirectUri = redirectUri ?? throw new ArgumentNullException(nameof(redirectUri));

            webView21.NavigationStarting += WebView21_NavigationStarting;

            this.Load += async (_, __) =>
            {
                await webView21.EnsureCoreWebView2Async();
                webView21.CoreWebView2.Navigate(authUrl);
            };
            txt();
        }

        private void WebView21_NavigationStarting(object sender, CoreWebView2NavigationStartingEventArgs e)
        {
            try
            {
                var uri = new Uri(e.Uri);

                if (uri.AbsoluteUri.StartsWith(redirectUri, StringComparison.OrdinalIgnoreCase))
                {
                    var query = System.Web.HttpUtility.ParseQueryString(uri.Query);
                    var code = query.Get("code");

                    if (!string.IsNullOrEmpty(code))
                    {
                        e.Cancel = true;

                        // TaskCompletionSource'u tamamla
                        if (!_tcsCode.Task.IsCompleted)
                        {
                            _tcsCode.TrySetResult(code);
                        }

                        // Formu kapat (Invoke ile UI thread)
                        if (this.InvokeRequired)
                        {
                            this.Invoke((Action)(() => this.Close()));
                        }
                        else
                        {
                            this.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Hata durumunda istersen logla veya yönet
                Debug.WriteLine("WebView NavigationStarting hatası: " + ex.Message);
            }
        }

        public Task<string> WaitForCodeAsync()
        {
            return _tcsCode.Task;
        }

        public void txt()
        {
            Translateable translateable = new Translateable();
            translateable.LoadJson(Properties.Settings.Default.Language);
            label1.Text = translateable.TranslatableText("launcher.microsoftauth.title");// Boşsa silebilirsin
        }
        // Eğer bu method gereksizse silebilirsin
        private void guna2Panel1_Paint(object sender, PaintEventArgs e)
        {
            
        }
    }
}
