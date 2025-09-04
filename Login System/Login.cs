using Godot;
using System;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
public partial class Login : Control
{
    //[Signal]
    //public delegate void LoginEventHandler(string username, string password);

    private LineEdit usernameInput, passwordInput;
    private Label errorMessage;

    private string filePath = "user://users.txt";

    int buttonPress = 0;

    public override void _Ready()
    {
        usernameInput = GetNode<LineEdit>("Username");
        passwordInput = GetNode<LineEdit>("Password");
        errorMessage = GetNode<Label>("ErrorMessage");

        
    }

    public override void _Process(double delta)
    {

    }
    public void OnLoginPressed()
    {

        string username = usernameInput.Text.Trim(); // Trim is used to remove accedental spaces
        string password = passwordInput.Text;

        if (string.IsNullOrWhiteSpace(username))
        {
            ShowError("Cannot leave Username Blank");
            return;
        }
        if (username.Length < 4 || username.Length > 20)
        {
            ShowError("Username length must be between 4 and 20 characters.");
            return;
        }
        if (username.Contains(" "))
        {
            ShowError("Username cannot contain Spaces");
            return;
        }


        if (string.IsNullOrWhiteSpace(password))
        {
            ShowError("Cannot leave Password Blank");
            return;
        }
        if (password.Length < 8 || password.Length > 20)
        {
            ShowError("Password length must be between 8 - 20 characters");
            return;
        }


        string realPath = ProjectSettings.GlobalizePath(filePath);

        // Convert entered password to same hex format as saved
        byte[] ba = Encoding.UTF8.GetBytes(password); // Coverts to Hex
        string hexPassword = BitConverter.ToString(ba).Replace("-", "");

        if (ValidateAccount(username, hexPassword, realPath) == true)
        {
            ShowSuccess("Login successful! Starting game...");
            return;
        }
        else
        {
            ShowError("Login Unsucessfull");
        }

        buttonPress += 1;
        ForgotPassword();
    }

    public void OnBackPressed()
    {
        GetTree().ChangeSceneToFile("res://Login System/LoginSystem.tscn");
    }

    private void ShowError(string msg)
    {
        if (errorMessage != null)
        {
            errorMessage.Text = msg;
            errorMessage.Visible = true;
        }
        else
        {
            GD.Print("ErrorMessage is null! Check node path.");
        }
    }

    private void ShowSuccess(string msg)
    {
        if (errorMessage != null)
        {
            errorMessage.Text = msg;
            errorMessage.Visible = true;
        }
        else
        {
            GD.Print("ErrorMessage is null! Check node path.");
        }
    }

    public void ForgotPassword() // Activates Forgot password Option
    {
        if (buttonPress == 3)
        {
            ShowError("Have You Forgotten Your Password?");
            GetNode<Button>("ForgotPassword").Visible = true;
        }
        if (buttonPress == 4)
        {
            ShowError("You will be locked out if you enter incorrectly again.");
        }
        if (buttonPress == 5)
        {
            GetTree().ChangeSceneToFile("res://MainMenu.tscn");
        }
    }

    public void OnForgotPasswordPressed()
    {
        GetTree().ChangeSceneToFile("res://Login System/ForgotPassword.tscn"); // Goes to forgot Password page.
    }

    static bool ValidateAccount(string username, string hexPassword, string realPath) // Checks if username and Password Matches.
    {
        foreach (var line in File.ReadAllLines(realPath))
        {
            var parts = line.Split(' ');
            if (parts.Length >= 3 && parts[0] == username && parts[2] == hexPassword)
            {
                return true;
            }
        }
        return false;
    }

    public void OnViewPasswordToggled(bool button_pressed) // boolean to see if button is pressed
    {
        var passwordBox = GetNode<LineEdit>("Password"); // declares what is the button
        passwordBox.Secret = !button_pressed;  // toggles it as Visible & invisible
    }
}
