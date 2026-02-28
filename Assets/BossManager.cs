using UnityEngine;
using System.Collections.Generic;

public class Part
{
    public Sprite sprite;
    public Vector2[] weak_spot_candidates;

    public Part(Sprite sprite, Vector2[] weak_spot_candidats)
    {
        this.sprite = sprite;
        this.weak_spot_candidates = weak_spot_candidates;
    }
}

public class BossManager : MonoBehaviour
{
    int lastIncrement = -1;
    public SpriteRenderer HEAD_RENDERER;
    List<Part> heads;
    public SpriteRenderer TORSO_RENDERER;
    List<Part> torsos;
    public SpriteRenderer LEGS_RENDERER;
    List<Part> legs;

List<Part> CreatePartArray(string folderA, string folderB)
{
    // Load Sprites instead of Texture2D
    Sprite[] folderASprites = Resources.LoadAll<Sprite>(folderA);
    Sprite[] folderBSprites = Resources.LoadAll<Sprite>(folderB);

    // Create lookup for folderB textures
    Dictionary<string, Texture2D> folderBDictionary = new Dictionary<string, Texture2D>();
    foreach (Sprite s in folderBSprites)
    {
        Texture2D tex = s.texture;
        if (!folderBDictionary.ContainsKey(s.name))
            folderBDictionary.Add(s.name, tex);
    }

    List<Part> partsList = new List<Part>();

    foreach (Sprite sprite in folderASprites)
    {
        if (!folderBDictionary.TryGetValue(sprite.name, out Texture2D textureB))
        {
            Debug.LogWarning("No matching texture found in FolderB for: " + sprite.name);
            continue;
        }

        // Make sure the texture is readable
        if (!textureB.isReadable)
        {
            Debug.LogError(textureB.name + " is not readable. Enable Read/Write in import settings.");
            continue;
        }

        // Collect non-transparent pixels from textureB
        List<Vector2> solidPixels = new List<Vector2>();
        Color32[] pixels = textureB.GetPixels32();
        int width = textureB.width;
        int height = textureB.height;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Color32 pixel = pixels[y * width + x];
                if (pixel.a > 0)
                    solidPixels.Add(new Vector2(x, y));
            }
        }

        partsList.Add(new Part(sprite, solidPixels.ToArray()));
    }

    return partsList;
}

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        heads = CreatePartArray("heads", "head_spots");
        torsos = CreatePartArray("torsos", "torso_spots");
        legs = CreatePartArray("legs", "leg_spots");
    }

    // Update is called once per frame
    void Update()
    {
        int currentIncrement = (int)Mathf.Round(Time.time*3);
        if(lastIncrement != currentIncrement)
        {
            lastIncrement = currentIncrement;

            int index = UnityEngine.Random.Range(0, heads.Count);
            HEAD_RENDERER.sprite = heads[index].sprite;
            index = UnityEngine.Random.Range(0, heads.Count);
            TORSO_RENDERER.sprite = torsos[index].sprite;
            index = UnityEngine.Random.Range(0, heads.Count);
            LEGS_RENDERER.sprite = legs[index].sprite;
            
        }
    }
}
