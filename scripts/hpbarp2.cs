using Godot;
using espacioPersonajes;
using System;
using System.IO;
using System.Text.Json;
using System.Collections.Generic;

public partial class hpbarp2 : Control
{
	[Signal]
	public delegate void Char2DeadEventHandler();
	private int ataque = 0;
	private int efectividad = 0;
	private int defensa = 0;
	private const int ajuste = 500;
	private int dmg = 0;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		string contenidoJson = File.ReadAllText("Char2.json");
		personaje char2 = JsonSerializer.Deserialize<personaje>(contenidoJson);
		Timer timer = GetNode("../../Hit") as Timer;
		timer.Connect("timeout", new Callable(this, "OnHit"));
		ProgressBar hp = GetNode("ProgressBar") as ProgressBar;
		Label text = GetNode("Label") as Label;
		hp.Value = char2.Hp;
		text.Text = "HP: " + char2.Hp + "/100"; 
	}

	void OnHit()
	{
		var rand = new Random((int)DateTime.Now.Ticks);
		string contenidoJson = File.ReadAllText("Char1.json");
		string contenidoJson2 = File.ReadAllText("Char2.json");
		personaje defender = JsonSerializer.Deserialize<personaje>(contenidoJson2);
		personaje attacker = JsonSerializer.Deserialize<personaje>(contenidoJson);
		efectividad = rand.Next(1, 101);
		ataque = attacker.Dest * attacker.Fuerza * attacker.Nivel;
		defensa = defender.Arm * defender.Vel;
		dmg = ((ataque * efectividad) - defensa) / ajuste;
		ProgressBar hp = GetNode("ProgressBar") as ProgressBar;
		Label text = GetNode("Label") as Label;
		hp.Value -= dmg;
		text.Text = "HP: " + hp.Value + "/100";
		if (hp.Value <= 0)
		{
			EmitSignal(SignalName.Char2Dead);
		}
	}
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
