using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using TodoList.Contracts;
using System.Collections.ObjectModel;
using System.Linq;
using ReactiveUI;
using System.Threading.Tasks;
using System.Reactive.Threading.Tasks;
using System.Reactive.Linq;

namespace TodoList.Desktop.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private ObservableCollection<WeatherForecast> _weatherForecasts = new();

        private SummaryType? _selectedSummary;

        public MainWindowViewModel()
        {
            var observable = GetWeatherForecast()
                .ToObservable()
                .ObserveOn(RxApp.MainThreadScheduler);
            observable
                .Subscribe(x => WeatherForecasts = new(x));
        }

        public ObservableCollection<WeatherForecast> WeatherForecasts
        {
            get => _weatherForecasts;
            set => this.RaiseAndSetIfChanged(ref _weatherForecasts, value);
        }

        public SummaryType[] Summaries { get; } = Enum.GetValues<SummaryType>();

        public SummaryType? SelectedSummary
        {
            get => _selectedSummary;
            set => this.RaiseAndSetIfChanged(ref _selectedSummary, value);
        }

        private async Task<List<WeatherForecast>> GetWeatherForecast()
        {
            using var client = new HttpClient();
            var response = await client.GetAsync("http://localhost:5115/WeatherForecast");
            var items = await response.Content.ReadFromJsonAsync<List<WeatherForecast>>();
            return items!;
        }

        public void OnRefresh()
        {
            SelectedSummary = null;
            var observable = GetWeatherForecast()
                .ToObservable()
                .ObserveOn(RxApp.MainThreadScheduler);
            observable
                .Subscribe(x => WeatherForecasts = new(x));
        }

        public void OnFilterBySummary()
        {
            if (SelectedSummary.HasValue)
            {
                var observable = GetWeatherForecast()
                    .ToObservable()
                    .ObserveOn(RxApp.MainThreadScheduler);
                observable
                    .Subscribe(x => WeatherForecasts = new(x.Where(forecast => forecast.Summary == SelectedSummary)));
            }
        }
    }
}