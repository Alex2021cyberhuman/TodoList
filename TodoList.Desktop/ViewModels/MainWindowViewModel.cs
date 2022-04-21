using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using System.Net.Http.Json;
using TodoList.Contracts;
using System.Collections.ObjectModel;
using DynamicData;
using ReactiveUI;
using System.Threading.Tasks;
using System.Reactive.Threading.Tasks;
using System.Reactive.Linq;
using Avalonia.Controls;

namespace TodoList.Desktop.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private ObservableCollection<WeatherForecast> _weatherForecasts = new();

        public MainWindowViewModel()
        {
            var observable = GetWeatherForecast()
                .ToObservable()
                .ObserveOn(RxApp.MainThreadScheduler);
            observable
                .Subscribe(x => WeatherForecasts = new(x));
            observable
                .Subscribe(x => Console.WriteLine(string.Join("\n", x)));
        }

        public string Greeting => "Welcome to Avalonia!";

        public ObservableCollection<WeatherForecast> WeatherForecasts 
        {
            get => _weatherForecasts; 
            set =>  this.RaiseAndSetIfChanged(ref _weatherForecasts, value); 
        }        

        private async Task<List<WeatherForecast>> GetWeatherForecast()
        {
            using var client = new HttpClient();
            var response = await client.GetAsync("http://localhost:5115/WeatherForecast");
            var items = await response.Content.ReadFromJsonAsync<List<WeatherForecast>>();
            return items!;
        }

        public void OnClickCommand(object? obj = null)
        {
            Console.WriteLine(obj?.GetType().ToString());
            var observable = GetWeatherForecast()
                .ToObservable()
                .ObserveOn(RxApp.MainThreadScheduler);
            observable
                .Subscribe(x => WeatherForecasts = new(x));

            observable
                .Subscribe(x => Console.WriteLine(string.Join("\n", x)));
        }
    }
}
