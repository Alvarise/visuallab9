using Avalonia;
using Avalonia.Controls.Primitives;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reactive;
using System.Runtime.CompilerServices;

namespace VolumeSliderAvalonia;

public class VolumeSlider : TemplatedControl, INotifyPropertyChanged
{
    public static readonly StyledProperty<bool> IsSliderVisibleProperty =
            AvaloniaProperty.Register<VolumeSlider, bool>("IsSliderVisible");

    public event PropertyChangedEventHandler? PropertyChanged;
    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }

    private int sliderValue;
    public int SliderValue
    {
        get => sliderValue;
        set
        {
            SetField(ref sliderValue, value);
            audioController.DefaultPlaybackDevice.Volume = sliderValue;
            FormattedSliderValue = sliderValue.ToString() + "%";
        }
    }

    private string formattedSliderValue;

    public string FormattedSliderValue
    {
        get => formattedSliderValue;
        set => _ = SetField(ref formattedSliderValue, value);
    }

    public bool IsSliderVisible
    {
        get => GetValue(IsSliderVisibleProperty);
        set
        {
            SetValue(IsSliderVisibleProperty, value);
        }
    }

    public ReactiveCommand<Unit, Unit> CloseButtonCommand { get; }

    private void CloseButtonAction()
    {
        IsSliderVisible = false;
    }

    private AudioSwitcher.AudioApi.CoreAudio.CoreAudioController
        audioController = new AudioSwitcher.AudioApi.CoreAudio.CoreAudioController();
    public ReactiveCommand<Unit, Unit> OpenSliderCommand { get; }

    private void RectangleTappedAction()
    {
        IsSliderVisible = true;
    }

    public VolumeSlider()
    {
        CloseButtonCommand = ReactiveCommand.Create(CloseButtonAction);
        OpenSliderCommand = ReactiveCommand.Create(RectangleTappedAction);
        SliderValue = Convert.ToInt32(audioController.DefaultPlaybackDevice.Volume);
    }
}