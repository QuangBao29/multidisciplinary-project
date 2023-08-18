using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class RecognizerParser
{
    private static Dictionary<char, Color> _colorsByChar = new()
    {
        {'R', new Color(1, 0, 0)},
        {'G', new Color(0, 1, 0)},
        {'B', new Color(0, 0, 1)},
        {'Y', new Color(1, 1, 0)},
        {'C', new Color(0, 1, 1)},
        {'M', new Color(1, 0, 1)},
    };

    public static void Save(string str, string name, string path = "")
    {
        if (path == "") path = Application.persistentDataPath;

        var texture = new Texture2D(2048, 2048, TextureFormat.RGB24, false);

        for (int i = 0; i < str.Length; i++)
        {
            int xStart = i % 4 * 512;
            int yStart = i / 4 * 512;

            for (int x = xStart; x < xStart + 512; x++)
            {
                for (int y = yStart; y < yStart + 512; y++)
                {
                    texture.SetPixel(x, y, _colorsByChar[str[i]]);
                }
            }
        }

        texture.Apply();
        File.WriteAllBytes(path + "/" + name + ".png", texture.EncodeToPNG());
        Debug.Log("Recognizer saved as " + path + "/" + name + ".png");
    }
}