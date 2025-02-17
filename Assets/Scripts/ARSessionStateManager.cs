using UnityEngine;
using UnityEngine.XR.ARFoundation;
using System.Collections;

public class ARSessionStateManager : MonoBehaviour
{
    [SerializeField]
    private ARSession arSession;
    
    public delegate void ARSessionStateChanged(ARSessionState state);
    public event ARSessionStateChanged onSessionStateChanged;

    private void Start()
    {
        if (arSession == null)
            arSession = FindObjectOfType<ARSession>();

        ARSession.stateChanged += HandleSessionStateChanged;
    }

    private void OnDestroy()
    {
        ARSession.stateChanged -= HandleSessionStateChanged;
    }

    private void HandleSessionStateChanged(ARSessionStateChangedEventArgs args)
    {
        onSessionStateChanged?.Invoke(args.state);

        switch (args.state)
        {
            case ARSessionState.None:
            case ARSessionState.CheckingAvailability:
                // Waiting for system initialization
                break;
            case ARSessionState.NeedsInstall:
                // Need to install AR support
                StartCoroutine(RequestARInstall());
                break;
            case ARSessionState.Installing:
                // Installing
                break;
            case ARSessionState.Ready:
                // AR system ready
                arSession.enabled = true;
                break;
            case ARSessionState.SessionTracking:
                // AR session tracking
                break;
        }
    }

    private IEnumerator RequestARInstall()
    {
        yield return ARSession.CheckAvailability();
        if (ARSession.state == ARSessionState.NeedsInstall)
        {
            yield return ARSession.Install();
        }
    }
}