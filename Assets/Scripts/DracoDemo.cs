using System.IO;
using UnityEngine;
using Draco;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class DracoDemo : MonoBehaviour {

    public string filePath;

    async void Start() {

        // Load file into memory
        var fullPath = Path.Combine(Application.streamingAssetsPath, filePath);
        var data = File.ReadAllBytes(fullPath);

        // Convert data to Unity mesh
        var draco = new DracoMeshLoader();
        // Async decoding has to start on the main thread and spawns multiple C# jobs.
        var mesh = await draco.ConvertDracoMeshToUnity(data, false, false, 3, 0);

        // Print bone weight data for debugging...

        Debug.Log(string.Join("\t",
            "index0", "index1", "index2", "index3",
            "weight0", "weight1", "weight2", "weight3"));

        foreach (var entry in mesh.boneWeights)
            Debug.LogFormat("{0:D}\t{1:D}\t{2:D}\t{3:D}\t{4:F}\t{5:F}\t{6:F}\t{7:F}",
                entry.boneIndex0, entry.boneIndex1, entry.boneIndex2, entry.boneIndex3,
                entry.weight0, entry.weight1, entry.weight2, entry.weight3);

        // in case you want to dump the values to disk
#if false
		using (var writer = new StreamWriter("D:/tmp/bone-weights.tsv"))
		{
			writer.WriteLine(string.Join("\t",
				"index0", "index1", "index2", "index3",
				"weight0", "weight1", "weight2", "weight3"));

			foreach (var entry in mesh.boneWeights)
				writer.WriteLine("{0:D}\t{1:D}\t{2:D}\t{3:D}\t{4:F}\t{5:F}\t{6:F}\t{7:F}",
					entry.boneIndex0, entry.boneIndex1, entry.boneIndex2, entry.boneIndex3,
					entry.weight0, entry.weight1, entry.weight2, entry.weight3);
		}
#endif

        if (mesh != null) {
            // Use the resulting mesh
            GetComponent<MeshFilter>().mesh= mesh;
        }
    }
}
