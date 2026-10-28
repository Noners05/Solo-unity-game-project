using NUnit.Framework.Constraints;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WeepingAngel : MonoBehaviour
{
    public NavMeshAgent ai;
    public Transform player;
    Vector3 dest;
    public Camera playerCam, deathCam;
    public float aiSpeed, catchDistance, deathCamTime;
    public string deathScene;
    void Update()
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(playerCam);

        float distance = Vector3.Distance(transform.position, player.position);

        if (GeometryUtility.TestPlanesAABB(planes, this.gameObject.GetComponent<Renderer>().bounds))
        {
            ai.speed = 0;
            ai.SetDestination(transform.position);
        }


        if (!GeometryUtility.TestPlanesAABB(planes, this.gameObject.GetComponent<Renderer>().bounds))
        {
         ai.speed = aiSpeed;
         dest = player.position;
         ai.destination = dest;
            
            if(distance <= catchDistance)
            {
                player.gameObject.SetActive(false);
                deathCam.gameObject.SetActive(true);
                StartCoroutine(killPlayer());
            }
        }
    }
    IEnumerator killPlayer()
    {
        yield return new WaitForSeconds(deathCamTime);
        SceneManager.LoadScene(deathScene);
    }
}


