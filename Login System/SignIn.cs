using Godot;
using System;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

public partial class SignIn : Control
{
    //[Signal]
    //public delegate void SignInEventHandler(string username,string password, string passwordConfirmation, string email);


    private LineEdit usernameInput, emailInput, passwordInput, passwordConfirmationInput; // These atributes are declared so that they can be used in the code.
    private Label errorMessage;

    private string filePath = "user://users.txt"; // filePath is declared 
    public override void _Ready()
    {
        usernameInput = GetNode<LineEdit>("Username"); // Converts text into stirng that can be used in code
        emailInput = GetNode<LineEdit>("Email");
        passwordInput = GetNode<LineEdit>("Password");
        passwordConfirmationInput = GetNode<LineEdit>("PasswordConfirmation");
        errorMessage = GetNode<Label>("ErrorMessage");

        string realPath = ProjectSettings.GlobalizePath(filePath);
        if (!File.Exists(realPath)) // Checks if filepath DOES NOT exist
        {
            File.Create(realPath).Close(); // Creats file if it didn't exist
        }
    }

    public override void _Process(double delta)
    {

    }

    public void OnSignInPressed()
    {

        string username = usernameInput.Text.Trim(); // Trim is used to remove accedental spaces.
        string email = emailInput.Text.Trim();
        string password = passwordInput.Text;
        string passwordConfirmation = passwordConfirmationInput.Text;


        // === Username Validation === //
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

        if (UsernameExists(username, ProjectSettings.GlobalizePath(filePath))) //This checks if the username exists 
        {
            ShowError("Username already exists. Please try another Username.");
            return;
        }

        // === Email Validation === //
        if (string.IsNullOrWhiteSpace(email))
        {
            ShowError("Cannot leave Email Blank");
            return;
        }

        if (!Regex.IsMatch(email, @"^\S+@\S+\.\S+$")) // Email Format
        {
            ShowError("Invalid email format. Use: [text]@[domain].[extension]");
            return;
        }

        if (EmailExists(email, ProjectSettings.GlobalizePath(filePath)))
        {
            ShowError("Email Already Exists!");
            return;
        }

        // === Password Verification === //
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
        if (!Regex.IsMatch(password, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#\$%\^&\*\-_\+=]).+$"))
        {
            ShowError("Password must include uppercase, lowercase, digit, and special character.");
            return;
        }
        if (password.Contains(" "))
        {
            ShowError("Password cannot contain Spaces");
            return;
        }
        if (password != passwordConfirmation)
        {
            ShowError("Passwords do not match!");
            return;
        }

        // === Save Account === //
        string realPath = ProjectSettings.GlobalizePath(filePath);

        byte[] ba = Encoding.UTF8.GetBytes(password);
        string hexPassword = BitConverter.ToString(ba).Replace("-", "");

        using (StreamWriter sw = File.AppendText(realPath))
        {
            sw.WriteLine($"{username} {email} {hexPassword}");
        }

        ShowSuccess("Account created successfully!");


        GetTree().ChangeSceneToFile("res://Login System/Login.tscn");
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
        errorMessage.Text = msg;
        errorMessage.Visible = true;
    }
    static bool UsernameExists(string username, string realPath)
    {

        foreach (var line in File.ReadAllLines(realPath)) // Reads all lines in the file
        {
            var parts = line.Split(' '); // splits username, email & password
            if (parts.Length >= 3 && parts[0] == username) // parts[0] = username & .Length is used to check if there is anything present at index 0
            {
                return true; // Username exists. No need to keep checking
            }
        }
        return false; // Username doesn't exist.
    }

    static bool EmailExists(string email, string realPath)
    {

        foreach (var line in File.ReadAllLines(realPath)) // Reads all lines in the file
        {
            var parts = line.Split(' '); // splits username, email & password
            if (parts.Length >= 3 && parts[1] == email) // parts[0] = username & .Length is used to check if there is anything present at index 0
            {
                return true; // Username exists. No need to keep checking
            }
        }
        return false; // Username doesn't exist.
    }

    public void OnViewPasswordToggled(bool button_pressed) // boolean to see if button is pressed
    {
        var passwordBox = GetNode<LineEdit>("Password"); // declares what is the button
        passwordBox.Secret = !button_pressed;  // toggles it as Visible & invisible

        var passwordBox1 = GetNode<LineEdit>("PasswordConfirmation");
        passwordBox1.Secret = !button_pressed;
    }

}
