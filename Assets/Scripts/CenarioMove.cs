using UnityEngine;
using System.Collections;

public class CenarioMove : MonoBehaviour {

    public float velocidade = 1f;
    public float limite;
    public float retorno;

	void Start () {
	}
	
	void Update () {
        if (GameController.instancia.estado == Estado.Jogando || GameController.instancia.estado == Estado.AguardoComecar){
            Vector3 velocidadeVetorial = Vector3.left * velocidade;

            transform.position = transform.position + velocidadeVetorial * Time.deltaTime;

            if (transform.position.x <= limite) {
                transform.position = new Vector3(retorno, transform.position.y, transform.position.z);
            }
        }
	}
}
