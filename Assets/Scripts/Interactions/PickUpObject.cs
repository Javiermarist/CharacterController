using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PickUpObject : MonoBehaviour
{
    public Transform backpack;
    private GameObject pickObject;

    private void Start()
    {
        pickObject = null;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.CompareTag("PickObject") && pickObject == null)
        {
            hit.transform.position = backpack.transform.position;
            // Le asignamos como parent el transform del player
            hit.transform.parent = this.transform;
            pickObject = hit.collider.gameObject;
        }
    }

    public void OnDropObject(InputValue value)
    {
        if (pickObject != null)
        {
            // Como el objeto no tiene Rigidbody hacemos esto
            RaycastHit hit;
            if (Physics.Raycast(pickObject.transform.position, Vector3.down, out hit))
            {
                pickObject.transform.position = new Vector3(pickObject.transform.position.x, hit.point.y + .28f, pickObject.transform.position.z);
                pickObject.transform.parent = null;
                //pickObject = null;
                // En vez de usar el pickObject == null; usamos una corrutina para que espere antes de volver a poder recoger objetos
                StartCoroutine(EsperarParaPonerNull());
            }
        }
        // Si tiene Rigidbody, esto nos funciona
        /*{
            pickObject.transform.parent = null;
            pickObject = null;
            StartCoroutine(EsperarParaPonerNull());
        }*/
    }

    private IEnumerator EsperarParaPonerNull()
    {
        yield return new WaitForSeconds(1f);
        pickObject = null;
    }
}
