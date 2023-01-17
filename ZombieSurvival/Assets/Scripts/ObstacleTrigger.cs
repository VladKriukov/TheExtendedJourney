using UnityEngine;

public class ObstacleTrigger : MonoBehaviour
{
	enum BoxSide
	{
		Left, Right
	}
	[SerializeField] BoxSide boxSide;

	AnimalAI animalAI;

	private void Awake()
	{
		animalAI = transform.parent.GetComponent<AnimalAI>();
	}

	private void OnTriggerEnter(Collider other)
	{
		if (boxSide == BoxSide.Left)
		{
			animalAI.AvoidObstacle(false);
		}
		else
		{
            animalAI.AvoidObstacle(true);
        }
	}

	private void OnTriggerExit(Collider other)
	{
		animalAI.ResumeMovement();
	}
}