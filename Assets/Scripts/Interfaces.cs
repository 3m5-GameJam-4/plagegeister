using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IAudio {
    void PlayAudio(string type);
    // list of strings:
    // attackEnemy
    // attackSlime
    // dieSwarm
    // dieEnemy
    // collected
}