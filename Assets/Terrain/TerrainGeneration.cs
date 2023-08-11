using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class TerrainGeneration : MonoBehaviour
{
    public Sprite TileGrass;
    public Sprite TileGround;
    public Sprite TileStone;
    public Sprite log;
    public Sprite leaf;
    public FillRule ruleTileGround;

    public int treeChance = 10;
    public bool generateCaves = true;
    public float surfaceValue = 0.25f;
    public int worldSize = 100;
    public float caveFreq = 0.05f;
    public float terrainFreq = 0.05f;
    public float heightMultiplier = 40f;
    public int heightAddition = 25;
    public int groundLayerHight = 5;
    public float seed;
    public Texture2D noiseTexture;
    

    // Start is called before the first frame update
    private void Start()
    {
        //seed = Random.Range(10000, -10000);
        seed = -910;
        GenerateNoiseExtra();
        GenerateTerrain();
    }

    public void GenerateTerrain()
    {
        for (int x = 0; x < worldSize; x++)
        {
            float height = Mathf.PerlinNoise ((x + seed) * terrainFreq, seed * terrainFreq) * heightMultiplier + heightAddition;
            for (int y = 0; y < height; y++)
            {
                Sprite tileSprite;
                if (y < height - groundLayerHight)
                {
                    tileSprite = TileStone;

                }
                else if (y < height - 1)
                {
                    tileSprite = TileGround;
                }
                else
                {
                    tileSprite = TileGrass;//STONE BE HERE
                    //tileSprite = TileGrass;
                }

                if (generateCaves)
                {

                    if (noiseTexture.GetPixel(x, y).r > surfaceValue)
                    {
                        PlaceTile(x, y, tileSprite);
                    }
                }
                else
                {
                    PlaceTile(x, y, tileSprite);
                }
            }
        }
    }

    private void PlaceTile(int x, int y, Sprite tileSprite)
    {
        GameObject newTile = new GameObject();
        newTile.transform.parent = this.transform;
        newTile.AddComponent<SpriteRenderer>();
        newTile.GetComponent<SpriteRenderer>().sprite = tileSprite;
        newTile.name = tileSprite.name;
        newTile.transform.position = new Vector2(x + 0.5f, y + 0.5f);
        //if (noiseTexture.GetPixel(x, y).r < 0.5f) ;
    }

    public void GenerateNoiseExtra()
    {
        noiseTexture = new Texture2D(worldSize, worldSize);

        for (int x = 0; x < noiseTexture.width; x++)
        {
            for (int y = 0; y < noiseTexture.width; y++)
            {
                float v = Mathf.PerlinNoise((x + seed) * caveFreq, (y + seed) * caveFreq);
                noiseTexture.SetPixel(x, y, new Color(v,v,v));
            }
        }
        noiseTexture.Apply();
    }
   

    // Update is called once per frame
    void Update()
    {
        
    }
}
