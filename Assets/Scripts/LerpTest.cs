// A short example of Vector3.Lerp usage.
// Add it to an object in your scene, and at Play time it will draw in the Scene View a small yellow line between the scene origin, and a position interpolated between two other positions (one on the up axis, one on the forward axis).

using UnityEngine;

public class LerpTest : MonoBehaviour
{
    public int interpolationFramesCount = 45; // Number of frames to completely interpolate between the 2 positions
    int elapsedFrames = 0;

    private void OnDrawGizmos()
    {
        // draw a 5-unit white line from the origin for 2.5 seconds
        //Gizmos.Drawline(Vector3.zero, new Vector3(5, 0, 0), Color.white, 2.5f);
        float interpolationRatio = (float)elapsedFrames / interpolationFramesCount;
        float interpolationRatiob = (float)elapsedFrames / 60;

        Vector3 interpolatedPosition = Vector3.Lerp(new Vector3(0, 1, 0), new Vector3(0, 0, 1), interpolationRatio);
        Vector3 interpolatedPositionb = Vector3.Lerp(new Vector3(0, 10, 0), new Vector3(0, 0, 1), interpolationRatiob);
        elapsedFrames = (elapsedFrames + 1) % (interpolationFramesCount + 1);  // reset elapsedFrames to zero after it reached (interpolationFramesCount + 1)


        Gizmos.color = Color.white;
        Gizmos.DrawLine(new Vector3(0,0,0), new Vector3(3.9f, 2.5f, 0));
        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector3(0,0,0), new Vector3(4.26f, -2.38f, 0));
        Gizmos.color = Color.green;
        Gizmos.DrawLine(Vector3.zero, interpolatedPositionb);
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(new Vector3(0, -5, 0), interpolatedPosition);

    }

    void Update()
    {
        float interpolationRatio = (float)elapsedFrames / interpolationFramesCount;

        Vector3 interpolatedPosition = Vector3.Lerp(new Vector3(0, 1, 0), new Vector3(0, 0, 1), interpolationRatio);

        elapsedFrames = (elapsedFrames + 1) % (interpolationFramesCount + 1);  // reset elapsedFrames to zero after it reached (interpolationFramesCount + 1)

        Debug.DrawLine(new Vector3(0,0,0), new Vector3(0,5,0), Color.green);
        Debug.DrawLine(new Vector3(0,0,0), new Vector3(0,0,10), Color.blue); 
        Debug.DrawLine(new Vector3(0,0,0), interpolatedPosition, Color.yellow);
    }
}