using Godot;
using System;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
public partial class LoginSystem : Control
{
    private string filePath = "user://users.txt";
    public override void _Ready()
    {
        string realPath = ProjectSettings.GlobalizePath(filePath); // Globalizes the file
        if (!File.Exists(realPath))
        {
            GetNode<Control>("Login/Login").Visible = false; // Cannot Login if no account has been created.
        }
    }

    public override void _Process(double delta)
    {

    }


    public void OnBackPressed()
    {
        GetTree().ChangeSceneToFile("res://MainMenu.tscn");
    }

    public void OnLoginPressed()
    {
        GetTree().ChangeSceneToFile("res://Login System/Login.tscn");
    }

    public void OnSignInPressed()
    {
        GetTree().ChangeSceneToFile("res://Login System/SignIn.tscn");
    }

}