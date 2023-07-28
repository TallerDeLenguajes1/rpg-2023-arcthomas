using Godot;
using espacioPersonajes;
using System;
using System.IO;
using System.Text.Json;
using System.Collections.Generic;

public partial class winner : Control
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		string contenidoJson = File.ReadAllText("Char1.json");
        personaje char1 = JsonSerializer.Deserialize<personaje>(contenidoJson);
		Label win = GetNode("Trono") as Label;
		win.Text += char1.Name;
		Sprite2D sprite = GetNode("WSprite") as Sprite2D;
		switch (char1.Tipo)
                {
                    case "Tanque":
                        sprite.Texture = ResourceLoader.Load("res://rcs/tankok.png") as Texture2D;
                        break;
                    case "Arquero":
                        sprite.Texture = ResourceLoader.Load("res://rcs/archerok.png") as Texture2D;
                        break;
                    case "Mago":
                        sprite.Texture = ResourceLoader.Load("res://rcs/wizardok.png") as Texture2D;
                        break;
                    case "Apoyo":
                        sprite.Texture = ResourceLoader.Load("res://rcs/bardok.png") as Texture2D;
                        break;
                    case "Barbaro":
                        sprite.Texture = ResourceLoader.Load("res://rcs/barbarianok.png") as Texture2D;
                        break;
                    default:
                        break;
                }
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
