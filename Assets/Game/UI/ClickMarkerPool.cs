using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

public class ClickMarkerPool : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private UIDocument uiDocument;
    [SerializeField] private VisualTreeAsset markerTemplate;

    [Header("Pool")]
    [SerializeField] private int initialPoolSize = 10;

    [Header("Marker")]
    [SerializeField] private float markerLifetime = 0.5f;

    private VisualElement _root;

    private readonly Queue<TemplateContainer> _availableMarkers = new();
    private readonly List<TemplateContainer> _allMarkers = new();

    private void Awake()
    {
        if (uiDocument == null)
        {
            Debug.LogError("UIDocument is missing.", this);
            enabled = false;
            return;
        }

        if (markerTemplate == null)
        {
            Debug.LogError("Marker template is missing.", this);
            enabled = false;
            return;
        }

        _root = uiDocument.rootVisualElement;

        CreateInitialPool();
    }

    private void OnEnable()
    {
        _root.RegisterCallback<PointerDownEvent>(OnPointerDown);
    }

    private void OnDisable()
    {
        _root.UnregisterCallback<PointerDownEvent>(OnPointerDown);
    }

    private void CreateInitialPool()
    {
        for (int i = 0; i < initialPoolSize; i++)
        {
            TemplateContainer marker = CreateMarker();

            marker.style.display = DisplayStyle.None;

            _availableMarkers.Enqueue(marker);
            _allMarkers.Add(marker);
        }
    }

    private TemplateContainer CreateMarker()
    {
        TemplateContainer marker = markerTemplate.Instantiate();

        marker.pickingMode = PickingMode.Ignore;
        marker.style.position = Position.Absolute;

        _root.Add(marker);

        return marker;
    }

    public void OnPointerDown(PointerDownEvent evt)
    {
        ShowMarker(evt.localPosition);
    }

    public void ShowMarker(Vector2 position)
    {
        TemplateContainer marker = GetMarker();

        marker.style.left = position.x - (marker.resolvedStyle.backgroundSize.x.value / 2 );
        marker.style.top = position.y - (marker.resolvedStyle.backgroundSize.y.value / 2 );
        marker.style.display = DisplayStyle.Flex;

        StartCoroutine(ReturnMarkerAfterDelay(marker));
    }

    private TemplateContainer GetMarker()
    {
        if (_availableMarkers.Count > 0)
        {
            return _availableMarkers.Dequeue();
        }

        TemplateContainer marker = CreateMarker();
        _allMarkers.Add(marker);

        return marker;
    }

    private IEnumerator ReturnMarkerAfterDelay(
        TemplateContainer marker)
    {
        yield return new WaitForSeconds(markerLifetime);

        marker.style.display = DisplayStyle.None;

        _availableMarkers.Enqueue(marker);
    }
}