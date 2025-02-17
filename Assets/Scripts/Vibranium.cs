using UnityEngine;
using Unity.Mathematics;
using Unity.Collections;
using Unity.Jobs;

public class Vibranium : MonoBehaviour 
{
    [Header("Sphere Settings")]
    public GameObject sphere;
    public int noSpheresX = 20;
    public int noSpheresY = 3;
    public int noSpheresZ = 3;
    public float size = 1.0f;
    
    [Header("Animation Settings")]
    public float sizePerlin = 0.5f;
    public float ampPerlin = 0.5f;
    public float freqPerlin = 0.5f;
    public float speedPerlin = 1.0f;
    public float ampVibrate = 0.5f;
    public float speedVibrate = 0.5f;
    public float phaseVibrate = 0.1f;

    [Header("Materials")]
    public Material mat1;
    public Material mat2;
    public Material mat3;

    private GameObject[] spheres;
    private NativeArray<float3> basePositions;
    private NativeArray<float> scales;
    private ComputeBuffer positionBuffer;
    private int totalSpheres;

    void Start()
    {
        totalSpheres = noSpheresX * noSpheresY * noSpheresZ;
        spheres = new GameObject[totalSpheres];
        basePositions = new NativeArray<float3>(totalSpheres, Allocator.Persistent);
        scales = new NativeArray<float>(totalSpheres, Allocator.Persistent);

        InitializeSpheres();
    }

    private void InitializeSpheres()
    {
        int i = 0;
        for (int x = 0; x < noSpheresX; x++)
        {
            for (int y = 0; y < noSpheresY; y++)
            {
                for (int z = 0; z < noSpheresZ; z++)
                {
                    spheres[i] = Instantiate(sphere, transform);
                    basePositions[i] = new float3(x, y, z);
                    spheres[i].transform.localPosition = new Vector3(x, y, z);
                    spheres[i].transform.localScale = Vector3.one * size;

                    var renderer = spheres[i].GetComponent<Renderer>();
                    renderer.material = z < 2 ? mat3 : (z < 5 ? mat2 : mat1);

                    i++;
                }
            }
        }
    }

    void Update()
    {
        float time = Time.time;
        JobHandle handle = new JobHandle();

        for (int i = 0; i < totalSpheres; i++)
        {
            float3 basePos = basePositions[i];
            float3 pos = basePos;

            // Perlin noise displacement
            pos += CalculatePerlinDisplacement(basePos, time);

            // Scale calculation
            float scaleP = 1f + sizePerlin * Perlin.Noise(
                basePos.x * freqPerlin + speedPerlin * time,
                basePos.y * freqPerlin,
                basePos.z * freqPerlin + 121.3f
            );

            // Vibration
            pos += CalculateVibration(scaleP, time, i);

            // Apply transformations
            if (spheres[i] != null)
            {
                spheres[i].transform.localPosition = pos;
                spheres[i].transform.localScale = Vector3.one * size * scaleP;
            }
        }
    }

    private float3 CalculatePerlinDisplacement(float3 basePos, float time)
    {
        return new float3(
            ampPerlin * Perlin.Noise(basePos.x * freqPerlin + speedPerlin * time, basePos.y * freqPerlin, basePos.z * freqPerlin),
            ampPerlin * Perlin.Noise(basePos.x * freqPerlin + speedPerlin * time, basePos.y * freqPerlin, basePos.z * freqPerlin + 13.2f),
            ampPerlin * Perlin.Noise(basePos.x * freqPerlin + speedPerlin * time, basePos.y * freqPerlin, basePos.z * freqPerlin + 49.9f)
        );
    }

    private float3 CalculateVibration(float scaleP, float time, int index)
    {
        float scaleFactor = (0.9f * scaleP + 0.1f) * ampVibrate;
        float phase = speedVibrate * time + phaseVibrate * index;
        
        return new float3(
            scaleFactor * math.sin(phase),
            scaleFactor * math.cos(phase),
            scaleFactor * math.cos(phase + 0.5f * math.PI)
        );
    }

    void OnDestroy()
    {
        if (basePositions.IsCreated) basePositions.Dispose();
        if (scales.IsCreated) scales.Dispose();
    }
}