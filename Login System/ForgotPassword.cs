using Godot;
using System;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

public partial class ForgotPassword : Control
{
    private string filePath = "user://users.txt";
    private LineEdit emailInput, passwordInput, rPasswordInput;
    private Label errorMessage;
    public override void _Ready()
    {
        string realPath = ProjectSettings.GlobalizePath(filePath);
        emailInput = GetNode<LineEdit>("Email");
        passwordInput = GetNode<LineEdit>("NewPassword");
        rPasswordInput = GetNode<LineEdit>("ConfirmNewPassword");
        errorMessage = GetNode<Label>("ErrorMessage");
    }

    public void OnChangePasswordPressed()
    {
        string email = emailInput.Text.Trim();
        string password = passwordInput.Text;
        string passwordConfirmation = rPasswordInput.Text;
        string realPath = ProjectSettings.GlobalizePath(filePath);


        if (string.IsNullOrWhiteSpace(email))
        {
            ShowError("Cannot leave Email Blank");
            return;
        }
        if (string.IsNullOrWhiteSpace(password))
        {
            ShowError("Cannot leave Password Blank");
            return;
        }
        if (string.IsNullOrWhiteSpace(passwordConfirmation))
        {
            ShowError("Cannot leave Password Blank");
            return;
        }

        if (!EmailExists(email, realPath))
        {
            ShowError("Email Does Not Exist!");
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
        if (password == passwordConfirmation)
        {
            byte[] ba = Encoding.UTF8.GetBytes(password); // Coverts to Hex
            string hexPassword = BitConverter.ToString(ba).Replace("-", "");
            OverwritePassword(email, hexPassword, realPath);
            ShowSuccess("Password Has Been Updated Sucessfully");

            GetTree().ChangeSceneToFile("res://Login System/Login.tscn");
        }
        else
        {
            ShowError("Passwords don't match");
        }

    }

    public void OnBackPressed()
    {
        GetTree().ChangeSceneToFile("res://Login System/Login.tscn");
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

    static void OverwritePassword(string email, string hexPassword, string realPath)
    {
        string[] lines = File.ReadAllLines(realPath);
        for (int i = 0; i < lines.Length; i++)
        {
            var parts = lines[i].Split(' ');
            if (parts.Length >= 3 && parts[1] == email) //Checks if email is the same
            {
                parts[2] = hexPassword; // updates password
                lines[i] = string.Join(" ", parts);
                break;
            }
        }
        File.WriteAllLines(realPath, lines); // saves changes
    }

    public void OnViewPasswordToggled(bool button_pressed) // boolean to see if button is pressed
    {
        var passwordBox = GetNode<LineEdit>("NewPassword"); // declares what is the button
        passwordBox.Secret = !button_pressed;  // toggles it as Visible & invisible

        var passwordBox1 = GetNode<LineEdit>("ConfirmNewPassword");
        passwordBox1.Secret = !button_pressed;
    }
    
}
