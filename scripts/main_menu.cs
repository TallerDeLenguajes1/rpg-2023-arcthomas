using Godot;
using System;
using chargen;

public partial class main_menu : Control
{
	private AnimationPlayer _animationPlayer = new AnimationPlayer();
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Gen.Generate();
		var ap = GetNode("AnimationPlayer") as AnimationPlayer;
		var button = GetNode("Button") as Button;
		button.Connect("pressed", new Callable(this,"_on_button_pressed"));
	}

	async void _on_button_pressed()
	{
		var ap = GetNode("AnimationPlayer") as AnimationPlayer;
		ap.Play("fade_in");
		await (ToSignal(ap, "animation_finished"));
		GetTree().ChangeSceneToFile("res://scenes/char_select.tscn");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
