using System;
using Unity.Burst.CompilerServices;
using UnityEngine;

namespace Workshop.Student
{
    public class MapGenerator : MonoBehaviour
    {
        public int columns = 10;
        public int rows = 10;

        public GameObject[] floorTiles;
        public GameObject[] wallTiles;
        public GameObject[] foodTiles;

        public GameObject[] obstacleTiles;



        public string[,] saveItemMap = new string[3, 3] 
        {
            { " ", "Soda", " "},
            { " ", " ", " "},
            { " ", " ", "Food"},
        };

        // 1. declare Players variable
        public GameObject[] players;
        // 7. declare Exit variable 
        public GameObject exit;

        public void Start()

        {
            

            // 1. random player at the position <0, 0> map
            int randomPlayer = UnityEngine.Random.Range(0, players.Length);
            GameObject player = Instantiate(players[randomPlayer],new Vector2(0, 0),Quaternion.identity);
            player.name = "Player";

            // 2. create obstacles (in the middle in a straight line 4,0 to 4,4)
            bool IsObstacle(int x, int y)
            {
                // Vertical obstacle from (4,0) to (4,4)
                if (x == 4 && y >= 0 && y <= 4)
                {
                    return true;
                }

                return false;
            }
            // 3. create floor
            for (int y = 0; y < rows; y++)
            {
                for (int x = 0; x < columns; x++)
                {
                    // If this position is an obstacle, create obstacle instead
                    if (IsObstacle(x, y))
                    {
                        int r = UnityEngine.Random.Range(0, obstacleTiles.Length);

                        GameObject obstacle = Instantiate(
                            obstacleTiles[r],
                            new Vector2(x, y),
                            Quaternion.identity
                        );

                        obstacle.name = "Obstacle" + x + "_" + y;

                        continue;
                    }

                    // Otherwise create normal floor
                    int floorRandom = UnityEngine.Random.Range(0, floorTiles.Length);

                    GameObject tile = Instantiate(
                        floorTiles[floorRandom],
                        new Vector2(x, y),
                        Quaternion.identity
                    );

                    tile.name = "Floor" + x + "_" + y;
                }
            }
            // 4. create walls
            for (int y = -1; y < rows+1; y++)
            {
                for (int x = -1; x < columns+1; x++)
                {
                    if (x == -1 || x == columns || y == -1 || y == rows)
                    {
                        int r = UnityEngine.Random.Range(0, wallTiles.Length);
                        GameObject tile = Instantiate(wallTiles[r], new Vector2(x, y), Quaternion.identity);
                        tile.name = "Wall" + x + "_" + y;
                    }
                }
            }
            // 5. random foods
            int numberOfFoods = UnityEngine.Random.Range(1, 3);
            for (int i = 0; i < numberOfFoods; i++)
            {
                int x_Food = UnityEngine.Random.Range(0, columns);
                int y_food = UnityEngine.Random.Range(0, rows);
                int r = UnityEngine.Random.Range(0,floorTiles.Length);
                Instantiate(foodTiles[0], new Vector2(x_Food, y_food), Quaternion.identity);
            }
            // 6. generate item along with the saveItemMap
            for(int y = 0;y < saveItemMap.GetLength(0); y++)
            {
                for(int x = 0;x < saveItemMap.GetLength(1); x++)
                {
                    string item = saveItemMap[x,y];
                    if (!string.IsNullOrEmpty(item))
                    {
                        foreach (var foodtile in foodTiles)
                        {
                            if(foodtile.name == item)
                            {
                                GameObject food = Instantiate(foodtile,new Vector2(x,y),Quaternion.identity);
                                food.name = "Food" + x + "_" + y;
                                break;
                            }
                        }
                    }
                }
            }
            // 7. place exit (create in the top right)
            GameObject exitObject = Instantiate(exit,new Vector2(columns - 1, rows - 1),Quaternion.identity);

            exitObject.name = "Exit";

            
        }
    }

}