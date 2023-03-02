using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//This script will only hold the function that you are going to call when needed from other scripts
// Create an IEnumerator that takes two variables
// A variable to control the duration of the shake
// Another variable to control the magnitude of the shake
// Start the method by declaring a new Vector3 originalPos to determine the camera's starting position 
//Declare a float for elapsedTime and set it to 0f
//For the shake: best do it in a while loop as long as the elapsedTime is less than the duration of the shake
//In the while loop, set two floats, one for the xOffset and one for the yOffset and multiply each by the magnitude 
//(it is good to use a random range for each offset
// Set the localPosition of the camera based on these two offsets
// finish the loop by incrementing the elapsedTime with Time.deltaTime
//Finally after the loop is done, set the localPosition to the originalPos and close the IEnumerator method
public class CameraShakeMH : MonoBehaviour
{

    public IEnumerator ShakeCam(float shakeDuration, float shakeMagnitude)
    {

        Vector3 originalPos = transform.position;

        float elapsedTime = 0f;
        float xOffset;
        float yOffset;

        while (elapsedTime < shakeDuration)
        {
            xOffset = Random.Range(-1f, 1f); yOffset = Random.Range(-1f, 1f);

            xOffset *= shakeMagnitude; yOffset *= shakeMagnitude;

            transform.position = new Vector3(xOffset, yOffset, originalPos.z);
            elapsedTime += Time.deltaTime;

            yield return null;

        }

        transform.position = originalPos;

    }
}


    
