using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EcoSimUtil;
using System.IO;

public class EnvManager : MonoBehaviour {

    [SerializeField] GameObject player;
    [SerializeField] GameObject foodNew;
    [SerializeField] float zMax = 30;
    [SerializeField] float xMax = 60;
    [SerializeField] int initCount = 20;
    [SerializeField] int population = 30;
    EnvStats stats;

    public HashSet<GameObject> foodSet;
    public List<Player> agents;
    private bool done = false;

    private string outPath = "/gen_stats.txt";
    private StreamWriter writer;
    private int gen = 0;

    void Start() {
        foodSet = new HashSet<GameObject>();
        agents = new List<Player>();
        stats = new EnvStats();
        stats.initialCount = initCount;
        stats.foodCount = initCount;
        stats.popCount = population;
        SpawnFood();
        SpawnFirstPopulation();
        outPath = Application.dataPath + outPath;
        writer = File.AppendText(outPath);
    }


    void Update() {
        Debug.Log(stats.foodCount);
        if (stats.foodCount <= 0) {
            newGeneration();
        }
    }

    private List<OrgStats> GetNewStats(List<Player> player) {
        //Create a mating pool
        List<OrgStats> pool = new List<OrgStats>();
        for (int i = 0; i < player.Count; ++i) {
            int count = player[i].Score();
            OrgStats orgStats = player[i].Stats();
            //Repeatedly add a gene according to its score
            for (int j = 0; j < count; ++j) {
                pool.Add(orgStats);
            }
        }
        List<OrgStats> newGen = new List<OrgStats>();
        //Generate a new population from the gene pool
        for (int i = 0; i < stats.popCount; ++i) {
            OrgStats f = pool[Random.Range(0, pool.Count)];
            OrgStats s = pool[Random.Range(0, pool.Count)];
            OrgStats child = OrgStats.FromGene(OrgStats.CrossAvg(f, s, 0.1f, 20));
            newGen.Add(child);
        }
        return newGen;
    }

    private void SpawnFood() {
        for (int i = 0; i < stats.initialCount; ++i) {
            Vector2 pos = Random.insideUnitCircle * xMax;
            foodSet.Add(
            Instantiate(
                foodNew,
                new Vector3(pos.x, 1, pos.y),
                Quaternion.identity
            ));
        }
    }

    private void SpawnFirstPopulation() {
        float angleDiff = 360 / stats.popCount;
        for (int i = 0; i < stats.popCount; ++i) {
            Vector3 pos = Constants.posAround(90, angleDiff * i, 3);
            Vector3 dir = (Vector3.zero - pos);
            agents.Add(
                Player.Create(stats, player, pos, dir, Constants.NewStats(1)[0])
            );
        }
    }

    private void SpawnGeneratedPopulation(List<OrgStats> orgStats) {
        float angleDiff = 360 / stats.popCount;
        for (int i = 0; i < stats.popCount; ++i) {
            Vector3 pos = Constants.posAround(90, angleDiff * i, 3);
            Vector3 dir = (Vector3.zero - pos);
            agents.Add(
                Player.Create(stats, player, pos, dir, orgStats[i])
            );
        }
    }

    class AgentComparer: IComparer<Player> {
        public int Compare(Player p1, Player p2) {
            return -p1.Score().CompareTo(p2.Score());
        }
    }

    /*IEnumerator*/ private void newGeneration() {
        
        //Sort agents by score
        agents.Sort(new AgentComparer());
        
        //Write data of each generation line by line 
        for (int i = 0; i < agents.Count; ++i) {
            string line = gen + " " + agents[i].Score() + " " + stats.initialCount + " ";
            float[] gene = agents[i].Stats().Gene();
            for (int j = 0; j < gene.Length; ++j) {
                line = line + gene[j].ToString("0.00") + " ";
            }
            writer.WriteLine(line);
        }
        
        //Generate a new population
        List<OrgStats> children = GetNewStats(agents);
        foreach(Player player in agents) {
            if (player != null) player.End();
        }
        agents.Clear();
        
        //Start Spawning
        foreach(GameObject f in foodSet) {
            if (f != null)
                Destroy(f);
        }
        foodSet.Clear();
        stats = new EnvStats();
        stats.initialCount = initCount;
        stats.foodCount = initCount;
        stats.popCount = population;
        SpawnFood();
        SpawnGeneratedPopulation(children);
        ++gen;
        //yield return new WaitForSecondsRealtime(2);
    }

    private void OnApplicationQuit() {
        writer.Close();
    }
}
