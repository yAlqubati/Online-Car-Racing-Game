using UnityEngine;

public class AdvancedObjectPlacer : MonoBehaviour
{
    [System.Serializable]
    public class PlacableObject
    {
        public GameObject prefab; // Objeye ait prefab
        public int spacing;       // Objeler arası mesafe
        public Vector3 startPosition; // Objelerin yerleştirilmeye başlanacağı pozisyon
    }

    // Objeler için prefablar, aralıklar ve başlangıç pozisyonları
    public PlacableObject aliens;
    public PlacableObject ruins;
    public PlacableObject alienShips;
    public PlacableObject bigAlienShips;
    public PlacableObject trafficThings;
    public PlacableObject powerUps;
    public PlacableObject trees;
    public PlacableObject barrels;

    public PlacableObject abandoned;
    public PlacableObject cottage;
    public PlacableObject polewood;
    public PlacableObject alienShips_green;
    public PlacableObject alienShips_white;

    public int width = 100; // Alanın genişliği
    public int height = 100; // Alanın yüksekliği

    void Start()
    {
        // Her obje türü için objeleri yerleştir
        PlaceObjects(ruins);
        PlaceObjects(abandoned);
        PlaceObjects(cottage);
        PlaceObjects(polewood);
        PlaceObjects(alienShips_green);
        PlaceObjects(alienShips_white);
        PlaceObjects(bigAlienShips);
        PlaceObjects(trafficThings);
        PlaceObjects(powerUps);
        PlaceObjects(trees);
        PlaceObjects(barrels);
    }

    void PlaceObjects(PlacableObject placableObject)
    {
        // Prefab veya spacing geçerli değilse işlem yapma
        if (placableObject.prefab == null || placableObject.spacing <= 0)
            return;

        // Başlangıç pozisyonundan itibaren belirtilen aralıklarla objeleri yerleştir
        Vector3 position = placableObject.startPosition;
        for (int x = 0; x < width; x += placableObject.spacing)
        {
            for (int z = 0; z < height; z += placableObject.spacing)
            {
                Vector3 spawnPosition = new Vector3(position.x + x, position.y, position.z + z);
                Instantiate(placableObject.prefab, spawnPosition, Quaternion.identity);
            }
        }
    }
}
