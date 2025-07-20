using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
public class AnimateImageMaterial : MonoBehaviour
{
    [SerializeField] private bool executeAlways = true;
    [SerializeField] private bool createMaterialInstance = true;
    [SerializeField] private string property1; // Values in arrays and lists cannot be animated
    [SerializeField] private float value1;
    [SerializeField] private Material material;

    private Image image;
    private bool isPlayingInScene;
    private float startValue;

    private void Awake()
    {
        // Application.isPlaying isn't enough because it's true when editing prefab while in play mode
        isPlayingInScene = gameObject.scene.isLoaded && Application.isPlaying;
        if (!isPlayingInScene && !executeAlways) return;
        material = null;
    }

    private void OnEnable()
    {
        if (!isPlayingInScene && !executeAlways) return;

        image = GetComponent<Image>();
        if (isPlayingInScene && material == null && createMaterialInstance)
        {
            material = Instantiate(image.material);
            image.material = material;
        }
        else material = image.material;
    }

    private void Start()
    {
        if (material) startValue = material.GetFloat(property1);
    }

    // Undocumented monobehaviour method
    private void OnDidApplyAnimationProperties()
    {
        if (!Application.isPlaying && !executeAlways) return;
        if (material) material.SetFloat(property1, value1);
    }

    private void OnDisable()
    {
        if (material) material.SetFloat(property1, startValue);
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        OnDidApplyAnimationProperties();
    }
#endif
}
