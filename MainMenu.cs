using Godot;

public partial class MainMenu : Control
{


    public override void _Ready()
    {
        GetNode<CheckButton>("Fullscreen").Toggled += OnFullscreenToggled;
        GetNode<Slider>("SettingsMenu/MainVol").ValueChanged += OnMainVolChanged;
        GetNode<Slider>("SettingsMenu/SFXVol").ValueChanged += OnSFXVolChanged;
        GetNode<Slider>("SettingsMenu/MusicVol").ValueChanged += OnMusicVolChanged;

    }


    public override void _Process(double delta)
    {

    }

    public void OnPlayPressed()
    {
        GetTree().ChangeSceneToFile("res://Login System/LoginSystem.tscn");
    }

    public void OnSettingsPressed()
    {
        GetNode<Control>("CenterContainer/MainButtons").Visible = false;
        GetNode<Control>("CenterContainer/SettingsMenu").Visible = true; // Settings Menu is now visible.
    }

    public void OnCreditsPressed()
    {
        GetNode<Control>("CenterContainer/MainButtons").Visible = false;
        GetNode<Control>("CenterContainer/CreditsMenu").Visible = true; // Credits Menue is now visible
    }

    public void OnBackPressed()
    {
        GetNode<Control>("CenterContainer/MainButtons").Visible = true; // All other menues are invisible 
        GetNode<Control>("CenterContainer/SettingsMenu").Visible = false;
        GetNode<Control>("CenterContainer/CreditsMenu").Visible = false;
    }

    public void OnQuitPressed()
    {
        GetTree().Quit(); // quits game
    }
    public void OnMainVolChanged(double value)
    {
        GD.Print("Main Volume: " + value);
        AudioServer.SetBusVolumeDb(AudioServer.GetBusIndex("Master"), (float)value);
        GetNode<Label>("MainVol/MainVolume").Text = $"{Mathf.Round(value)} dB";
    }
    public void OnSFXVolChanged(double value)
    {
        GD.Print("SFX Volume: " + value);
        AudioServer.SetBusVolumeDb(AudioServer.GetBusIndex("SFX"), (float)value);
        GetNode<Label>("SFXVol/SFXVolume").Text = $"{Mathf.Round(value)} dB";
    }
    public void OnMusicVolChanged(double value)
    {
        GD.Print("Music Volume: " + value);
        AudioServer.SetBusVolumeDb(AudioServer.GetBusIndex("Music"), (float)value);
        GetNode<Label>("MusicVol/MusicVolume").Text = $"{Mathf.Round(value)} dB";
    }

    public void OnFullscreenToggled(bool buttonPressed)
    {
        if (buttonPressed == true)
        {
            DisplayServer.WindowSetMode(DisplayServer.WindowMode.Fullscreen);
        }
        else
        {
            DisplayServer.WindowSetMode(DisplayServer.WindowMode.Windowed);
        }
    }
    
    
}
