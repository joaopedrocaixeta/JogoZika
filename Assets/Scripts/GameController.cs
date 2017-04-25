using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    public Estado estado;

    public float espera;
    public GameObject obstaculo;
    public float tempoDestruicao;

    public GameObject menu;
    public GameObject painelMenu;

    public Text txtPontos;
    public Text txtMaiorPontuacao;
    public int pontos;

    public GameObject gameOverPanel;
    public GameObject pontosPanel;

    public static GameController instancia = null;

    private List<GameObject> obstaculos;

    void Awake () {
        if(instancia == null){
            instancia = this;
        }
        else if (instancia != null){
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
	}

    void Start(){
        obstaculos = new List<GameObject>();
        estado = Estado.AguardoComecar;
        PlayerPrefs.SetInt("HighScore", 0);
        menu.SetActive(true);
        painelMenu.SetActive(true);
        gameOverPanel.SetActive(false);
        pontosPanel.SetActive(false);
    }

    IEnumerator DestruirObstaculo(GameObject obj)
    {
        yield return new WaitForSeconds(tempoDestruicao);
        if (obstaculos.Remove(obj))
        {
            Destroy(obj);
        }
    }

    IEnumerator GerarObstaculos(){
        while (GameController.instancia.estado == Estado.Jogando){
            Vector3 pos = new Vector3(17f, Random.Range(-3f, 9f), 7.8f);
            GameObject obj = Instantiate(obstaculo, pos, Quaternion.Euler(0f,180f,0)) as GameObject;
            obstaculos.Add(obj);
            StartCoroutine(DestruirObstaculo(obj));
            yield return new WaitForSeconds(espera);
        }
        yield return null;
    }

    public void PlayerComecou(){
        estado = Estado.Jogando;
        menu.SetActive(false);
        painelMenu.SetActive(false);
        pontosPanel.SetActive(true);
        atualizarPontos(0);
        StartCoroutine(GerarObstaculos());
    }

    public void PlayerMorreu(){
        estado = Estado.GameOver;
        if (pontos > PlayerPrefs.GetInt("HighScore")){
            PlayerPrefs.SetInt("HighScore", pontos);
            txtMaiorPontuacao.text = "" + pontos;
        }
        gameOverPanel.SetActive(true);
    }

    public void PlayerVoltou(){
        while (obstaculos.Count > 0){
            GameObject obj = obstaculos[0];
            if (obstaculos.Remove(obj)){
                Destroy(obj);
            }
        }
        estado = Estado.AguardoComecar;
        menu.SetActive(true);
        painelMenu.SetActive(true);
        gameOverPanel.SetActive(false);
        pontosPanel.SetActive(false);
        GameObject.Find("meninoassustado").GetComponent<PlayerController>().recomecar();
    }

    private void atualizarPontos(int x){
        pontos = x;
        txtPontos.text = "" + x;
    }

    public void acrescentarPontos(int x){
        atualizarPontos(pontos + x);
    }
}
