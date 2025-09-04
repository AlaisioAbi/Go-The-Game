using Godot;
using System;

public partial class BackgroundMusic : Control
{

    public override void _Ready()
    {
        GetNode<AudioStreamPlayer2D>("BackgroundMusic").Play();
    }

}
