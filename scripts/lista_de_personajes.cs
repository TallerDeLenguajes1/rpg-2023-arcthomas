using System;
using System.IO;
using System.Collections.Generic;
using System.Text.Json;
using espacioPersonajes;
using Godot;

public partial class lista_de_personajes : Node2D
{

    private int selout = 0;
    public override void _Ready()
    {
        string jsonString;
        // Leer el archivo JSON
        try
        {
            jsonString = File.ReadAllText("ListaPersonajes.json");
        }
        catch (Exception e)
        {
            GD.Print("Error al leer el archivo JSON: " + e.Message);
            return;
        }
        List<Button> characterButtons = new List<Button>();
        List<Button> selectedButtons = new List<Button>();
        personajesJson instpj = new personajesJson();
        List<personaje> listapj = new List<personaje>();
        listapj = instpj.leerPersonajes("ListaPersonajes.json");
        var confirm = GetNode("../Control2/Confirm") as Button;
        confirm.Disabled = true;
        // Recorrer la lista de objetos y crear nodos para cada uno
        int i = 0;
        foreach (personaje jsonpj in listapj)
        {
            string nombre = jsonpj.Name.ToString();
            float vida = Convert.ToSingle(jsonpj.Hp);
            var etiquetas = GetTree().GetNodesInGroup("Etiquetas");
            if (etiquetas[i] is Label label)
            {
                label.Text = jsonpj.Tipo;
            }
            var sprites = GetTree().GetNodesInGroup("CharacterSprites");
            if (sprites[i] is Sprite2D sprite)
            {
                switch (jsonpj.Tipo)
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
            // Obtener todos los botones de personajes en la escena
            foreach (Node node in GetTree().GetNodesInGroup("CharacterButtons"))
            {
                if (node is Button button)
                {
                    characterButtons.Add(button);
                    if (!button.IsConnected(Button.SignalName.Pressed, new Callable(this, MethodName.OnCharacterButtonPressed)))
                    {
                        button.Connect(Button.SignalName.Pressed, new Callable(this, MethodName.OnCharacterButtonPressed));
                    }
                }
            }
            i++;
        }
        var ap = GetNode("../Control/AnimationPlayer") as AnimationPlayer;
        ap.PlayBackwards("fade_in");
    }
    private void OnCharacterButtonPressed()
    {
        List<Button> characterButtons = new List<Button>();
        List<Button> selectedButtons = new List<Button>();
        personajesJson instpj = new personajesJson();
        List<personaje> listapj = new List<personaje>();

        int p1 = 11;
        int p2 = 11;
        foreach (Node node in GetTree().GetNodesInGroup("CharacterButtons"))
        {
            if (node is Button button)
            {
                characterButtons.Add(button);
                if (button.ButtonPressed)
                {
                    selectedButtons.Add(button);
                }
            }
        }
        if (selectedButtons.Count == 2)
        {
            p1 = int.Parse(selectedButtons[0].Name);
            p2 = int.Parse(selectedButtons[1].Name);

            foreach (Button button in characterButtons)
            {
                button.Disabled = !button.ButtonPressed;
            }
            listapj = instpj.leerPersonajes("ListaPersonajes.json");
            string pj1 = JsonSerializer.Serialize(listapj[p1 - 1]);
            string pj2 = JsonSerializer.Serialize(listapj[p2 - 1]);
            File.WriteAllText("Char1.json", pj1);
            File.WriteAllText("Char2.json", pj2);
            var confirm = GetNode("../Control2/Confirm") as Button;
            confirm.Disabled = false;
            confirm.Connect("pressed", new Callable(this, "OnConfirm"));
        }
    }
    private async void OnConfirm()
    {
        var ap = GetNode("../Control/AnimationPlayer") as AnimationPlayer;
        ap.Play("fade_in");
        await (ToSignal(ap, "animation_finished"));
        GetTree().ChangeSceneToFile("res://scenes/main.tscn");
    }
    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }
}

