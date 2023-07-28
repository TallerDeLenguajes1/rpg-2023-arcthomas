using System;
using System.IO;
using System.Collections.Generic;
using System.Text.Json;
using Godot;

namespace espacioPersonajes
{

    public class personaje
    {
        private string? tipo;
        private string? name;
        private DateTime fecNac;
        private int edad;
        private int vel;
        private int dest;
        private int fuerza;
        private int nivel;
        private int arm;
        private int hp;

        public string? Tipo { get => tipo; set => tipo = value; }
        public string? Name { get => name; set => name = value; }
        public DateTime FecNac { get => fecNac; set => fecNac = value; }
        public int Edad { get => edad; set => edad = value; }
        public int Vel { get => vel; set => vel = value; }
        public int Dest { get => dest; set => dest = value; }
        public int Fuerza { get => fuerza; set => fuerza = value; }
        public int Nivel { get => nivel; set => nivel = value; }
        public int Arm { get => arm; set => arm = value; }
        public int Hp { get => hp; set => hp = value; }

    }

    public class FabricaDePersonajes
    {
        string[] tipos = { "Tanque", "Arquero", "Mago", "Apoyo", "Barbaro" };
        string[] names = { "Thomas", "Agustin", "Ignacio", "Omar", "Juan" };

        public personaje crearPersonaje()
        {
            personaje nuevo = new personaje();
            Random rnd = new Random();
            int auxTipo = rnd.Next(0, 5);
            nuevo.Tipo = tipos[auxTipo];
            int auxName = rnd.Next(0, 5);
            nuevo.Name = names[auxName];
            int dia = rnd.Next(1, 31);
            int mes = rnd.Next(1, 13);
            int anio = rnd.Next(1723, 2024);
            nuevo.FecNac = new DateTime(anio, mes, dia);
            nuevo.Edad = 2023 - anio;
            nuevo.Hp = 100;
            nuevo.Nivel = rnd.Next(1, 11);
            nuevo.Arm = rnd.Next(1, 11);
            nuevo.Dest = rnd.Next(1, 6);
            nuevo.Vel = rnd.Next(1, 11);
            nuevo.Fuerza = rnd.Next(1, 11);

            return nuevo;
        }
    }

    public class personajesJson
    {
        public bool existeArchivo(string archivo)
        {
            return File.Exists(archivo);
        }
        public void guardarPjs(List<personaje> lista)
        {
            string contenidoJson = JsonSerializer.Serialize(lista);
            File.WriteAllText("ListaPersonajes.json", contenidoJson);
        }
        public List<personaje> leerPersonajes(string archivo)
        {
            string contenidoJson = File.ReadAllText(archivo);
            List<personaje> listapersonajes = JsonSerializer.Deserialize<List<personaje>>(contenidoJson);
            return listapersonajes;
        }
    }
}