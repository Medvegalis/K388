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

    private float currentHeight = 180;

    private void OnEnable()
    {
        moveSpeedText.text = dynamicMoveProvider.moveSpeed.ToString();
        heightText.text = cameraTransform.position.y.ToString();
    }

    public void IncreaseHeight()
    {
        float subtr = cameraTransform.position.y + heightIncramentAmount;

        if(subtr < 1)
        {
            Vector3 amount = new Vector3(0, heightIncramentAmount,0);
            cameraTransform.position += amount;
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
    }
    public void DecreaseSpeed()
    {
        float subtr = dynamicMoveProvider.moveSpeed - speedIncramentAmount;

        if (subtr > 1)
        {
            dynamicMoveProvider.moveSpeed -= speedIncramentAmount;
            moveSpeedText.text = dynamicMoveProvider.moveSpeed.ToString();
        }
    }

}
