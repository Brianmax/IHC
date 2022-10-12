using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;

public class AssetProcessor : AssetPostprocessor
{
    void OnPreprocessModel()
    {
        ModelImporter modelImporter = base.assetImporter as ModelImporter;
        if (modelImporter != null)
        {
            //modelImporter.importMaterials = false;
            //modelImporter.materialName = ModelImporterMaterialName.BasedOnMaterialName;
            //modelImporter.generateSecondaryUV = true;
        }
    }

    void OnPostprocessModel(GameObject gameObject)
    {
        gameObject.isStatic = true;

        if (gameObject.GetComponent<Animation>() || gameObject.GetComponent<Animator>())
            gameObject.isStatic = false;
    }
}
