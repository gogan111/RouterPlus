using System.Net.Http;
using System.Text;
using System.Windows.Input;
using System.Threading.Tasks;
using RouterPlus.Core;
using RouterPlus.Helpers;

namespace RouterPlus.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private string _url;
        private string _password;
        private string _errorMessage;
        private bool _isErrorVisible;
        private string _normalizedUrl;

        public string Url
        {
            get => _url;
            set
            {
                _url = value;
                _isErrorVisible = false;
                OnPropertyChanged();
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                _isErrorVisible = false;
                OnPropertyChanged();
            }
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                _errorMessage = value;
                OnPropertyChanged();
            }
        }

        public bool IsErrorVisible
        {
            get => _isErrorVisible;
            set
            {
                _isErrorVisible = value;
                OnPropertyChanged();
            }
        }

        public ICommand LoginCommand { get; }

        public LoginViewModel()
        {
            LoginCommand = new RelayCommand(Login);
        }

        private async void Login(object parameter)
        {
            bool isUrlValid = ValidationHelper.ValidateField(Url, "URL", ref _errorMessage, ref _isErrorVisible);
            bool isPasswordValid = ValidationHelper.ValidateField(Password, "Password", ref _errorMessage, ref _isErrorVisible);

            OnPropertyChanged(nameof(ErrorMessage));
            OnPropertyChanged(nameof(IsErrorVisible));
            
            if (isUrlValid && isPasswordValid)
            {
                NormalizeUrl();
                bool isAuthenticated = await AuthenticateAsync(_normalizedUrl, Password);
                if (isAuthenticated)
                {
                    ErrorMessage = string.Empty;
                    IsErrorVisible = false;
                }
                else
                {
                    ErrorMessage = "Invalid credentials.";
                    IsErrorVisible = true;
                }
            }
        }

        private void NormalizeUrl()
        {
            if (!Url.StartsWith("http://", StringComparison.OrdinalIgnoreCase))
            {
                _normalizedUrl = $"http://{Url}";
            }
            else
            {
                _normalizedUrl = Url;
            }
        }

        private async Task<bool> AuthenticateAsync(string url, string password)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string encodedPassword = Convert.ToBase64String(Encoding.UTF8.GetBytes(password));
                    var formData = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("isTest", "false"),
                        new KeyValuePair<string, string>("goformId", "LOGIN"),
                        new KeyValuePair<string, string>("password", encodedPassword)
                    });

                    string endpoint = $"{url}/goform/goform_set_cmd_process";
                    var requestMessage = new HttpRequestMessage(HttpMethod.Post, endpoint) { Content = formData };
                    requestMessage.Headers.Add("Referer", $"{url}/index.html");

                    var response = await client.SendAsync(requestMessage);
                    return response.IsSuccessStatusCode && (await response.Content.ReadAsStringAsync()).Contains("\"result\":\"0\"");
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = "Error connecting to router.";
                IsErrorVisible = true;
                return false;
            }
        }
    }
}
