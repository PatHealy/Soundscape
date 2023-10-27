using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNameManager : MonoBehaviour
{
    public static PlayerNameManager instance;
    public Dictionary<string, string> players;
    public Dictionary<string, string> colors;
    string[] possibleNames = new string[] {"Accent", "Adagio", "Allegro", "Alto", "Andante", "Arpeggio", "Bar", "Cadence", "Cadenza", "Canon", "Clef", "Coda", "Crescendo", "Da Capo", "Dal Segno", "Diminuendo", "Fermata", "Flat", "Forte", "Fortepiano", "Giocoso", "Glissando", "Glockenspiel", "Largo/Larghetto", "Leggero", "Legato", "Motif", "Natural", "Nonet", "Ostinato", "Pan", "Pianissimo", "Pizzicato", "Quarter", "Quintuplet", "Rhapsody", "Rondo", "Scherzo", "Sforzando", "Sharp", "Soprano", "Sostenuto", "Staccato", "Tempo", "Tenor", "Tremolo", "Trill", "Vibrato", "Vivace"};
    string[] possibleColors = new string[] { "blue", "green", "orange", "purple", "red", "yellow"};

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        players = new Dictionary<string, string>();
        colors = new Dictionary<string, string>();
    }

    public string GetName(string ip) {
        if (players.ContainsKey(ip)) {
            return "<color=" + colors[ip] + ">" + players[ip] + "</color>: ";
        }

        string possible = possibleNames[Random.Range(0, possibleNames.Length)];
        int i = 0;
        while (players.ContainsKey(possible) && i < 10) {
            possible = possibleNames[Random.Range(0, possibleNames.Length)];
            i++;
        }

        colors[ip] = possibleColors[Random.Range(0, possibleColors.Length)];
        players[ip] = possible;

        return "<color=" + colors[ip] + ">" + players[ip] + "</color>: ";
    }
}
