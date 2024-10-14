using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamage 
{
    // Start is called before the first frame update
    // Start is called before the first frame update

    // Update is called once per frame
    void takeDamage(int amount);
    void gainHealth(int amount);

}
