using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;

public class PlayerStatsEditorUI : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private DynamicMoveProvider dynamicMoveProvider;

    [SerializeField] private Text moveSpeedText;
    [SerializeField] private Text heightText;

    private float heightIncramentAmount = 0.01f;
    private float speedIncramentAmount = 1f;

    private static float currentHeight = 180;
    private static float currentSpeed = 3;
    private static float currentHeightCord = 1.4f;

    private void OnEnable()
    {
        moveSpeedText.text = dynamicMoveProvider.moveSpeed.ToString();
        heightText.text = cameraTransform.position.y.ToString();
    }

    private void Start()
    {
        dynamicMoveProvider.moveSpeed = currentSpeed;
        cameraTransform.position = new Vector3(cameraTransform.position.x, currentHeightCord, cameraTransform.position.z);
        heightText.text = currentHeight.ToString();
        moveSpeedText.text = currentSpeed.ToString();
    }

    public void IncreaseHeight()
    {
        float subtr = cameraTransform.position.y + heightIncramentAmount;
        Debug.Log("subtr = " + subtr);

        if(subtr < 1.8f)
        {
            Vector3 amount = new Vector3(0, heightIncramentAmount,0);
            cameraTransform.position += amount;
            currentHeightCord = cameraTransform.position.y;
            heightText.text = (currentHeight + 1).ToString();
            currentHeight += 1;
        }
    }
    public void DecreaseHeight()
    {
        float subtr = cameraTransform.position.y - heightIncramentAmount;

        if (subtr > -1)
        {
            Vector3 amount = new Vector3(0, heightIncramentAmount, 0);
            cameraTransform.position -= amount;
            currentHeightCord = cameraTransform.position.y;
            heightText.text = (currentHeight - 1).ToString();
            currentHeight -= 1;
        }
    }
    public void IncreaseSpeed()
    {
        float subtr = dynamicMoveProvider.moveSpeed + speedIncramentAmount;

        if (subtr < 20)
        {
            dynamicMoveProvider.moveSpeed += speedIncramentAmount;
            moveSpeedText.text = dynamicMoveProvider.moveSpeed.ToString();
        }

        currentSpeed = dynamicMoveProvider.moveSpeed;
    }
    public void DecreaseSpeed()
    {
        float subtr = dynamicMoveProvider.moveSpeed - speedIncramentAmount;

        if (subtr > 1)
        {
            dynamicMoveProvider.moveSpeed -= speedIncramentAmount;
            moveSpeedText.text = dynamicMoveProvider.moveSpeed.ToString();
        }

        currentSpeed = dynamicMoveProvider.moveSpeed;
    }

}
