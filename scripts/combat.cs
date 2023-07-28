using Godot;
using espacioPersonajes;
using System;
using System.IO;
using System.Text.Json;
using System.Collections.Generic;

public partial class combat : Control
{
    [Signal]
    public delegate void PlayerOneFinishEventHandler();
    [Signal]
    public delegate void RoundStartEventHandler();
    public int turnCounter = 1;
    public int roundCounter = 1;
    public int randClass = 0;
    public List<string> bufadasp1 = new List<string>();
    public List<string> bufadasp2 = new List<string>();
    public const int buff = 5;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Label turnC = GetNode("TurnCounter") as Label;
        Label roundC = GetNode("RoundCounter") as Label;
        turnC.Text += " " + turnCounter;
        roundC.Text = "Ronda " + roundCounter + "/3";
        Button button = GetNode("AttackB") as Button;
        button.Connect("pressed", new Callable(this, "Attack"));
        GetNode("PJ1/Control").Connect("Char1Dead", new Callable(this, "OnChar1Dead"));
        GetNode("PJ2/Control").Connect("Char2Dead", new Callable(this, "OnChar2Dead"));
        AnimationPlayer fade = GetNode("Fade/Fade") as AnimationPlayer;
        fade.PlayBackwards("fade_in");
        Timer title = GetNode("TitleEnd") as Timer;
        title.Connect("timeout", new Callable(this, "OnTitle"));
        this.Connect("PlayerOneFinish", new Callable(this, "OnTurnEnd"));
        this.Connect("RoundStart", new Callable(this, "OnRoundStart"));
        string contenidoJson = File.ReadAllText("Char1.json");
        personaje char1 = JsonSerializer.Deserialize<personaje>(contenidoJson);
        string contenidoJson2 = File.ReadAllText("Char2.json");
        personaje char2 = JsonSerializer.Deserialize<personaje>(contenidoJson2);

        Label infop1 = GetNode("InfoPJ1") as Label;
        Label infop2 = GetNode("InfoPJ2") as Label;
        infop1.Text = char1.Name + "\n\nFRZ: " + char1.Fuerza + "\nVEL: " + char1.Vel + "\nARM: " + char1.Arm + "\nDES: " + char1.Dest + "\nLVL: " + char1.Nivel;
        infop2.Text = char2.Name + "\n\nFRZ: " + char2.Fuerza + "\nVEL: " + char2.Vel + "\nARM: " + char2.Arm + "\nDES: " + char2.Dest + "\nLVL: " + char2.Nivel;
        Sprite2D char1sp = GetNode("PJ1/Char1") as Sprite2D;
        Sprite2D char2sp = GetNode("PJ2/Char1") as Sprite2D;
        switch (char1.Tipo)
        {
            case "Tanque":
                char1sp.Texture = ResourceLoader.Load("res://rcs/tankok.png") as Texture2D;
                break;
            case "Arquero":
                char1sp.Texture = ResourceLoader.Load("res://rcs/archerok.png") as Texture2D;
                break;
            case "Mago":
                char1sp.Texture = ResourceLoader.Load("res://rcs/wizardok.png") as Texture2D;
                break;
            case "Apoyo":
                char1sp.Texture = ResourceLoader.Load("res://rcs/bardok.png") as Texture2D;
                break;
            case "Barbaro":
                char1sp.Texture = ResourceLoader.Load("res://rcs/barbarianok.png") as Texture2D;
                break;
            default:
                break;
        }
        switch (char2.Tipo)
        {
            case "Tanque":
                char2sp.Texture = ResourceLoader.Load("res://rcs/tankok.png") as Texture2D;
                break;
            case "Arquero":
                char2sp.Texture = ResourceLoader.Load("res://rcs/archerok.png") as Texture2D;
                break;
            case "Mago":
                char2sp.Texture = ResourceLoader.Load("res://rcs/wizardok.png") as Texture2D;
                break;
            case "Apoyo":
                char2sp.Texture = ResourceLoader.Load("res://rcs/bardok.png") as Texture2D;
                break;
            case "Barbaro":
                char2sp.Texture = ResourceLoader.Load("res://rcs/barbarianok.png") as Texture2D;
                break;
            default:
                break;
        }
    }

    async void Attack()
    {
        Timer timer = GetNode("Hit") as Timer;
        Timer timer2 = GetNode("Hit2") as Timer;
        Label turnC = GetNode("TurnCounter") as Label;
        Button button = GetNode("AttackB") as Button;
        AnimationPlayer animPlayer = GetNode("PJ1/AnimationPlayer") as AnimationPlayer;
        AnimationPlayer animPlayer2 = GetNode("PJ2/AnimationPlayer") as AnimationPlayer;
        timer.Start();
        animPlayer.Play("attack");
        animPlayer2.Play("hurt");
        button.Disabled = true;
        await (ToSignal(animPlayer, "animation_finished"));
        EmitSignal(SignalName.PlayerOneFinish);
    }

    async void OnTurnEnd()
    {
        ProgressBar hpChar2 = GetNode("PJ2/Control/ProgressBar") as ProgressBar;
        Timer timer2 = GetNode("Hit2") as Timer;
        Label turnC = GetNode("TurnCounter") as Label;
        Button button = GetNode("AttackB") as Button;
        AnimationPlayer animPlayer = GetNode("PJ1/AnimationPlayer") as AnimationPlayer;
        AnimationPlayer animPlayer2 = GetNode("PJ2/AnimationPlayer") as AnimationPlayer;
        if (hpChar2.Value > 0)
        {
            timer2.Start();
            animPlayer2.Play("attacknpc");
            animPlayer.Play("hurt");
            await (ToSignal(animPlayer2, "animation_finished"));
            turnCounter++;
            turnC.Text = "Turno " + turnCounter;
            button.Disabled = false;
        }
    }

    async void OnChar1Dead()
    {
        var rand = new Random((int)DateTime.Now.Ticks);
        randClass = rand.Next(1, 6);
        string contenidoJson = File.ReadAllText("Char1.json");
        personaje char1 = JsonSerializer.Deserialize<personaje>(contenidoJson);
        Timer title = GetNode("TitleEnd") as Timer;
        Label roundTitle = GetNode("RoundTitle") as Label;
        AnimationPlayer fade = GetNode("Fade/Fade") as AnimationPlayer;
        fade.Play("fade_in");
        await (ToSignal(fade, "animation_finished"));
        if (roundCounter < 3)
        {
            roundCounter++;
            roundTitle.Text = "Ronda " + roundCounter;
            roundTitle.Visible = true;
            // 1. Fuerza, 2. Armadura, 3. Nivel, 4. Destreza, 5. Velocidad
            switch (randClass)
            {
                case 1:
                    char1.Fuerza += buff;
                    bufadasp1.Add("FRZ");
                    break;
                case 2:
                    char1.Arm += buff;
                    bufadasp1.Add("DEF");
                    break;
                case 3:
                    char1.Nivel += buff;
                    bufadasp1.Add("LVL");
                    break;
                case 4:
                    char1.Dest += buff;
                    bufadasp1.Add("DES");
                    break;
                case 5:
                    char1.Vel += buff;
                    bufadasp1.Add("VEL");
                    break;
                default:
                    break;
            }
            contenidoJson = JsonSerializer.Serialize(char1);
            File.WriteAllText("Char1.json", contenidoJson);
            title.Start();
        }
        else
        {
            GetTree().ChangeSceneToFile("res://scenes/loser.tscn");
        }
    }
    async void OnChar2Dead()
    {
        var rand = new Random((int)DateTime.Now.Ticks);
        randClass = rand.Next(1, 6);
        string contenidoJson = File.ReadAllText("Char2.json");
        personaje char2 = JsonSerializer.Deserialize<personaje>(contenidoJson);
        Timer title = GetNode("TitleEnd") as Timer;
        Label roundTitle = GetNode("RoundTitle") as Label;
        AnimationPlayer fade = GetNode("Fade/Fade") as AnimationPlayer;
        fade.Play("fade_in");
        await (ToSignal(fade, "animation_finished"));
        if (roundCounter < 3)
        {
            roundCounter++;
            roundTitle.Text = "Ronda " + roundCounter;
            roundTitle.Visible = true;
            // 1. Fuerza, 2. Armadura, 3. Nivel, 4. Destreza, 5. Velocidad
            switch (randClass)
            {
                case 1:
                    char2.Fuerza += buff;
                    bufadasp2.Add("FRZ");
                    break;
                case 2:
                    char2.Arm += buff;
                    bufadasp2.Add("DEF");
                    break;
                case 3:
                    char2.Nivel += buff;
                    bufadasp2.Add("LVL");
                    break;
                case 4:
                    char2.Dest += buff;
                    bufadasp2.Add("DES");
                    break;
                case 5:
                    char2.Vel += buff;
                    bufadasp2.Add("VEL");
                    break;
                default:
                    break;
            }
            contenidoJson = JsonSerializer.Serialize(char2);
            File.WriteAllText("Char2.json", contenidoJson);
            title.Start();
        }
        else
        {
            GetTree().ChangeSceneToFile("res://scenes/winner.tscn");
        }
    }

    void OnTitle()
    {
        Label infop1 = GetNode("InfoPJ1") as Label;
        foreach (string stat in bufadasp1)
        {
            // Agregar el texto "+5" al lado de la estadística bufada
            infop1.Text += "\n+5 " + stat;
        }
        bufadasp1.Clear();
        Label infop2 = GetNode("InfoPJ2") as Label;
        foreach (string stat in bufadasp2)
        {
            // Agregar el texto "+5" al lado de la estadística bufada
            infop2.Text += "\n+5 " + stat;
        }
        bufadasp2.Clear();
        Label roundTitle = GetNode("RoundTitle") as Label;
        EmitSignal(SignalName.RoundStart);
        roundTitle.Visible = false;
        Label roundC = GetNode("RoundCounter") as Label;
        roundC.Text = "Ronda " + roundCounter + "/3";
        AnimationPlayer fade = GetNode("Fade/Fade") as AnimationPlayer;
        fade.PlayBackwards("fade_in");
    }

    void OnRoundStart()
    {
        Button button = GetNode("AttackB") as Button;
        ProgressBar hp1 = GetNode("PJ1/Control/ProgressBar") as ProgressBar;
        ProgressBar hp2 = GetNode("PJ2/Control/ProgressBar") as ProgressBar;
        button.Disabled = false;
        hp1.Value = 100;
        hp2.Value = 100;
        Label textp1 = GetNode("PJ1/Control/Label") as Label;
        textp1.Text = "HP: " + hp1.Value + "/100";
        Label textp2 = GetNode("PJ2/Control/Label") as Label;
        textp2.Text = "HP: " + hp1.Value + "/100";
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }
}
