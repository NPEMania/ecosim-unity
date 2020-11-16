using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EcoSimUtil {
    
    public class Constants
    {
        public static float MAX_SPEED = 25;
        public static float MAX_RANGE = 25;
        public static float MAX_ANGLE = 160; // in degrees
        public static float MAX_RS = 300;

        public static float MIN_SPEED = 10;
        public static float MIN_RANGE = 8;
        public static float MIN_ANGLE = 90;
        public static float MIN_RS = 150;

        public static float[] MAXES = new float[] {
            MAX_ANGLE, MAX_RANGE, MAX_SPEED, MAX_RS
        };

        public static float[] MINS = new float[] {
            MIN_ANGLE, MIN_RANGE, MIN_SPEED, MIN_RS
        };

        public static string WALL_TAG = "walls";

        public static Vector3 dirFromAngle(float degrees) {
            return new Vector3(Mathf.Sin(degrees * Mathf.Deg2Rad), 0, Mathf.Cos(degrees * Mathf.Deg2Rad));
        }

        public static Vector3 dir(Vector3 initial, Vector3 final) {
            return (final - initial).normalized;
        }

        public static Vector3 posAroundRadius(float radius, float y) {
            float angle = Random.Range(0, 359);
            return radius * new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), y, Mathf.Cos(angle * Mathf.Deg2Rad));
        }

        public static Vector3 posAround(float radius, float angle, float y) {
            return new Vector3(radius * Mathf.Sin(angle * Mathf.Deg2Rad), 3, radius * Mathf.Cos(angle * Mathf.Deg2Rad));
        }

        public static OrgStats[] NewStats(int size) {
            OrgStats[] stats = new OrgStats[size];
            for (int i = 0; i < size; ++i) {
                stats[i] = new OrgStats();
                stats[i].angle = Random.Range(MIN_ANGLE, MAX_ANGLE);
                stats[i].range = Random.Range(MIN_RANGE, MAX_RANGE);
                stats[i].speed = Random.Range(MIN_SPEED, MAX_SPEED);
                stats[i].rSpeed = Random.Range(MIN_RS, MAX_RS);
            }
            return stats;
        }
    }

    public class OrgStats    {
        public float angle, range, speed, rSpeed;

        public OrgStats(float a, float r, float s, float rs) {
            speed = s; range = r; angle = a; rSpeed = rs;
        }

        public OrgStats() {}

        public static OrgStats FromGene(float[] gene) {
            return new OrgStats(gene[0], gene[1], gene[2], gene[3]);
        }

        public float[] Gene() {
            float[] gene = new float[4];
            gene[0] = angle; gene[1] = range; gene[2] = speed; gene[3] = rSpeed;
            return gene;
        }

        public static float[] CrossAvg(OrgStats f, OrgStats s, float mutRate, int mutChances) {
            float[] gene = new float[4];
            float[] first = f.Gene(), second = s.Gene();
            for (int i = 0; i < 4; ++i) {
                gene[i] = (first[i] + second[i]) / 2;
            }
            return Mutate(gene, mutRate, mutChances);
        }

        public static float[] CrossAlternate(OrgStats f, OrgStats s, bool start, float mutRate, int mutChances) {
            float[] gene = new float[4];
            float[] first = f.Gene(), second = s.Gene();
            for (int i = 0; i < 4; ++i, start = !start) {
                if (start) {
                    gene[i] = first[i];
                } else {
                    gene[i] = second[i];
                }
            }
            return Mutate(gene, mutRate, mutChances);
        }

        public static float[] Mutate(float[] gene, float mutRate, int mutChances) {
            //mutRate is percentage [0, 1];
            for (int i = 0; i < 4; ++i) {
                int r = Random.Range(1, mutChances);
                if (r == 1) {
                    r = Random.Range(1, 20) % 2;
                    if (r == 0) {
                        if (gene[i] * (1 + mutRate) < Constants.MAXES[i])
                            gene[i] += gene[i] * mutRate;
                    } else {
                        if (gene[i] * (1 - mutRate) > Constants.MINS[i])
                            gene[i] -= gene[i] * mutRate;
                    }
                }
            }
            return gene;
        }

        public float[] ToGene() {
            return new float[] { angle, range, speed, rSpeed };
        }
    }

    public class EnvStats {
        public int initialCount;
        public int foodCount;
        public int popCount;

        public EnvStats() {
            Debug.Log("created new envstats");
        }

        public void DecFood() { 
            --foodCount;
        }
    }
}
