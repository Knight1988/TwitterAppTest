using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using LiveCharts;
using LiveCharts.Wpf;

namespace TwitterApp.ViewModels;

public class MainViewModel : INotifyPropertyChanged
{
    private int _tweetCount;
    private double _averageTweetPerMinute;

    public int TweetCount
    {
        get => _tweetCount;
        set => SetField(ref _tweetCount, value);
    }

    public double AverageTweetPerMinute
    {
        get => _averageTweetPerMinute;
        set => SetField(ref _averageTweetPerMinute, value);
    }

    public ChartValues<LiveCharts.Defaults.ObservableValue> AverageTweetObservableValues { get; set; } = new();
    public ChartValues<LiveCharts.Defaults.ObservableValue> TweetReceivedObservableValues { get; set; } = new();

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}