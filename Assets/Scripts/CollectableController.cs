using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableController : MonoBehaviour {
    public GameController _gameController;
    private GameObject _particleEffectPrefab;
    private float _maxHealth = 4;
    private float _health = 4;
    private Transform _model;

    private Vector3 _initScale;
    private IAudio _myAudioInterfacee;

    private bool isDead = false;

    void Start() {
        _gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        _particleEffectPrefab = _gameController.ParticleEffectPrefab;
        _model = transform.GetChild(0);
        _initScale = _model.localScale;
        _myAudioInterfacee = GetComponent<IAudio>();
        _maxHealth = _gameController.gatherableMaxHealth;
        _health = _maxHealth;
    }

    private void OnTriggerStay(Collider other) {
        Debug.Log(other.gameObject.layer);
        if (
            other.gameObject.layer == LayerMask.NameToLayer("Player1")
            || other.gameObject.layer == LayerMask.NameToLayer("Player2")
        ) {
            if (_health > 0) {
                _health -= Time.deltaTime;
                _model.localScale = _initScale * (_health / _maxHealth);
            }
            else {
                if (isDead == false) {
                    isDead = true;
                    // award Res & ScorePoints
                    _gameController.Res++;
                    _gameController.Score++;

                    // Instantiate particle effect gameObject
                    Instantiate(_particleEffectPrefab, this.transform.position, Quaternion.identity);

                    // play audio
                    _myAudioInterfacee?.PlayAudio("collected");

                    // destroy object after particle effect ended
                    Destroy(this.gameObject);
                }
            }
        }
    }
}