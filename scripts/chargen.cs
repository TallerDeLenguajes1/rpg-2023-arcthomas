﻿using System;
using System.Collections.Generic;
using espacioPersonajes;
using System.Text.Json;

namespace chargen
{
    public class Gen
    {
        public static void Generate()
        {
            personaje nuevo;
            List<personaje> listaPjs = new List<personaje>();
            List<personaje> listaNueva = new List<personaje>();
            FabricaDePersonajes fp = new FabricaDePersonajes();
            for (int i = 0; i < 10; i++)
            {
                nuevo = fp.crearPersonaje();
                listaPjs.Add(nuevo);
            }

            personajesJson pjson = new personajesJson();

            if (pjson.existeArchivo("ListaPersonajes.json"))
            {
                Console.WriteLine("Existe");
            }
            else
            {
                Console.WriteLine("No Existe");
            }

            pjson.guardarPjs(listaPjs);

            listaNueva = pjson.leerPersonajes("ListaPersonajes.json");
        }
    }
}