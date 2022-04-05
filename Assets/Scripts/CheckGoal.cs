using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckGoal : MonoBehaviour
{
    int meta = 0;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "meta1") { meta = 1; }
        else if (other.tag == "meta2") { meta = 2; }
        else if (other.tag == "meta3") { meta = 3; }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "meta1" && meta == 1) { meta = 0; }
        else if (other.tag == "meta2" && meta == 2) { meta = 0; }
        else if (other.tag == "meta3" && meta == 3) { meta = 0; }
    }

    public int GetScore() { return meta; }
}
